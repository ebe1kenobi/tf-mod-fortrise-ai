using System.Threading;
using Monocle;
using System;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseAIModule {
  public class MyTFGame {
    // This allows identifying that TowerFall.exe is patched.
    public const string AiModVersion = AiMod.ModAiVersion;

    internal static void Load()
    {
      On.TowerFall.TFGame.ctor += ctor_patch;
      On.TowerFall.TFGame.Initialize += Initialize_patch;
      On.TowerFall.TFGame.Update += Update_patch;
      On.TowerFall.TFGame.Draw += Draw_patch;
      On.TowerFall.TFGame.Load += Load_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.TFGame.ctor -= ctor_patch;
      On.TowerFall.TFGame.Initialize -= Initialize_patch;
      On.TowerFall.TFGame.Update -= Update_patch;
      On.TowerFall.TFGame.Draw -= Draw_patch;
      On.TowerFall.TFGame.Load -= Load_patch;
    }

    public MyTFGame() { }

    public static void ctor_patch(On.TowerFall.TFGame.orig_ctor orig, global::TowerFall.TFGame self, bool noIntro) {

      orig(self, noIntro);
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
    }

    public static void Load_patch(On.TowerFall.TFGame.orig_Load orig) {
      orig();
      TaskHelper.Run("WAITING FOR THE AI PYTHON TO CONNECT", () =>
      {
        try
        {
          AiMod.PreGameInitialize();
          AiMod.PostGameInitialize();

          while (!AiMod.PreUpdate()) {
            Thread.Sleep(1000);
          }

        }
        catch (Exception ex)
        {
          TFGame.Log(ex, true);
          TFGame.OpenLog();
          Engine.Instance.Exit();
        }
      });
    }

    public static void Initialize_patch(On.TowerFall.TFGame.orig_Initialize orig, global::TowerFall.TFGame self) {
      orig(self);
    }

    public static void Update_patch(On.TowerFall.TFGame.orig_Update orig, global::TowerFall.TFGame self, GameTime gameTime) {

      TFModFortRiseAIModule.gameTime = gameTime;
      TFModFortRiseAIModule.Update(orig, self);

    }

    public static void Draw_patch(On.TowerFall.TFGame.orig_Draw orig, global::TowerFall.TFGame self, GameTime gameTime) {

      if (AiMod.ModAITraining &&  (!AiMod.IsMatchRunning() || AiMod.NoGraphics)) {
        Engine.Instance.GraphicsDevice.SetRenderTarget(null);
        return;
      }
      
      orig(self, gameTime);

      // I don't know what this is for
      if (AiMod.ModAITraining) {
        Draw.SpriteBatch.Begin();
        Agents.Draw(); 
        Draw.SpriteBatch.End();
      }
    }
  }
}
