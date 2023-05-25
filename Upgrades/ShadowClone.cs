using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppSystem.IO;
using PathsPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackDuckNinjaPath.Upgrades
{
  class ShadowClone : UpgradePlusPlus<NinjaPath>
  {
    public override int Cost => 2175;
    public override int Tier => 3;
    public override string Icon => GetTextureGUID(Name + "-Icon");
    public override string Portrait => GetTextureGUID(Name + "-Portrait");

    public override string Description => "Throws sharp kunai which have high pierce and can pop frozen bloons.\nAbility: Uses an ancient art to spawn a shadow clone nearby which has all the same attacks as the original and lasts for 2 rounds.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
      if (IsHighestUpgrade(towerModel))
      {
        towerModel.ApplyDisplay<Displays.ShadowCloneDisplay>();
      }
      foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
      {
        damageModel.immuneBloonProperties &= ~BloonProperties.Frozen;

      }
      foreach (var a in towerModel.GetAttackModels())
      {
        foreach (var w in a.weapons)
        {
          switch(a.name)
          {
            case "AttackModel_Caltrops_":
              w.projectile.pierce += 2.0f;
              break;
            case "AttackModel_Attack_":
              w.projectile.ApplyDisplay<Displays.Projectiles.KunaiDisplay>();
              w.projectile.RemoveBehavior<RotateModel>();
              w.projectile.GetBehavior<TravelStraitModel>().speed *= 1.25f;
              w.projectile.pierce += 5.0f;
              break;
            case "AttackModel_Attack Katana_":
              w.projectile.pierce += 5.0f;
              break;
          }
        }
      }

      // Ability

      // Grab tower spawner ability from Ezili's totem
      AbilityModel ability = Game.instance.model.GetTowerFromId("Ezili 7")
        .GetAbilities().Find(a => a.name == "AbilityModel_TotemAbility").Duplicate();
      ability.livesCost = 0;
      ability.name = "AbilityModel_Shadow Clone_";
      ability.displayName = "Shadow Clone";
      ability.description = "Spawn a shadow clone nearby which has all the same attacks as the original and lasts for 2 rounds.";
      ability.icon = GetSpriteReference(Name + "-Icon");
      ability.cooldownFrames = 2700;
      
      //ability.icon = GetSpriteReference(VanillaSprites.AscendedShadowSeekingShuriken);

      // Replace Ezili's totem placement with Engi's sentry placement
      //AttackModel abilityAttack = Game.instance.model.GetTowerFromId("EngineerMonkey-100")
      //  .GetAttackModels().Find(m => m.name == "AttackModel_Spawner_").Duplicate();

      AttackModel abilityAttack = ability.GetBehavior<ActivateAttackModel>().attacks[0];

      RandomPositionModel targetting =Game.instance.model.GetTowerFromId("EngineerMonkey-100").GetAttackModels()
        .Find(m => m.name == "AttackModel_Spawner_").GetBehavior<RandomPositionModel>().Duplicate();
      abilityAttack.behaviors = new AttackBehaviorModel[] { targetting };
      abilityAttack.targetProvider = targetting;

      // Construct Shadow Clone tower model
      TowerModel shadowTower = towerModel.Duplicate();
      shadowTower.RemoveBehaviors<AbilityModel>();
      // Converting to subtower
      shadowTower.dontDisplayUpgrades = true;
      shadowTower.cost = 0;
      shadowTower.isSubTower = true;
      shadowTower.footprint.doesntBlockTowerPlacement = true;
      foreach (var att in shadowTower.GetAttackModels())
      {
        foreach (var targetType in att.weapons[0].GetBehaviors<TargetSupplierModel>())
        {
          targetType.isOnSubTower = true;
        }
      }
      shadowTower.AddBehavior(new SavedSubTowerModel("SavedSubTowerModel_"));
      shadowTower.AddBehavior(new CreditPopsToParentTowerModel("CreditPopsToParentTowerModel_"));
      shadowTower.AddBehavior(Game.instance.model.GetTower(TowerType.Sentry)
        .GetBehavior<Il2CppAssets.Scripts.Models.Towers.Behaviors.CreateEffectOnExpireModel>().Duplicate());
      //shadowTower.tier = 0;
      //shadowTower.tiers = new int[] { 0, 0, 0 };
      //shadowTower.upgrades.Clear();
      shadowTower.AddBehavior(new TowerExpireModel("TowerExpireModel_", 180f, 2, false, false));
      shadowTower.name = "ShadowClone";
      shadowTower.displayScale = 0.8f;


      abilityAttack.weapons[0].GetDescendant<CreateTowerModel>().tower = shadowTower;
      abilityAttack.weapons[0].GetDescendant<CreateTowerModel>().useParentTargetPriority = true;


      towerModel.AddBehavior(ability);
    }
  }
}
