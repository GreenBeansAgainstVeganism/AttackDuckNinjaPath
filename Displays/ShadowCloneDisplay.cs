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
  class ShadowCloneDisplay : ModDisplay
  {

    // Copy the Boomerang Monkey display
    public override string BaseDisplay => GetDisplay(TowerType.NinjaMonkey, 0, 3, 0);

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
      // Print info about the node in order to edit it easier
      //node.PrintInfo();
      //node.SaveMeshTexture(0);
      //node.SaveMeshTexture(1);

      // Set our custom texture
      SetMeshTexture(node, Name, 0);
      SetMeshTexture(node, Name + 1, 1);
      SetMeshOutlineColor(node, new Color(100f / 255, 100f / 255, 100f / 255));

      // Make it not hold the shuriken
      node.RemoveBone("NinjaMonkeyRig:Dart");
    }
  }
}
