using System.Xml;


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
      if (!(self.Ending))
      {
        Agents.RefreshInputFromAgents(self);

        if (NAIMod.NAIModEnabled && nbUpdate % MyRollcallElement.difficultyLevel["NAI"] == 0)
        {
          NAIMod.AgentUpdate(self); //ACTIVATE
        }
      }

      orig(self);
    }
  }
}
