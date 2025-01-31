using System;
using System.Xml;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;
using Monocle;


namespace TFModFortRiseAIModule {
  public class MyLevel {

    public static int nbUpdate = 0;

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
      if (NAIMod.NAIModEnabled) { 
        NAIMod.SetAgentLevel(self); //ACTIVATE
      }
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
      nbUpdate++;
        //Logger.Info("nbUpdate = " + nbUpdate + " MyRollcallElement.difficultyLevel[\"AI\"] = " + MyRollcallElement.difficultyLevel["AI"] + " MyRollcallElement.difficultyLevel[\"NAI\"] = " + MyRollcallElement.difficultyLevel["NAI"]);
      if (!(self.Ending))
      {
        //if (self.Session.CurrentLevel.LivingPlayers > 0 && ((Player)self.Session.CurrentLevel.Players[0]).playTagCountDownOn) //todo maybe crash here...
        //  TowerfallModPlayTag.TowerfallModPlayTag.Update();
        //TODO PLAYTAG
        //if (nbUpdate % MyRollcallElement.difficultyLevel["AI"] == 0)
        //{
          Agents.RefreshInputFromAgents(self);

        //}

        if (NAIMod.NAIModEnabled && nbUpdate % MyRollcallElement.difficultyLevel["NAI"] == 0)
        {
          NAIMod.AgentUpdate(self); //ACTIVATE
        }
        //if (AiMod.ModAIEnabled)
        //{
        //Agents.RefreshInputFromAgents(self);

        //}

        //if (NAIMod.NAIMod.NAIModEnabled)
        //{
        //NAIMod.AgentUpdate(self);
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
