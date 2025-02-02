using System;
using MonoMod.Utils;


namespace TFModFortRiseAIModule
{
  public class MyLoader
  {
    public static DateTime creationTime;

    internal static void Load()
    {
      On.TowerFall.Loader.ctor += ctor_patch;
      On.TowerFall.Loader.Render += Render_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.Loader.ctor -= ctor_patch;
      On.TowerFall.Loader.Render -= Render_patch;
    }

    public MyLoader() { }

    public static void ctor_patch(On.TowerFall.Loader.orig_ctor orig, global::TowerFall.Loader self, bool showMessage)
    {
      orig(self, showMessage);
      creationTime = DateTime.Now;
    }

    public static void Render_patch(On.TowerFall.Loader.orig_Render orig, global::TowerFall.Loader self)
    {
      var dynData = DynamicData.For(self);
      
      String Message = "WAITING FOR THE AI PYTHON TO CONNECT ...";
      for (var i = 0; i < (int)(DateTime.Now - creationTime).TotalSeconds; i++) {
        if (i % 2 == 0) continue;
        Message += ".";
      }

      dynData.Set("Message", Message);
      orig(self);
      dynData.Dispose();
    }

  }
}
