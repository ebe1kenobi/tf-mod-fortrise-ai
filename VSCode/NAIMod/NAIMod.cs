using Microsoft.Xna.Framework;
using System;
using TowerFall;

namespace TFModFortRiseAIModule
{
  public static class NAIMod
  {

    public const string ModNativeAiModVersion = "v0.1";
    public const string InputName = "TFModFortRiseAIModule.Input";
    public const string TowerfallKeyboardInputName = "TowerFall.KeyboardInput";
    public static GameTime gameTime;
    public static bool isAgentReady = false;
    //private static Agent[] agents = new Agent[TFGame.Players.Length];
    private static Agent[] agents = new Agent[8];
    //public static PlayerInput[] AgentInputs = new PlayerInput[TFGame.Players.Length];
    public static PlayerInput[] AgentInputs = new PlayerInput[8];

    public static bool NAIModEnabled { get;  set;}
    //public static bool NAIModNoKeyboardEnabled { get; private set;}
    
    public static void ParseArgs(string[] args)
    {
      NAIModEnabled = false; //ACTIVATE
    }

    public static void Update(Action<GameTime> originalUpdate)
    {
      if (NAIModEnabled &&  TFGame.GameLoaded && !isAgentReady) {
        CreateAgent();
      }
      try
      {
        originalUpdate(gameTime);
      }
      catch (AggregateException aggregateException)
      {
        foreach (var innerException in aggregateException.Flatten().InnerExceptions)
        {
          HandleFailure(innerException);
        }
      }
    }

    public static void CreateAgent()
    {
      //detect first player slot free
      for (int i = 0; i < TFGame.Players.Length; i++) //todo use everywhere
      {
        // create an agent for each player
        AgentInputs[i] = new Input(i);
        agents[i] = new Agent(i, AgentInputs[i]);
        TFModFortRiseAIModule.nbPlayerType[i]++;
        Logger.Info("Agent " + i + " Created");
        if (null != TFGame.PlayerInputs[i]) continue;

        TFGame.PlayerInputs[i] = AgentInputs[i];
        TFModFortRiseAIModule.currentPlayerType[i] = PlayerType.NAIMod;
      }

      isAgentReady = true;
    }

    public static void SetAgentLevel(Level level)
    {
      for (var i = 0; i < TFGame.Players.Length; i++)
      {
        if (!TFGame.Players[i]) continue;
        if (null == TFGame.PlayerInputs[i]) continue;
        if (! InputName.Equals(TFGame.PlayerInputs[i].GetType().ToString())) continue;
        //set level reference once, at Level creation
        agents[i].SetLevel(level);
      }
    }

    public static void AgentUpdate(Level level) {
      if (level.LivingPlayers == 0) return;

      for (int i = 0; i < TFGame.Players.Length; i++)
      {
        if (!(TFModFortRiseAIModule.CurrentPlayerIs(PlayerType.NAIMod, i)
            && TFModFortRiseAIModule.IsAgentPlaying(i, level)))
          continue;
        agents[i].Play();
      }
    }

    public static void HandleFailure(Exception ex)
    {
      Logger.Info($"Unhandled exception.\n  {ex}");
      throw ex;
    }
  }
}
