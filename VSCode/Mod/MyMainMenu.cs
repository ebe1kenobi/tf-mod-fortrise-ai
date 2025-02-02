using TowerFall;


namespace TFModFortRiseAIModule
{
  public class MyMainMenu
  {
    internal static void Load()
    {
      On.TowerFall.MainMenu.Update += Update_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.MainMenu.Update -= Update_patch;
    }

    public MyMainMenu() {}

    public static void Update_patch(On.TowerFall.MainMenu.orig_Update orig, global::TowerFall.MainMenu self)
    {
      if (!AiMod.PreUpdate()) {
        TFGame.GameLoaded = false;
        AiMod.AgentConnected = false;
      } else {
        TFGame.GameLoaded = true;
        AiMod.AgentConnected = true;
      }
      orig(self);
    }
  }
}
