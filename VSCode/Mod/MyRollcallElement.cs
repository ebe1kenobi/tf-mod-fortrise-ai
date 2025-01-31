using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Utils;
using MonoMod;
using FortRise;
using System;
using System.Collections.Generic;
using TowerFall;


namespace TFModFortRiseAIModule
{
  public class MyRollcallElement //: RollcallElement
  {
    //public static Text playerName;
    //public static Image upArrow; 
    //public static Image downArrow;

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
      //Logger.Info("ctor_patch roolcall");
      orig(self, playerIndex);
      var dynData = DynamicData.For(self);
      //int playerIndex = (int)dynData.Get("playerIndex");
      //if (AiMod.ModAIEnabled || NAIMod.NAIMod.NAIModEnabled)
      //{
      // set the player with gamepad connected to human control even if AI player is the current player => the human can utilise the 
      // gamepad/keyboard to change the archer options
      //Logger.Info("TFModFortRiseAIModule.savedHumanPlayerInput.length " + TFModFortRiseAIModule.savedHumanPlayerInput.Length);
      //Logger.Info("TFModFortRiseAIModule.savedHumanPlayerInput["+playerIndex+"] " + TFModFortRiseAIModule.savedHumanPlayerInput[playerIndex].GetType());

        if (TFModFortRiseAIModule.savedHumanPlayerInput[playerIndex] != null)
        {
          //Logger.Info("TFModFortRiseAIModule.savedHumanPlayerInput[playerIndex] != null");
          TFGame.PlayerInputs[playerIndex] = TFModFortRiseAIModule.savedHumanPlayerInput[playerIndex];
          //Logger.Info("dynData.Set(\"input\", TFGame.PlayerInputs[playerIndex])");
          dynData.Set("input", TFGame.PlayerInputs[playerIndex]);
        }
      //}

      //Logger.Info(" Color color = ((StateMachine)dynData.Get(\"state\")).State = ");
      //Logger.Info(" Color color = ((StateMachine)dynData.Get(\"state\")).State = " + ((StateMachine)dynData.Get("state")).State);
      //Color color = ((((StateMachine)dynData.Get("state")).State == 1) ? ArcherData.Archers[self.CharacterIndex].ColorB : ArcherData.Archers[self.CharacterIndex].ColorA);
      Color color = Color.White;
      Vector2 positionText;
      //if (TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
      if (TFGame.Players.Length > 4)
      {
        positionText = new Vector2(-10, -60);
        positionText = new Vector2(0, 0);
      }
      else
      {
        positionText = new Vector2(10, -60);
      }
      //positionText = new Vector2(0, 0);

      //Logger.Info("before upArrow");
      upArrow[playerIndex] = new Image(TFGame.Atlas["versus/playerIndicator"]);
      upArrow[playerIndex].FlipY = true;
      upArrow[playerIndex].Visible = true;
      upArrow[playerIndex].Color = color;
      //!!
      self.Add((Component)upArrow[playerIndex]);
      upArrow[playerIndex].X = -10;
      upArrow[playerIndex].Y = -70;

      //Logger.Info("before downArrow");
      downArrow[playerIndex] = new Image(TFGame.Atlas["versus/playerIndicator"]);
      downArrow[playerIndex].Visible = true;
      //!!
      self.Add((Component)downArrow[playerIndex]);
      downArrow[playerIndex].X = -10;
      downArrow[playerIndex].Y = -50;
      downArrow[playerIndex].Color = color;

      String name = "-";
      //Logger.Info("before playerName[playerIndex] = new ...");
      playerName[playerIndex] = new Text(TFGame.Font, name, positionText, color, Text.HorizontalAlign.Left, Text.VerticalAlign.Bottom);
      //Logger.Info("before self.Add((Component)playerName[playerIndex]");

      difficultyLevel["AI"] = 20;
      difficultyLevel["NAI"] = 20;
      //!!
      self.Add((Component)playerName[playerIndex]);
      
      dynData.Dispose();
    }

    public static void SetPlayerName(int playerIndex) {
      var dynData = DynamicData.For(playerName[playerIndex]);
      String type = TFModFortRiseAIModule.GetPlayerTypePlaying(playerIndex);
      string name = type + (playerIndex + 1);
      //string name = type + (playerIndex + 1) + (type != "P" ? "\n\nLVL " + difficultyLevel[type] : "");
      //Logger.Info(name);
      //Logger.Info((string)dynData.Get("text"));
      dynData.Set("text", name);
      //Logger.Info((string)dynData.Get("text"));
      //((Text)MyRollcallElement.playerName[playerIndex]).text = TFModFortRiseAIModule.GetPlayerTypePlaying(playerIndex) + (playerIndex + 1);
      dynData.Dispose();
    }

    //public static Vector2 GetPosition(int playerIndex)
    //{
    //  //if (!TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
    //  if (TFGame.Players.Length == 4)
    //  {
    //    return new Vector2(52 + 72 * playerIndex, 100f);
    //  }

    //  return new Vector2((float)(0x12 + (0x29 * playerIndex)), 100f);
    //}

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
      //Logger.Info("Render_patch");
      var dynData = DynamicData.For(self);
      //Logger.Info("(int)dynData.Get(\"playerIndex\")");
      int playerIndex = (int)dynData.Get("playerIndex");
      SetPlayerName(playerIndex);

      //Logger.Info("(Image)dynData.Get(\"rightArrow\")");

      if (((Image)dynData.Get("rightArrow")).Visible && TFModFortRiseAIModule.IsThereOtherPlayerType(playerIndex))
      {
        //Logger.Info("ok");

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
        //Logger.Info("(bool)dynData.Get(\"rightArrowWiggle\")");
        var rightArrowWiggle = (bool)dynData.Get("rightArrowWiggle");
        var arrowWiggle = DynamicData.For(dynData.Get("arrowWiggle"));
        float arrowSineValue = (float)arrowSine.Get("Value");
        //Logger.Info("$arrowSineValue");
        float arrowWiggleValue = (float)arrowWiggle.Get("Value");
        //Logger.Info("$arrowWiggleValue");
        //Logger.Info(((float)arrowSine.Get("Value")).ToString());
        //Logger.Info(((float)arrowWiggle.Get("Value")).ToString());

        upArrow[playerIndex].Y = (float)(-68 + arrowSineValue * 3.0 + 6.0 * (rightArrowWiggle ? arrowWiggleValue : 0.0));
        //upArrow[playerIndex].Y = (float)(-68 + (double)arrowSine.Get("Value") * 3.0 + 6.0 * (rightArrowWiggle ? (double)arrowWiggle.Get("Value") : 0.0));
        //Logger.Info("after1");
        downArrow[playerIndex].Y = (float)(-50.0 - arrowSineValue * 3.0 + 6.0 * (!rightArrowWiggle ? arrowWiggleValue : 0.0));
        //downArrow[playerIndex].Y = (float)(-50.0 - (double)arrowSine.Get("Value") * 3.0 + 6.0 * (!rightArrowWiggle ? (double)arrowWiggle.Get("Value") : 0.0));
        //Logger.Info("after2");

        //upArrow[playerIndex].Y = (float)(-68 + (double)dynData.Get("arrowSine").Value * 3.0 + 6.0 * (dynData.Get("rightArrowWiggle") ? (double)dynData.Get("arrowWiggle").Value : 0.0));
        //downArrow[playerIndex].Y = (float)(-50.0 - (double)dynData.Get("arrowSine").Value * 3.0 + 6.0 * (!dynData.Get("rightArrowWiggle") ? (double)dynData.Get("arrowWiggle").Value : 0.0));
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
      //Logger.Info("NotJoinedUpdate_patch");
      var dynData = DynamicData.For(self);

      //Logger.Info("TFModFortRiseAIModule.currentPlayerType["+ (int)dynData.Get("playerIndex")+"]" + TFModFortRiseAIModule.currentPlayerType[(int)dynData.Get("playerIndex")]);
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
            //&& (int)TFModFortRiseAIModule.currentPlayerType[playerIndex] < (int)PlayerType.NAIMod)
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

      //if (!TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
      //  return self.NotJoinedUpdate();

      //if (this.input.MenuBack && !self.MainMenu.Transitioning)
      //{
      //  for (int i = 0; i < 8; i++)
      //  {
      //    TFGame.Players[i] = false;
      //  }
      //  Sounds.ui_clickBack.Play(160f, 1f);
      //  if ((MainMenu.RollcallMode == MainMenu.RollcallModes.Versus) || (MainMenu.RollcallMode == MainMenu.RollcallModes.Trials))
      //  {
      //    self.MainMenu.State = MainMenu.MenuState.Main;
      //  }
      //  else
      //  {
      //    self.MainMenu.State = MainMenu.MenuState.CoOp;
      //  }
      //}
      //else if (this.input.MenuLeft && self.CanChangeSelection)
      //{
      //  self.drawDarkWorldLock = false;
      //  self.ChangeSelectionLeft();
      //  Sounds.ui_move2.Play(160f, 1f);
      //  self.arrowWiggle.Start();
      //  self.rightArrowWiggle = false;
      //}
      //else if (this.input.MenuRight && this.CanChangeSelection)
      //{
      //  this.drawDarkWorldLock = false;
      //  this.ChangeSelectionRight();
      //  Sounds.ui_move2.Play(160f, 1f);
      //  this.arrowWiggle.Start();
      //  this.rightArrowWiggle = true;
      //}
      //else if (this.input.MenuAlt && GameData.DarkWorldDLC)
      //{
      //  this.drawDarkWorldLock = false;
      //  this.altWiggle.Start();
      //  Sounds.ui_altCostumeShift.Play(base.X, 1f);
      //  if (this.archerType == ArcherData.ArcherTypes.Normal)
      //  {
      //    this.archerType = ArcherData.ArcherTypes.Alt;
      //  }
      //  else
      //  {
      //    this.archerType = ArcherData.ArcherTypes.Normal;
      //  }
      //  this.portrait.SetCharacter(this.CharacterIndex, this.archerType, 1);
      //}
      //else if ((this.input.MenuConfirmOrStart && !TFGame.CharacterTaken(this.CharacterIndex)) && (TFGame.PlayerAmount < this.MaxPlayers))
      //{
      //  if (ArcherData.Get(this.CharacterIndex, this.archerType).RequiresDarkWorldDLC && !GameData.DarkWorldDLC)
      //  {
      //    this.drawDarkWorldLock = true;
      //    if ((this.darkWorldLockEase < 1f) || !TFGame.OpenStoreDarkWorldDLC())
      //    {
      //      this.portrait.Shake();
      //      this.shakeTimer = 30f;
      //      Sounds.ui_invalid.Play(base.X, 1f);
      //      if (TFGame.PlayerInputs[this.playerIndex] != null)
      //      {
      //        TFGame.PlayerInputs[this.playerIndex].Rumble(1f, 20);
      //      }
      //    }
      //    return 0;
      //  }
      //  if ((this.input.MenuAlt2Check && (this.archerType == ArcherData.ArcherTypes.Normal)) && (ArcherData.SecretArchers[this.CharacterIndex] != null))
      //  {
      //    this.archerType = ArcherData.ArcherTypes.Secret;
      //    this.portrait.SetCharacter(this.CharacterIndex, this.archerType, 1);
      //  }
      //  this.portrait.Join(false);
      //  TFGame.Players[this.playerIndex] = true;
      //  TFGame.AltSelect[this.playerIndex] = this.archerType;
      //  if (TFGame.PlayerInputs[this.playerIndex] != null)
      //  {
      //    TFGame.PlayerInputs[this.playerIndex].Rumble(1f, 20);
      //  }
      //  this.shakeTimer = 20f;
      //  if (TFGame.PlayerAmount == this.MaxPlayers)
      //  {
      //    this.ForceStart();
      //  }
      //  return 1;
      //}
      //return 0;
    }

    //public int MaxPlayers
    //{
    //  get
    //  {
    //    switch (MainMenu.RollcallMode)
    //    {
    //      case MainMenu.RollcallModes.Quest:
    //      case MainMenu.RollcallModes.DarkWorld:
    //      case MainMenu.RollcallModes.Trials:
    //          return base.MaxPlayers;
    //    }
    //    return (TF8PlayerMod.TF8PlayerMod.Mod8PEnabled) ? 8 : base.MaxPlayers;
    //  }
    //}
  }
}
