using System;
using System.Xml;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;


namespace TFModFortRiseAIModule {
  public class MyLevel {
    //Action originalUpdate;

		//Action originalHandlePausing;

    internal static void Load()
    {
      On.TowerFall.Level.HandlePausing += HandlePausing_patch;
      On.TowerFall.Level.Update += Update_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.Level.HandlePausing -= HandlePausing_patch;
      On.TowerFall.Level.Update -= Update_patch;
    }


    public MyLevel() { }

   // public MyLevel(Session session, XmlElement xml) : base(session, xml) {

   //   var ptr = typeof(Level).GetMethod("$original_Update").MethodHandle.GetFunctionPointer();
   //   originalUpdate = (Action)Activator.CreateInstance(typeof(Action), this, ptr);

			//ptr = typeof(Level).GetMethod("$original_HandlePausing").MethodHandle.GetFunctionPointer();
   //   originalHandlePausing = (Action)Activator.CreateInstance(typeof(Action), this, ptr);
   // }

		public static void HandlePausing_patch(On.TowerFall.Level.orig_HandlePausing orig, global::TowerFall.Level self) {
      // Avoid pausing when no human is playing and the screen goes out of focus.
			if (AiMod.ModAITraining && !AiMod.IsHumanPlaying()) {
        return;
      }

      orig(self);
    }

		public static void Update_patch(On.TowerFall.Level.orig_Update orig, global::TowerFall.Level self) {
      //if (AiMod.ModAIEnabled) {
        Agents.RefreshInputFromAgents(self);
      //}

      orig(self);

    }
  }
}
