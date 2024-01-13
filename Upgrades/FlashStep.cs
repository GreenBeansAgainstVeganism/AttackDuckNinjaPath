using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
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
  class FlashStep : UpgradePlusPlus<NinjaPath>
  {
    public override int Cost => 750;
    public override int Tier => 2;
    public override string Icon => GetTextureGUID(Name + "-Icon");

    public override bool Ability => true;

    public override string Description => "Ability: Expert footwork allow the ninja to instantly zip to a nearby location.\nAlso reduces the ninja's footprint size.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
      //towerModel.footprint = new CircleFootprintModel("CircleFootprintModel_Circle Footprint", 2.0f,false,false,false);
      towerModel.radius /= 1.5f;
      //towerModel.footprint.TryCast<CircleFootprintModel>().radius = 4.0f;
      AbilityModel ability = Game.instance.model.GetTowerFromId("SuperMonkey-003").GetAbilities().Find(m => m.name == "AbilityModel_DarkshiftAbility").Duplicate();
      ability.cooldownFrames = 180;
      ability.name = "AbilityModel_FlashStepAbility";
      ability.displayName = "Flash Step";
      ability.description = "Instantly move ninja anywhere within range.";
      ability.icon = GetSpriteReference(Name + "-Icon");
      towerModel.AddBehavior(ability);
      

      if (IsHighestUpgrade(towerModel))
      {
        // apply a custom display, if you want
      }
    }
  }
}
