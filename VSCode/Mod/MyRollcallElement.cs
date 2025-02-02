using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using TowerFall;


namespace TFModFortRiseAIModule
{
  public class MyRollcallElement
  {
    public static Dictionary<int, Text> playerName = new Dictionary<int, Text>(8);
    public static Dictionary<int, Image> upArrow = new Dictionary<int, Image>(8);
    public static Dictionary<int, Image> downArrow = new Dictionary<int, Image>(8);
    public static Dictionary<String, int> difficultyLevel = new Dictionary<string, int>();

    internal static void Load()
    {
      On.TowerFall.RollcallElement.ctor += ctor_patch;
      On.TowerFall.RollcallElement.ForceStart += ForceStart_patch;
      On.TowerFall.RollcallElement.StartVersus += StartVersus_patch;
      On.TowerFall.RollcallElement.Render += Render_patch;
      On.TowerFall.RollcallElement.NotJoinedUpdate += NotJoinedUpdate_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.RollcallElement.ctor -= ctor_patch;
      On.TowerFall.RollcallElement.ForceStart -= ForceStart_patch;
      On.TowerFall.RollcallElement.StartVersus -= StartVersus_patch;
      On.TowerFall.RollcallElement.Render -= Render_patch;
      On.TowerFall.RollcallElement.NotJoinedUpdate -= NotJoinedUpdate_patch;
    }

    public MyRollcallElement() { }

    public static void ctor_patch(On.TowerFall.RollcallElement.orig_ctor orig, global::TowerFall.RollcallElement self, int playerIndex)
    {
      orig(self, playerIndex);
      var dynData = DynamicData.For(self);

      if (TFModFortRiseAIModule.savedHumanPlayerInput[playerIndex] != null)
      {
        TFGame.PlayerInputs[playerIndex] = TFModFortRiseAIModule.savedHumanPlayerInput[playerIndex];
        dynData.Set("input", TFGame.PlayerInputs[playerIndex]);
      }

      Color color = Color.White;
      Vector2 positionText;
      if (TFGame.Players.Length > 4)
      {
        positionText = new Vector2(-10, -60);
        positionText = new Vector2(0, 0);
      }
      else
      {
        positionText = new Vector2(10, -60);
      }
      upArrow[playerIndex] = new Image(TFGame.Atlas["versus/playerIndicator"]);
      upArrow[playerIndex].FlipY = true;
      upArrow[playerIndex].Visible = true;
      upArrow[playerIndex].Color = color;
      self.Add((Component)upArrow[playerIndex]);
      upArrow[playerIndex].X = -10;
      upArrow[playerIndex].Y = -70;

      downArrow[playerIndex] = new Image(TFGame.Atlas["versus/playerIndicator"]);
      downArrow[playerIndex].Visible = true;
      self.Add((Component)downArrow[playerIndex]);
      downArrow[playerIndex].X = -10;
      downArrow[playerIndex].Y = -50;
      downArrow[playerIndex].Color = color;

      String name = "-";
      playerName[playerIndex] = new Text(TFGame.Font, name, positionText, color, Text.HorizontalAlign.Left, Text.VerticalAlign.Bottom);

      difficultyLevel["AI"] = 20;
      difficultyLevel["NAI"] = 20;
      self.Add((Component)playerName[playerIndex]);
      
      dynData.Dispose();
    }

    public static void SetPlayerName(int playerIndex) {
      var dynData = DynamicData.For(playerName[playerIndex]);
      String type = TFModFortRiseAIModule.GetPlayerTypePlaying(playerIndex);
      string name = type + (playerIndex + 1);
      dynData.Set("text", name);
      dynData.Dispose();
    }

    public static void SetAllPLayerInput() {
      for (var i = 0; i < TFGame.Players.Length; i++) {
        switch (TFModFortRiseAIModule.currentPlayerType[i]) {
          case PlayerType.Human:
            TFGame.PlayerInputs[i] = TFModFortRiseAIModule.savedHumanPlayerInput[i];
            break;
          case PlayerType.AiMod:
            TFGame.PlayerInputs[i] = AiMod.agents[i];
            break;
          case PlayerType.NAIMod:
            TFGame.PlayerInputs[i] = NAIMod.AgentInputs[i];
            break;
          case PlayerType.None:
            throw new Exception("Player Type not initialised for player " + i);
        }
      }
    }
    public static void ForceStart_patch(On.TowerFall.RollcallElement.orig_ForceStart orig, global::TowerFall.RollcallElement self)
    {
      SetAllPLayerInput();
      orig(self);
    }
    public static void StartVersus_patch(On.TowerFall.RollcallElement.orig_StartVersus orig, global::TowerFall.RollcallElement self)
    {
      SetAllPLayerInput();
      orig(self);
    }

    public static bool HumanControlExists(int playerIndex)
    {
      return TFModFortRiseAIModule.savedHumanPlayerInput[playerIndex] != null;
    }
    public static void Render_patch(On.TowerFall.RollcallElement.orig_Render orig, global::TowerFall.RollcallElement self)
    {
      var dynData = DynamicData.For(self);
      int playerIndex = (int)dynData.Get("playerIndex");
      SetPlayerName(playerIndex);

      if (((Image)dynData.Get("rightArrow")).Visible && TFModFortRiseAIModule.IsThereOtherPlayerType(playerIndex))
      {
        if (HumanControlExists(playerIndex)) {
          if (TFModFortRiseAIModule.CurrentPlayerIs(PlayerType.Human, playerIndex)) {
            upArrow[playerIndex].Visible = false;
            downArrow[playerIndex].Visible = true;
          } 
          else if (TFModFortRiseAIModule.CurrentPlayerIs(PlayerType.AiMod, playerIndex))
          {
            upArrow[playerIndex].Visible = true;
            if (NAIMod.NAIModEnabled)
              downArrow[playerIndex].Visible = true;
            else
              downArrow[playerIndex].Visible = false;  
          }
          else if (TFModFortRiseAIModule.CurrentPlayerIs(PlayerType.NAIMod, playerIndex))
          {
            upArrow[playerIndex].Visible = true;
            downArrow[playerIndex].Visible = false;
          }
        } else if (TFModFortRiseAIModule.CurrentPlayerIs(PlayerType.AiMod, playerIndex)) {
          upArrow[playerIndex].Visible = false;
          downArrow[playerIndex].Visible = true;
        } else if (TFModFortRiseAIModule.CurrentPlayerIs(PlayerType.NAIMod, playerIndex)) {
          upArrow[playerIndex].Visible = true;
          downArrow[playerIndex].Visible = false;
        }

        var arrowSine = DynamicData.For(dynData.Get("arrowSine"));
        var rightArrowWiggle = (bool)dynData.Get("rightArrowWiggle");
        var arrowWiggle = DynamicData.For(dynData.Get("arrowWiggle"));
        float arrowSineValue = (float)arrowSine.Get("Value");
        float arrowWiggleValue = (float)arrowWiggle.Get("Value");

        upArrow[playerIndex].Y = (float)(-68 + arrowSineValue * 3.0 + 6.0 * (rightArrowWiggle ? arrowWiggleValue : 0.0));
        downArrow[playerIndex].Y = (float)(-50.0 - arrowSineValue * 3.0 + 6.0 * (!rightArrowWiggle ? arrowWiggleValue : 0.0));
        arrowSine.Dispose();
        arrowWiggle.Dispose();
      }
      else
      {
        upArrow[playerIndex].Visible = false;
        downArrow[playerIndex].Visible = false;
      }

      orig(self);
      dynData.Dispose();

    }
    public static int NotJoinedUpdate_patch(On.TowerFall.RollcallElement.orig_NotJoinedUpdate orig, global::TowerFall.RollcallElement self)
    {
      var dynData = DynamicData.For(self);

      int playerIndex = (int)dynData.Get("playerIndex");
    
      var input = DynamicData.For(dynData.Get("input"));
      if (input == null)
        return 0;

      if ((int)TFModFortRiseAIModule.currentPlayerType[playerIndex] > (int)PlayerType.Human)
      {
        String type = TFModFortRiseAIModule.GetPlayerTypePlaying(playerIndex);

        if ((bool)input.Get("MenuAlt"))
          difficultyLevel[type] += 5;
        if (difficultyLevel[type] > 100) difficultyLevel[type] = 0;
        if ((bool)input.Get("MenuAlt2"))
          difficultyLevel[type] -= 5;
        if (difficultyLevel[type] < 1) difficultyLevel[type] = 100;
      }

      var MenuUp = (bool)input.Get("MenuUp");
      var MenuDown = (bool)input.Get("MenuDown");


      if (TFModFortRiseAIModule.IsThereOtherPlayerType(playerIndex)) { //at leat 2 player type
        // Move up 
        if (MenuUp
            && HumanControlExists(playerIndex)
            && (int)TFModFortRiseAIModule.currentPlayerType[playerIndex] > (int)PlayerType.Human)
        {
          TFModFortRiseAIModule.currentPlayerType[playerIndex] = (PlayerType)(int)TFModFortRiseAIModule.currentPlayerType[playerIndex] - 1;
        }
        else if (MenuUp
                && (int)TFModFortRiseAIModule.currentPlayerType[playerIndex] > (int)PlayerType.AiMod)
        {
          TFModFortRiseAIModule.currentPlayerType[playerIndex] = (PlayerType)(int)TFModFortRiseAIModule.currentPlayerType[playerIndex] - 1;
        }

        // Move down
        if (MenuDown
            && NAIMod.NAIModEnabled && (int)TFModFortRiseAIModule.currentPlayerType[playerIndex] < (int)PlayerType.NAIMod)
        {
          TFModFortRiseAIModule.currentPlayerType[playerIndex] = (PlayerType)(int)TFModFortRiseAIModule.currentPlayerType[playerIndex] + 1;
        } else if (MenuDown
                  && (int)TFModFortRiseAIModule.currentPlayerType[playerIndex] < (int)PlayerType.AiMod) {
          TFModFortRiseAIModule.currentPlayerType[playerIndex] = (PlayerType)(int)TFModFortRiseAIModule.currentPlayerType[playerIndex] + 1;
        }
      }
      dynData.Dispose();

      return orig(self);
    }
  }
}
