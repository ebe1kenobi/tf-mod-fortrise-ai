using System;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseAIModule {
  public class MySession {
    //Action originalOnLevelLoadFinish;

    internal static void Load()
    {
      On.TowerFall.Session.OnLevelLoadFinish += OnLevelLoadFinish_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.Session.OnLevelLoadFinish -= OnLevelLoadFinish_patch;
    }

    public MySession() { }

    //public static void ctor_patch(MatchSettings matchSettings) {
    //  orig(self, matchSettings);

    //  //originalOnLevelLoadFinish = Util.GetAction("$original_OnLevelLoadFinish", typeof(Session), this);
    //}

    public static void OnLevelLoadFinish_patch(On.TowerFall.Session.orig_OnLevelLoadFinish orig, global::TowerFall.Session self) {
      orig(self);

      //if (AiMod.ModAIEnabled) {
        Agents.NotifyLevelLoad(self.CurrentLevel);
      //}
    }
  }
}
