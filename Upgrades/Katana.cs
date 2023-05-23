using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
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
  class Katana : UpgradePlusPlus<NinjaPath>
  {
    public override int Cost => 450;
    public override int Tier => 1;
    public override string Icon => VanillaSprites.SwordChargeAA;

    public override string Description => "Gives the ninja a secondary melee attack.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
      //towerModel.ApplyDisplay<Displays.KatanaDisplay>();
      //foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
      //{
      //  damageModel.immuneBloonProperties &= ~BloonProperties.Frozen;
      //}

      //towerModel.behaviors.Append<AttackModel>(Game.instance.model.tow);
      AttackModel swordAttackModel = Game.instance.model.GetTowerFromId("Sauda").GetAttackModel().Duplicate();
      swordAttackModel.weapons[0].rate = 0.5f;
      swordAttackModel.name = "AttackModel_Attack Katana_";
      if(towerModel.tiers[0] > 0)
      {
        swordAttackModel.weapons[0].rate *= 0.7f;
      }
      if(towerModel.tiers[0] > 1)
      {
        swordAttackModel.weapons[0].projectile.pierce += 2.0f;
      }
      if(towerModel.tiers[0] > 3)
      {
        swordAttackModel.GetDescendant<DamageModel>().damage++;
        swordAttackModel.weapons[0].projectile.pierce += 4.0f;
      }
      if(towerModel.tiers[0] > 4)
      {
        swordAttackModel.GetDescendant<DamageModel>().damage += 3;
        swordAttackModel.weapons[0].rate *= 0.5f;
        swordAttackModel.weapons[0].projectile.pierce += 6.0f;
        swordAttackModel.weapons[0].projectile.GetDescendant<DamageModifierForTagModel>().damageAddative += 4;
      }
      if(towerModel.tiers[1] > 4 || towerModel.tiers[2] > 4)
      {
        swordAttackModel.GetDescendant<DamageModel>().damage += 9;
        swordAttackModel.weapons[0].projectile.pierce += 4.0f;
      }
      if(towerModel.tiers[1] > 0)
      {
        swordAttackModel.weapons[0].projectile.AddBehavior(towerModel.GetDescendant<WindModel>().Duplicate());
      }
      if(towerModel.tiers[1] > 1)
      {
        swordAttackModel.weapons[0].projectile.collisionPasses = new int[2] { -1, 0 };
        swordAttackModel.weapons[0].projectile.AddBehavior(towerModel.GetDescendant<RemoveBloonModifiersModel>().Duplicate());
      }
      towerModel.AddBehavior(swordAttackModel);

      if (IsHighestUpgrade(towerModel))
      {
        // apply a custom display, if you want
      }
    }
  }
}
