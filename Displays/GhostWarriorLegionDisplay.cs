using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AttackDuckNinjaPath.Displays
{
  class GhostWarriorLegionDisplay : ModDisplay
  {

    // Copy the Boomerang Monkey display
    public override string BaseDisplay => GetDisplay(TowerType.NinjaMonkey, 0, 0, 5);

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
      // Print info about the node in order to edit it easier
      //node.PrintInfo();
      //node.SaveMeshTexture(0);
      //node.SaveMeshTexture(1);

      // Set our custom texture
      SetMeshTexture(node, "ShadowCloneDisplay");
      SetMeshOutlineColor(node, new Color(0f / 255, 255f / 255, 144f / 255));

      // Make it not hold the shuriken
      node.RemoveBone("Dart");
    }
  }
}
