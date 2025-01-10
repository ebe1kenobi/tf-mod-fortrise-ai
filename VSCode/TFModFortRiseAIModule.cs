using System;
using System.IO;
using System.Reflection;
using System.Xml;
using FortRise;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;
using MonoMod.ModInterop;
using MonoMod.Utils;
using TowerFall;

namespace TFModFortRiseAIModule
{
  [Fort("com.ebe1.kenobi.tfmodfortriseaimodule", "TFModFortRiseAIModule")]
  public class TFModFortRiseAIModule : FortModule
  {
    public static TFModFortRiseAIModule Instance;

    public override Type SettingsType => typeof(TFModFortRiseAISettings);
    public TFModFortRiseAISettings Settings => (TFModFortRiseAISettings)Instance.InternalSettings;

    public TFModFortRiseAIModule() 
    {
        Instance = this;
        Logger.Init("TFModFortRiseAIModuleLOG");
    }

    public override void LoadContent()
    {
      
    }

    public override void Load()
    {
      MyLevel.Load();
      MyLoader.Load();
      MyMainMenu.Load();
      MyMenuInput.Load();
      MyPauseMenu.Load();
      MySession.Load();
      MyTFGame.Load();
      MyXGamepadInput.Load();
    }

    public override void Unload()
    {
      MyLevel.Unload();
      MyLoader.Unload();
      MyMainMenu.Unload();
      MyMenuInput.Unload();
      MyPauseMenu.Unload();
      MySession.Unload();
      MyTFGame.Unload();
      MyXGamepadInput.Unload();
    }
  }
}
