using System;
using Monocle;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;


namespace TFModFortRiseAIModule {
  public static class MyMenuInput
  {
    internal static void Load()
    {
      On.TowerFall.MenuInput.cctor += cctor_patch;
      //On.TowerFall.MenuInput.Confirm += Confirm_patch;
      //On.TowerFall.MenuInput.ConfirmOrStart += ConfirmOrStart_patch;
      //On.TowerFall.MenuInput.ReplaySkip += ReplaySkip_patch;

    }

    internal static void Unload()
    {
    }

    static void cctor_patch(On.TowerFall.MenuInput.orig_cctor orig) {
      //if (!AiMod.ModAIEnabled)
      //{
      //  // Avoid overriding menu inputs created by the mod.
      //  MenuInput.MenuInputs = new PlayerInput[0];
      //}
      //MenuInput.repeatLeftCounter = new Counter(); //Util ? TODO check
      //MenuInput.repeatRightCounter = new Counter(); //Util ? TODO check
      //MenuInput.repeatUpCounter = new Counter(); //Util ? TODO check
      //MenuInput.repeatDownCounter = new Counter(); //Util ? TODO check
    }

    public static bool Confirm { //TODO
      get {
        // Makes the bot automatically confirm all menus.
        if (AiMod.ModAITraining && !AiMod.IsHumanPlaying()) return true;
        var ptr = typeof(MenuInput).GetMethod("$original_get_Confirm").MethodHandle.GetFunctionPointer();
        var orginalGetConfirm = (Func<bool>)Activator.CreateInstance(typeof(Func<bool>), null, ptr);
        return orginalGetConfirm();
      }
    }

    public static bool ConfirmOrStart //TODO
    {
      get {
        // Makes the bot automatically confirm all menus.
        if (AiMod.ModAITraining && !AiMod.IsHumanPlaying()) return true; 
        var ptr = typeof(MenuInput).GetMethod("$original_get_ConfirmOrStart").MethodHandle.GetFunctionPointer();
        var orginalGetConfirm = (Func<bool>)Activator.CreateInstance(typeof(Func<bool>), null, ptr);
        return orginalGetConfirm();
      }
    }
    public static bool ReplaySkip //TODO
    {
      get
      {
        // Makes the bot automatically confirm all menus.
        if (AiMod.ModAITraining && !AiMod.IsHumanPlaying()) return true;
        var ptr = typeof(MenuInput).GetMethod("$original_get_ReplaySkip").MethodHandle.GetFunctionPointer();
        var orginalGetReplaySkip = (Func<bool>)Activator.CreateInstance(typeof(Func<bool>), null, ptr);
        return orginalGetReplaySkip();
      }
    }
  }
}
