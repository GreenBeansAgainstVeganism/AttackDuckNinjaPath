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
  class GhostWarriorLegion : UpgradePlusPlus<NinjaPath>
  {
    public override int Cost => 45000;
    public override int Tier => 5;
    public override string Icon => VanillaSprites.GhostsUpgradeFxIcon;

    public override string Description => "Increased power on all attacks. Shadow Clones now summon 3 at a time and last an additional round.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
      if (IsHighestUpgrade(towerModel))
      {
        towerModel.ApplyDisplay<Displays.GhostWarriorLegionDisplay>();
      }
      foreach (var a in towerModel.GetAttackModels())
      {
        foreach (var w in a.weapons)
        {
          switch(a.name)
          {
            case "AttackModel_Attack_":
              w.projectile.hasDamageModifiers = true;
              w.projectile.AddBehavior(new DamageModifierForTagModel(
                "DamageModifierForTagModel_", "Ceramic, Moabs", 1.0f, 1.0f, false, false));
              w.projectile.pierce += 8.0f;
              w.projectile.ApplyDisplay<Displays.Projectiles.GhostKunaiDisplay>();
              break;
            case "AttackModel_Caltrops_":
              w.projectile.hasDamageModifiers = true;
              w.projectile.AddBehavior(new DamageModifierForTagModel(
                "DamageModifierForTagModel_", "Ceramic, Moabs", 1.0f, 1.0f, false, false));
              break;
            case "AttackModel_Attack Katana_":
              w.projectile.GetDamageModel().damage++;
              w.projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 2.0f;
              w.rate *= 0.667f;
              w.projectile.pierce += 8.0f;
              a.range -= 12.0f; // counteract IncreaseRange
              break;
          }
        }
      }
      towerModel.IncreaseRange(20.0f);

      var slashAttack = towerModel.GetAbilities().Find(a => a.name == "AbilityModel_FlashStepAbility")
        .GetBehavior<ActivateAttackModel>().attacks[0];

      slashAttack.weapons[0].projectile.GetDamageModel().damage += 5f;
      slashAttack.weapons[0].projectile.GetBehaviors<DamageModifierForTagModel>().Find(m => m.tag == "Moabs").damageAddative = 140f;
      slashAttack.weapons[0].projectile.GetBehaviors<DamageModifierForTagModel>().Find(m => m.tag == "Ceramic").damageAddative = 50f;
      slashAttack.weapons[0].projectile.radius -= 12f; // counteract IncreaseRange
      slashAttack.weapons[0].projectile.pierce += 100f;
      slashAttack.weapons[0].projectile.GetBehavior<SlowModel>().lifespanFrames = towerModel.tiers[1] < 0 ? 110 : 90;

      var cloneAbilityBehavior = towerModel.GetAbilities()
        .Find(a => a.name == "AbilityModel_Shadow Clone_").GetBehavior<ActivateAttackModel>();

      cloneAbilityBehavior.lifespanFrames = 20;
      //cloneAbilityBehavior.processOnActivate = false;
      cloneAbilityBehavior.attacks[0].weapons[0].rate = 0.15f;

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
      shadowTower.AddBehavior(new TowerExpireModel("TowerExpireModel_", 180f, 3, false, false));
      shadowTower.name = "ShadowClone";
      shadowTower.displayScale = 0.8f;


      cloneAbilityBehavior.attacks[0]
        .weapons[0].GetDescendant<CreateTowerModel>().tower = shadowTower;

    }
  }
}
