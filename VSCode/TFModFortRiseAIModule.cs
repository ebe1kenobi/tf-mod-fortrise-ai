using System;
using System.Diagnostics;
using System.Threading;
using FortRise;
using Microsoft.Xna.Framework;
using MonoMod.ModInterop;
using TowerFall;

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
      //////////////////////////////////////
      if (TFGame.GameLoaded && !isHumanPlayerTypeSaved)
      {
        for (var i = 0; i < TFGame.Players.Length; i++)
        {
          if (TFGame.PlayerInputs[i] == null)
          {
            currentPlayerType[i] = PlayerType.None;

            continue;
          }
          nbPlayerType[i]++;
          currentPlayerType[i] = PlayerType.Human;
          savedHumanPlayerInput[i] = TFGame.PlayerInputs[i];
        }
        isHumanPlayerTypeSaved = true;
      }
      //////////////////////////////////////

      //////////////////////////////////////
      if (NAIMod.NAIModEnabled && TFGame.GameLoaded && !NAIMod.isAgentReady && isHumanPlayerTypeSaved)
      {
        NAIMod.CreateAgent(); // TODO //ACTIVATE
      }
      //////////////////////////////////////

      int fps = 0;

      if (isHumanPlayerTypeSaved)
      {
        if (AiMod.Config?.fps > 0)
        {
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

      try
      {
        // We originalUpdate() if no AI or (if ai is enable but agent not yet connected we need to call original update
        // to display the Loader, else Preupdate will give problem (I forgot which one...)
        // introduce a problem with sandboxmode whcih wait for a reset call, which we had, but resetOperation will be null and nAgents.restet never called
        // because of the second preupdate (this one or the one in MainMenu.Update
        // => Ok corrected with (!AiMod.ModAITraining && !AiMod.AgentConnected)
        if (((!AiMod.ModAITraining && !AiMod.AgentConnected) || AiMod.PreUpdate()))
        {
          if (fps > 0)
          {
            orig(self, AiMod.GetGameTime());
          }
          else
          {
            orig(self, gameTime);
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
      {
        AiMod.LogGameTime();
        AiMod.gameTimeWatch.Restart();
      }
    }
  }
}
