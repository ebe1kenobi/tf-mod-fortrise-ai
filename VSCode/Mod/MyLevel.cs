using System;
using System.Xml;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;


namespace TFModFortRiseAIModule {
  public class MyLevel {

    internal static void Load()
    {
      On.TowerFall.Level.HandlePausing += HandlePausing_patch;
      On.TowerFall.Level.Update += Update_patch;
      On.TowerFall.Level.ctor += ctor_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.Level.HandlePausing -= HandlePausing_patch;
      On.TowerFall.Level.Update -= Update_patch;
      On.TowerFall.Level.ctor -= ctor_patch;
    }

    public static void ctor_patch(On.TowerFall.Level.orig_ctor orig, global::TowerFall.Level self, global::TowerFall.Session session, XmlElement xml)
    {
      NAIMod.SetAgentLevel(self);
      orig(self, session, xml);
    }

    public static void HandlePausing_patch(On.TowerFall.Level.orig_HandlePausing orig, global::TowerFall.Level self) {
      // Avoid pausing when no human is playing and the screen goes out of focus.
			if (AiMod.ModAITraining && !AiMod.IsHumanPlaying()) {
        return;
      }

      orig(self);
    }

		public static void Update_patch(On.TowerFall.Level.orig_Update orig, global::TowerFall.Level self) {
      if (!(self.Ending))
      {
        //if (self.Session.CurrentLevel.LivingPlayers > 0 && ((Player)self.Session.CurrentLevel.Players[0]).playTagCountDownOn) //todo maybe crash here...
        //  TowerfallModPlayTag.TowerfallModPlayTag.Update();
          //TODO PLAYTAG

        //if (AiMod.ModAIEnabled)
        //{
          Agents.RefreshInputFromAgents(self);

        //}

        //if (NAIMod.NAIMod.NAIModEnabled)
        //{
          NAIMod.AgentUpdate(self);
        //}
      }
      else
      {
      }

      //if (AiMod.ModAIEnabled) {
      //Agents.RefreshInputFromAgents(self);
      //}

      orig(self);

    }
  }
}
