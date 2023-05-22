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
  class KatanaDisplay : ModDisplay
  {

    // Copy the Boomerang Monkey display
    public override string BaseDisplay => GetDisplay(TowerType.NinjaMonkey);

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
      // Print info about the node in order to edit it easier
      node.PrintInfo();
      node.SaveMeshTexture();

      // Set our custom texture
      //SetMeshTexture(node, Name);
      //SetMeshOutlineColor(node, new Color(73f / 255, 175f / 255, 52f / 255));

      // Make it not hold the Boomerang
      //node.RemoveBone("SuperMonkeyRig:Dart");
    }
  }
}
