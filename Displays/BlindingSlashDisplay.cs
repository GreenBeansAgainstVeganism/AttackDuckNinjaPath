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
  class BlindingSlashDisplay : ModDisplay
  {

    // Copy the Boomerang Monkey display
    public override string BaseDisplay => GetDisplay(TowerType.NinjaMonkey, 4, 0, 0);

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
      // Print info about the node in order to edit it easier
      //node.PrintInfo();
      //node.SaveMeshTexture(0);
      //node.SaveMeshTexture(1);
      //node.SaveMeshTexture(2);
      //node.SaveMeshTexture(3);

      // Set our custom texture
      SetMeshTexture(node, "ShadowCloneDisplay", 0);
      SetMeshTexture(node, "ShadowCloneDisplay1", 1);
      SetMeshTexture(node, "ShadowCloneDisplay", 2);
      //SetMeshTexture(node, "ShadowCloneDisplay", 3);
      SetMeshOutlineColor(node, new Color(147f / 255, 62f / 255, 62f / 255));

      // Make it not hold the shuriken
      node.RemoveBone("NinjaMonkeyRig:Dart");
    }
  }
}
