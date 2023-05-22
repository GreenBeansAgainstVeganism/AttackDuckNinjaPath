using MelonLoader;
using BTD_Mod_Helper;
using AttackDuckNinjaPath;
using PathsPlusPlus;
using Il2CppAssets.Scripts.Models.Towers;

[assembly: MelonInfo(typeof(AttackDuckNinjaPath.AttackDuckNinjaPath), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace AttackDuckNinjaPath
{
  public class AttackDuckNinjaPath : BloonsTD6Mod
  {
    public override void OnApplicationStart()
    {
      ModHelper.Msg<AttackDuckNinjaPath>("AttackDuckNinjaPath loaded!");
    }
  }

  public class NinjaPath : PathPlusPlus
  {
    public override string Tower => TowerType.NinjaMonkey;

    public override int UpgradeCount => 5; // Increase this up to 5 as you create your Upgrades
  }
}
