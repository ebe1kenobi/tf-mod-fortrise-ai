using System;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseAIModule {
  public class MyTFGame {
    // This allows identifying that TowerFall.exe is patched.
    public const string AiModVersion = AiMod.ModAiVersion;

    //Action originalInitialize;
    //Action<GameTime> originalUpdate;

    internal static void Load()
    {
      On.TowerFall.TFGame.ctor += ctor_patch;
      On.TowerFall.TFGame.Initialize += Initialize_patch;
      On.TowerFall.TFGame.Update += Update_patch;
      On.TowerFall.TFGame.Draw += Draw_patch;
      //On.TowerFall.TFGame.Main += Main_ctor;
    }

    internal static void Unload()
    {
      On.TowerFall.TFGame.ctor -= ctor_patch;
      On.TowerFall.TFGame.Initialize -= Initialize_patch;
      On.TowerFall.TFGame.Update -= Update_patch;
      On.TowerFall.TFGame.Draw -= Draw_patch;
      //On.TowerFall.TFGame.Main -= Main_ctor;

    }

    //[STAThread]
    //public static void Main_ctor(On.TowerFall.TFGame.orig_Main orig, string[] args)
    //{
    //  Logger.Info("Main_ctor");

    //  //try
    //  //{
    //    AiMod.ParseArgs(args);
    //    NAIMod.ParseArgs(args);
    //    orig(args);
    //  //}
    //  //catch (Exception exception)
    //  //{
    //  //  TFGame.Log(exception, false);
    //  //  TFGame.OpenLog();
    //  //}
    //}
    public MyTFGame() { }

    public static void ctor_patch(On.TowerFall.TFGame.orig_ctor orig, global::TowerFall.TFGame self, bool noIntro) {

      //AiMod.ModAIEnabled = true;
      //AiMod.ModAITraining = false;
      //NAIMod.NAIModEnabled = true;
      //AiMod.ModAIEnabled = true;
      orig(self, noIntro);
      //var ptr = typeof(TFGame).GetMethod("$original_Initialize").MethodHandle.GetFunctionPointer();
      //originalInitialize = (Action)Activator.CreateInstance(typeof(Action), this, ptr);

      //ptr = typeof(TFGame).GetMethod("$original_Update").MethodHandle.GetFunctionPointer();
      //originalUpdate = (Action<GameTime>)Activator.CreateInstance(typeof(Action<GameTime>), this, ptr);

      //if (AiMod.ModAIEnabled)
      //{
        self.InactiveSleepTime = new TimeSpan(0);

        if (AiMod.IsFastrun)
        {
          Monocle.Engine.Instance.Graphics.SynchronizeWithVerticalRetrace = false;
          self.IsFixedTimeStep = false;
        }
        else
        {
          self.IsFixedTimeStep = true;
        }
      //}
    }

    public static void Initialize_patch(On.TowerFall.TFGame.orig_Initialize orig, global::TowerFall.TFGame self) {
      //if (!AiMod.ModAIEnabled) {
      //  originalInitialize();
      //  return;
      //}
      //Logger.Info($"TowerfallAiMod version: {AiMod.ModAiVersion} Enabled: {AiMod.ModAIEnabled} Training: {AiMod.ModAITraining}");

      AiMod.PreGameInitialize();
      orig(self);
      AiMod.PostGameInitialize();
    }

    public static void Update_patch(On.TowerFall.TFGame.orig_Update orig, global::TowerFall.TFGame self, GameTime gameTime) {

      //if (!AiMod.ModAIEnabled) {
      //  originalUpdate(gameTime);
      //  return;
      //}
      TFModFortRiseAIModule.gameTime = gameTime;
      TFModFortRiseAIModule.Update(orig, self);

    }

    public static void Draw_patch(On.TowerFall.TFGame.orig_Draw orig, global::TowerFall.TFGame self, GameTime gameTime) {
      //if (!AiMod.ModAIEnabled) {
      //  base.Draw(gameTime);
      //  return;
      //}

      if (AiMod.ModAITraining &&  (!AiMod.IsMatchRunning() || AiMod.NoGraphics)) {
        Monocle.Engine.Instance.GraphicsDevice.SetRenderTarget(null);
        return;
      }
      
      orig(self, gameTime);

      // I don't know what this is for
      if (AiMod.ModAITraining) {
        Monocle.Draw.SpriteBatch.Begin();
        Agents.Draw(); 
        Monocle.Draw.SpriteBatch.End();
      }
    }
  }
}
