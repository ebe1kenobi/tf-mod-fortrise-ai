using System;
using System.Diagnostics;
using System.Threading;
using FortRise;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;
using MonoMod.ModInterop;
using MonoMod.Utils;
using TowerFall;

//NAIMod + AI 
//Level  ok reste playtag code if
//Loader  ok
//MainMenu  ok
//PauseMenu  ok
//PlayerIndicator  ok
//RollcallElement  ok
//TfGame  todo
//MyVersusRoundResults  TODO : On.TowerFall.Entity.Render doesn't exists
//modcompilkenobi  ok transferer dans TFModFortRiseAIModule => reste Aimod a supprimer
//Aimod ok laisse comme ça


namespace TFModFortRiseAIModule
{
  public enum PlayerType
  {
    None = 0,
    Human,
    AiMod,
    NAIMod,
  }

  [Fort("com.ebe1.kenobi.tfmodfortriseaimodule", "TFModFortRiseAIModule")]
  public class TFModFortRiseAIModule : FortModule
  {
    public static TFModFortRiseAIModule Instance;
    public static bool IsModPlaytagExists;
    public static bool IsModEigthPlayerExists;

    public const string BaseDirectory = "TFModFortRiseAIModule";

    public static GameTime gameTime;
    public static Stopwatch gameTimeWatch;
    private static readonly Stopwatch fpsWatch = new Stopwatch();

    //public static PlayerInput[] savedHumanPlayerInput = new PlayerInput[TFGame.Players.Length];
    //public static int[] nbPlayerType = new int[TFGame.Players.Length];
    //public static PlayerType[] currentPlayerType = new PlayerType[TFGame.Players.Length];

    public static PlayerInput[] savedHumanPlayerInput = new PlayerInput[8];
    public static int[] nbPlayerType = new int[8];
    public static PlayerType[] currentPlayerType = new PlayerType[8];

    public static bool isHumanPlayerTypeSaved = false;

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
      //MyLoader.Load();
      MyMainMenu.Load();
      MyMenuInput.Load();
      MyPauseMenu.Load();
      MySession.Load();
      MyTFGame.Load();
      MyXGamepadInput.Load();
      MyPlayerIndicator.Load();
      MyRollcallElement.Load();
      MyVersusRoundResults.Load();

      typeof(EigthPlayerImport).ModInterop();
      typeof(PlayTagImport).ModInterop();

    }

    public override void Unload()
    {
      TFModFortRiseAIModule.IsModEigthPlayerExists = IsModExists("WiderSetMod");
      MyLevel.Unload();
      //MyLoader.Unload();
      MyMainMenu.Unload();
      MyMenuInput.Unload();
      MyPauseMenu.Unload();
      MySession.Unload();
      MyTFGame.Unload();
      MyXGamepadInput.Unload();
      MyPlayerIndicator.Unload();
      MyRollcallElement.Unload();
      MyVersusRoundResults.Unload();
    }

    public override void ParseArgs(string[] args)
    {
      AiMod.ParseArgs(args);
      NAIMod.ParseArgs(args);
    }

    public static bool CurrentPlayerIs(PlayerType type, int playerIndex)
    {
      return currentPlayerType[playerIndex] == type;
    }


    public static string GetPlayerTypePlaying(int playerIndex)
    {
      switch (currentPlayerType[playerIndex])
      {
        case PlayerType.Human: return "P";
        case PlayerType.AiMod: return "AI";
        case PlayerType.NAIMod: return "NAI";
        default: throw new Exception("Current type Player not initialised :" + currentPlayerType[playerIndex]);
      }
    }

    public static bool IsAgentPlaying(int playerIndex, Level level)
    {
      return level.GetPlayer(playerIndex) != null && (currentPlayerType[playerIndex] == PlayerType.AiMod || currentPlayerType[playerIndex] == PlayerType.NAIMod);
    }
    public static bool IsThereOtherPlayerType(int playerIndex)
    {
      return nbPlayerType[playerIndex] > 1;
    }


    public static void Update(On.TowerFall.TFGame.orig_Update orig, TFGame self)
    {
      //Logger.Info("Update");
      //Logger.Info("TFGame.GameLoaded:" + TFGame.GameLoaded);
      //Logger.Info("isHumanPlayerTypeSaved:" + isHumanPlayerTypeSaved);


      //////////////////////////////////////
      if (TFGame.GameLoaded && !isHumanPlayerTypeSaved)
      {
        //Logger.Info("in TFGame.GameLoaded && !isHumanPlayerTypeSaved");
        for (var i = 0; i < TFGame.Players.Length; i++)
        //for (var i = 0; i < TFGame.PlayerInputs.Length; i++)
        {
          //Logger.Info("i=" + i);
          //if (TFGame.PlayerInputs[i] == null) continue;
          if (TFGame.PlayerInputs[i] == null)
          {
            currentPlayerType[i] = PlayerType.None;
            //Logger.Info("currentPlayerType["+ i+"]" + currentPlayerType[i]);

            continue;
          }
          nbPlayerType[i]++;
          currentPlayerType[i] = PlayerType.Human;
          savedHumanPlayerInput[i] = TFGame.PlayerInputs[i];
            //Logger.Info("currentPlayerType["+ i+"]" + currentPlayerType[i]);
        }
        isHumanPlayerTypeSaved = true;
      }
      //////////////////////////////////////


      //Logger.Info("after if");

      //if (NAIMod.NAIModEnabled)
      //{
      //Logger.Info("in AIMod.NAIModEnabled");


      //////////////////////////////////////
      if (NAIMod.NAIModEnabled && TFGame.GameLoaded && !NAIMod.isAgentReady && isHumanPlayerTypeSaved)
      {
        //Logger.Info("call CreateAgent()");
        NAIMod.CreateAgent(); // TODO //ACTIVATE
      }
      //////////////////////////////////////



      //}
      //Logger.Info("after if NAIMOD()");

      int fps = 0;

      if (isHumanPlayerTypeSaved)
      //if (AiMod.ModAIEnabled && isHumanPlayerTypeSaved)
      {
        //Logger.Info("in if AiMod.ModAIEnabled && isHumanPlayerTypeSaved");
        if (AiMod.Config?.fps > 0)
        {
          //Logger.Info("in if AiMod.Config?.fps > 0");
          fps = AiMod.IsMatchRunning() ? AiMod.Config.fps : 10;
          fpsWatch.Stop();
          long ticks = 10000000L / fps;
          if (fpsWatch.ElapsedTicks < ticks)
          {
            Thread.Sleep((int)(ticks - fpsWatch.ElapsedTicks) / 10000);
          }
          fpsWatch.Reset();
          fpsWatch.Restart();
        }

        if (!AiMod.ConnectionDispatcher.IsRunning)
        {
          throw new Exception("ConnectionDispatcher stopped running");
        }

        if (!AiMod.loggedScreenSize)
        {
          Logger.Info("Screen: {0} x {1}, {2}".Format(
            TFGame.Instance.Screen.RenderTarget.Width,
            TFGame.Instance.Screen.RenderTarget.Height,
            TFGame.Instance.Screen.RenderTarget.Format));
          AiMod.loggedScreenSize = true;
        }
      }
      //Logger.Info("before try orig");

      try
      {
        // We originalUpdate() if no AI or (if ai is enable but agent not yet connected we need to call original update
        // to display the Loader, else Preupdate will give problem (I forgot which one...)
        // introduce a problem with sandboxmode whcih wait for a reset call, which we had, but resetOperation will be null and nAgents.restet never called
        // because of the second preupdate (this one or the one in MainMenu.Update
        // => Ok corrected with (!AiMod.ModAITraining && !AiMod.AgentConnected)
        if (((!AiMod.ModAITraining && !AiMod.AgentConnected) || AiMod.PreUpdate()))
        //if (!AiMod.ModAIEnabled || ((!AiMod.ModAITraining && !AiMod.AgentConnected) || AiMod.PreUpdate()))
        {
          //Logger.Info("if (fps > 0)");
          if (fps > 0)
          //if (AiMod.ModAIEnabled && fps > 0)
          {
          Logger.Info("in if (fps > 0)");
            //Logger.Info(" if (AiMod.ModAIEnabled && fps > 0)");
            orig(self, AiMod.GetGameTime());
          }
          else
          {
            //Logger.Info("else " + gameTime.TotalGameTime);
            //Logger.Info("else " + gameTime.ElapsedGameTime);
            //Logger.Info("else " + gameTime.ToString());
            orig(self, gameTime);
            //Logger.Info("after");
          }
        }
      }
      catch (AggregateException aggregateException)
      {
        foreach (var innerException in aggregateException.Flatten().InnerExceptions)
        {
          AiMod.HandleFailure(innerException);
        }
      }
      catch (Exception ex)
      {
        AiMod.HandleFailure(ex);
      }

      if (AiMod.gameTimeWatch.ElapsedMilliseconds > AiMod.logTimeInterval.TotalMilliseconds)
      //if (AiMod.ModAIEnabled && AiMod.gameTimeWatch.ElapsedMilliseconds > AiMod.logTimeInterval.TotalMilliseconds)
      {
        AiMod.LogGameTime();
        AiMod.gameTimeWatch.Restart();
      }
    }
  }
}
