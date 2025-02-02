using Microsoft.Xna.Framework;
using MonoMod.Utils;

namespace TFModFortRiseAIModule
{
  public class MyPlayerIndicator
  {
    internal static void Load()
    {
      On.TowerFall.PlayerIndicator.ctor += ctor_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.PlayerIndicator.ctor -= ctor_patch;
    }

    public MyPlayerIndicator() { }

    public static void ctor_patch(On.TowerFall.PlayerIndicator.orig_ctor orig, global::TowerFall.PlayerIndicator self, Vector2 offset, int playerIndex, bool crown)
    {
      orig(self, offset, playerIndex, crown);
      var dynData = DynamicData.For(self);
      if (!TFModFortRiseAIModule.CurrentPlayerIs(PlayerType.Human, playerIndex))
      {
        dynData.Set("text", TFModFortRiseAIModule.GetPlayerTypePlaying(playerIndex) + " " + (playerIndex + 1).ToString());
      }
      dynData.Dispose();
    }
  }
}
