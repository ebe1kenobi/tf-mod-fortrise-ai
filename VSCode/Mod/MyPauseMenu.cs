using System;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;


namespace TFModFortRiseAIModule {
  public class MyPauseMenu {

  	public static DateTime creationTime;

    internal static void Load()
    {
      On.TowerFall.PauseMenu.VersusMatchSettingsAndSave += VersusMatchSettingsAndSave_patch;
      On.TowerFall.PauseMenu.Quit += Quit_patch;
      On.TowerFall.PauseMenu.VersusMatchSettings += VersusMatchSettings_patch;
      On.TowerFall.PauseMenu.VersusArcherSelect += VersusArcherSelect_patch;
      On.TowerFall.PauseMenu.QuestMap += QuestMap_patch;
      On.TowerFall.PauseMenu.VersusRematch += VersusRematch_patch;
      On.TowerFall.PauseMenu.QuestRestart += QuestRestart_patch;
      On.TowerFall.PauseMenu.QuestMapAndSave += QuestMapAndSave_patch;
      On.TowerFall.PauseMenu.QuitAndSave += QuitAndSave_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.PauseMenu.VersusMatchSettingsAndSave -= VersusMatchSettingsAndSave_patch;
      On.TowerFall.PauseMenu.Quit -= Quit_patch;
      On.TowerFall.PauseMenu.VersusMatchSettings -= VersusMatchSettings_patch;
      On.TowerFall.PauseMenu.VersusArcherSelect -= VersusArcherSelect_patch;
      On.TowerFall.PauseMenu.QuestMap -= QuestMap_patch;
      On.TowerFall.PauseMenu.VersusRematch -= VersusRematch_patch;
      On.TowerFall.PauseMenu.QuestRestart -= QuestRestart_patch;
      On.TowerFall.PauseMenu.QuestMapAndSave -= QuestMapAndSave_patch;
      On.TowerFall.PauseMenu.QuitAndSave -= QuitAndSave_patch;
    }

    public MyPauseMenu() { }


    public static void VersusMatchSettingsAndSave_patch(On.TowerFall.PauseMenu.orig_VersusMatchSettingsAndSave orig, global::TowerFall.PauseMenu self) {
      if (AiMod.ModAITraining) {
        AiMod.EndSession();
        return;
      }
      
      //Util.GetAction("$original_VersusMatchSettingsAndSave", this)();
      orig(self);
    }

    public static void Quit_patch(On.TowerFall.PauseMenu.orig_Quit orig, global::TowerFall.PauseMenu self) {
      if (AiMod.ModAITraining) {
        AiMod.EndSession();
        return;
      }

      //Util.GetAction("$original_Quit", this)();
      orig(self);
    }

    public static void VersusMatchSettings_patch(On.TowerFall.PauseMenu.orig_VersusMatchSettings orig, global::TowerFall.PauseMenu self) {
      if (AiMod.ModAITraining) {
        AiMod.EndSession();
        return;
      }

      //Util.GetAction("$original_VersusMatchSettings", this)();
      orig(self);
    }

    public static void VersusArcherSelect_patch(On.TowerFall.PauseMenu.orig_VersusArcherSelect orig, global::TowerFall.PauseMenu self) {

      if (AiMod.ModAITraining) {
        AiMod.EndSession();
        return;
      }

      //Util.GetAction("$original_VersusArcherSelect", this)();
      orig(self);
    }

    public static void QuestMap_patch(On.TowerFall.PauseMenu.orig_QuestMap orig, global::TowerFall.PauseMenu self) {
      if (AiMod.ModAITraining) {
        AiMod.EndSession();
        return;
      }

      //Util.GetAction("$original_QuestMap", this)();
      orig(self);
    }

    public static void VersusRematch_patch(On.TowerFall.PauseMenu.orig_VersusRematch orig, global::TowerFall.PauseMenu self) {
      if (AiMod.ModAITraining) {
        AiMod.Rematch();
        return;
      }

      //Util.GetAction("$original_VersusRematch", this)();
      orig(self);
    }

    public static void QuestRestart_patch(On.TowerFall.PauseMenu.orig_QuestRestart orig, global::TowerFall.PauseMenu self) {
      if (AiMod.ModAITraining) {
        AiMod.Rematch();
        return;
      }

      //Util.GetAction("$original_QuestRestart", this)();
      orig(self);
    }

    public static void QuestMapAndSave_patch(On.TowerFall.PauseMenu.orig_QuestMapAndSave orig, global::TowerFall.PauseMenu self) {
      if (AiMod.ModAITraining)
      {
        AiMod.EndSession();
        return;
      }

      //Util.GetAction("$original_QuestMapAndSave", this)();
      orig(self);
    }

    public static void QuitAndSave_patch(On.TowerFall.PauseMenu.orig_QuitAndSave orig, global::TowerFall.PauseMenu self) {
      if (AiMod.ModAITraining) {
        AiMod.EndSession();
        return;
      }

      //Util.GetAction("$original_QuitAndSave", this)();
      orig(self);
    }
  }
}
