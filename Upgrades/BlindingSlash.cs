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
using Il2CppAssets.Scripts.Models.Towers.Filters;
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
  class BlindingSlash : UpgradePlusPlus<NinjaPath>
  {
    public override int Cost => 10500;
    public override int Tier => 4;
    public override string Icon => GetTextureGUID(Name + "-Icon");
    public override string Portrait => GetTextureGUID(Name + "-Portrait");

    public override string Description => "Increased stats and deadliness all-round. Sword attacks can now pop lead bloons.\nFlash Step ability upgrade: the ninja draws their blade with dazzling speed and vigor, stunning and ravaging all bloon types in the near vicinity as they teleport. Distraction upgrade increases stun effectiveness.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
      if (IsHighestUpgrade(towerModel))
      {
        towerModel.ApplyDisplay<Displays.BlindingSlashDisplay>();
      }
      foreach (var a in towerModel.GetAttackModels())
      {
        foreach (var w in a.weapons)
        {
          switch(a.name)
          {
            case "AttackModel_Attack_":
            case "AttackModel_Caltrops_":
              w.projectile.GetDamageModel().damage++;
              w.rate *= 0.8f;
              w.projectile.pierce += 9.0f;
              break;
            case "AttackModel_Attack Katana_":
              w.projectile.GetDamageModel().immuneBloonProperties &= ~BloonProperties.Lead;
              w.projectile.GetDamageModel().damage++;
              w.rate *= 0.8f;
              w.projectile.pierce += 11.0f;
              a.range -= 8.0f; // counteract IncreaseRange
              break;
          }
        }
      }
      towerModel.IncreaseRange(8.0f);

      AttackModel abilityAttack = Game.instance.model.GetTowerFromId("TackShooter-400").GetAttackModel().Duplicate();
      abilityAttack.name = "AttackModel_BlindingSlash";
      abilityAttack.fireWithoutTarget = true;
      abilityAttack.weapons[0].fireWithoutTarget = true;
      abilityAttack.weapons[0].fireBetweenRounds = false;
      abilityAttack.weapons[0].projectile.radius = 30.0f;
      abilityAttack.weapons[0].projectile.pierce = 100.0f;
      abilityAttack.weapons[0].projectile.SetHitCamo(true);
      abilityAttack.weapons[0].projectile.hasDamageModifiers = true;
      abilityAttack.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
      abilityAttack.weapons[0].projectile.GetDamageModel().damage = 5f;
      abilityAttack.weapons[0].projectile.GetDamageModel().name = "DamageModel_BlindingSlash";
      abilityAttack.weapons[0].projectile.collisionPasses = new int[2] { -1, 0 };
      abilityAttack.weapons[0].projectile.id = "BlindingSlash";
      abilityAttack.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel(
        "DamageModifierForTagModel_BlindingSlash", "Ceramic", 1.0f, 10.0f, false, false));
      abilityAttack.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel(
        "DamageModifierForTagModel_BlindingSlash", "Moabs", 1.0f, 95.0f, false, false));
      // Give slash attack decamo if counter-espionage is purchased
      if (towerModel.tiers[1] > 1)
      {
        abilityAttack.weapons[0].projectile.AddBehavior(towerModel.GetDescendant<RemoveBloonModifiersModel>().Duplicate());
      }
      SlowModel stun = Game.instance.model.GetTowerFromId("BombShooter-500").GetDescendant<SlowModel>().Duplicate();
      stun.lifespanFrames = towerModel.tiers[1] > 0 ? 75 : 60;
      abilityAttack.weapons[0].projectile.AddBehavior(stun);

      var ability = towerModel.GetAbilities().Find(a => a.name == "AbilityModel_FlashStepAbility");
        ability.AddBehavior(new ActivateAttackModel("ActivateAttackModel_BlindingSlash", 0.35f, true,
        new AttackModel[] { abilityAttack }, false, false, false, false, false));
      ability.icon = GetSpriteReference(Name + "-Icon");

      // Reonstruct Shadow Clone tower model to match upgraded tower
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
      // Tower expire
      shadowTower.AddBehavior(new TowerExpireModel("TowerExpireModel_", 180f, 2, false, false));
      shadowTower.name = "ShadowClone";
      shadowTower.displayScale = 0.8f;


      towerModel.GetAbilities().Find(a => a.name == "AbilityModel_Shadow Clone_").GetBehavior<ActivateAttackModel>().attacks[0]
        .weapons[0].GetDescendant<CreateTowerModel>().tower = shadowTower;

    }
  }
}
