﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Monocle;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TowerFall;

namespace TFModFortRiseAIModule
{
  /// <summary>
  /// Handles serialization of the game state and the communication with the agents.
  /// </summary>
  public static class Agents {
    public static List<AgentConnection> AgentConnections = new List<AgentConnection>();
    static List<AgentConnectionRemote> remoteConnections = new List<AgentConnectionRemote>();
    static StateUpdate stateUpdate = new StateUpdate();
    static List<DrawInstruction> draws = new List<DrawInstruction>();
    static List<int> listPlayerAIIndexPlaying = new List<int>();
    static int frame;
    static bool levelLoaded;
    static bool scenarioSent;

    //TowerFall.ShockCircle
    static List<String> listEntityToAdd = new List<string>
    {
      //"TowerFall.Player",

      //"TowerFall.Lantern",
      //"TowerFall.Chain",
      //"TowerFall.Cobwebs",
      //"TowerFall.LevelTiles",
      //"TowerFall.DefaultArrow",
      //"TowerFall.DefaultHat",
      //"TowerFall.LightFade",
      //"TowerFall.PlayerCorpse",
      //"TowerFall.DeathSkull",
      //"TowerFall.CatchShine",
      //"TowerFall.TreasureChest",
      //"TowerFall.BGSkeleton",
      //"TowerFall.JumpPad",
      //"TowerFall.CrackedPlatform",
      //"TowerFall.Spikeball",
      //"TowerFall.OrbPickup",
      //"TowerFall.Orb",
      //"TowerFall.Crown",
      //"TowerFall.BGBigMushroom",
      //"TowerFall.CrackedWall",
      //"TowerFall.ArrowTypePickup",
      //"TowerFall.FloatText",
      //"TowerFall.ShieldPickup",
      //"TowerFall.BombArrow",
      //"TowerFall.BombParticle",
      //"TowerFall.Explosion",
      //"TowerFall.BombPickup",
      //"TowerFall.LaserArrow",
      //"TowerFall.MovingPlatform",
      //"TowerFall.LavaControl",
      //"TowerFall.Lava",
      //"TowerFall.WingsPickup",
      //"TowerFall.Icicle",
      //"TowerFall.SnowClump",
      //"TowerFall.Ice",
      //"TowerFall.PlayerBreath",
      //"TowerFall.DrillArrow",
      //"TowerFall.SpeedBootsPickup",
      //"TowerFall.Miasma",
      //"TowerFall.KingIntro",
      //"TowerFall.SwitchBlockControl",
      //"TowerFall.SwitchBlock",
      //"TowerFall.LevelEntity",
      //"TowerFall.ShiftBlock",
      //"TowerFall.MirrorPickup",
      //"TowerFall.BoltArrow",
      //"TowerFall.SmallShock",
      //"TowerFall.WaterDrop",
      //"TowerFall.ProximityBlock",
      //"TowerFall.MoonGlassBlock",
      //"TowerFall.HotCoals",
      //"TowerFall.RainDrops",
      //"TowerFall.LoopPlatform",
      //"TowerFall.PirateBanner",
      //"TowerFall.GhostShipWindow",
      //"TowerFall.RotatePlatform",
      //"TowerFall.Mud",
      //"TowerFall.SensorBlock",
      //"TowerFall.BrambleArrow",
      //"TowerFall.Brambles",
      //"TowerFall.CrumbleBlock",
      //"TowerFall.TriggerArrow",
      //"TowerFall.PrismArrow",
      //"TowerFall.CrumbleWall",
      //"Monocle.ParticleSystem",
      //"TowerfallAi.Mod.PlayTag",
    };

    static List<String> listEntityToIgnore = new List<string> {
      
        //still freeze with :
      "TowerFall.Lantern",
      "TowerFall.Chain",
      "TowerFall.Cobwebs",
      "TowerFall.LevelTiles",
      "TowerFall.DefaultArrow",
      "TowerFall.DefaultHat",
      "TowerFall.LightFade",
      "TowerFall.PlayerCorpse",
      "TowerFall.DeathSkull",
      "TowerFall.CatchShine",
      "TowerFall.TreasureChest",
      "TowerFall.BGSkeleton",
      "TowerFall.JumpPad",
      "TowerFall.CrackedPlatform",
      "TowerFall.Spikeball",
      "TowerFall.OrbPickup",
      "TowerFall.Orb",
      "TowerFall.Crown",
      "TowerFall.BGBigMushroom",
      "TowerFall.CrackedWall",
      "TowerFall.ArrowTypePickup",
      "TowerFall.FloatText",
      "TowerFall.ShieldPickup",
      "TowerFall.BombArrow",
      "TowerFall.BombParticle",
      "TowerFall.Explosion",
      "TowerFall.BombPickup",
      "TowerFall.LaserArrow",
      "TowerFall.MovingPlatform",
      "TowerFall.LavaControl",
      "TowerFall.Lava",
      "TowerFall.WingsPickup",
      "TowerFall.Icicle",
      "TowerFall.SnowClump",
      "TowerFall.Ice",
      "TowerFall.PlayerBreath",
      "TowerFall.DrillArrow",
      "TowerFall.SpeedBootsPickup",
      "TowerFall.Miasma",
      "TowerFall.KingIntro",
      "TowerFall.SwitchBlockControl",
      "TowerFall.SwitchBlock",
      "TowerFall.LevelEntity",
      "TowerFall.ShiftBlock",
      "TowerFall.MirrorPickup",
      "TowerFall.BoltArrow",
      "TowerFall.SmallShock",
      "TowerFall.WaterDrop",
      "TowerFall.ProximityBlock",
      "TowerFall.MoonGlassBlock",
      "TowerFall.HotCoals",
      "TowerFall.RainDrops",
      "TowerFall.LoopPlatform",
      "TowerFall.PirateBanner",
      "TowerFall.GhostShipWindow",
      "TowerFall.RotatePlatform",
      "TowerFall.Mud",
      "TowerFall.SensorBlock",
      "TowerFall.BrambleArrow",
      "TowerFall.Brambles",
      "TowerFall.CrumbleBlock",
      "TowerFall.TriggerArrow",
      "TowerFall.PrismArrow",
      "TowerFall.CrumbleWall",
      "Monocle.ParticleSystem",
      //"TowerfallAi.Mod.PlayTag",
      "TowerFall.FeatherArrow",
"TowerFall.Prism",
"TowerFall.PrismParticle",
"TowerFall.CrumbleWallChunk",
"TowerFall.PrismVanish",
"TowerFall.ShockCircle",

      ////still freeze with :
      //"TowerFall.Lantern",
      //"TowerFall.Chain",
      //"TowerFall.Cobwebs",
      //"TowerFall.LevelTiles",
      ////"TowerFall.DefaultArrow",
      //"TowerFall.DefaultHat",
      //"TowerFall.LightFade",
      //"TowerFall.PlayerCorpse",
      //"TowerFall.DeathSkull",
      //"TowerFall.CatchShine",
      ////"TowerFall.TreasureChest",
      //"TowerFall.BGSkeleton",
      ////"TowerFall.JumpPad",
      ////"TowerFall.CrackedPlatform",
      ////"TowerFall.Spikeball",
      ////"TowerFall.OrbPickup",
      ////"TowerFall.Orb",
      //"TowerFall.Crown",
      //"TowerFall.BGBigMushroom",
      ////"TowerFall.CrackedWall",
      ////"TowerFall.ArrowTypePickup",
      //"TowerFall.FloatText",
      ////"TowerFall.ShieldPickup",
      ////"TowerFall.BombArrow",
      //"TowerFall.BombParticle",
      //"TowerFall.Explosion",
      ////"TowerFall.BombPickup",
      ////"TowerFall.LaserArrow",
      ////"TowerFall.MovingPlatform",
      //"TowerFall.LavaControl",
      //"TowerFall.Lava",
      ////"TowerFall.WingsPickup",
      ////"TowerFall.Icicle",
      ////"TowerFall.SnowClump",
      //"TowerFall.Ice",
      //"TowerFall.PlayerBreath",
      ////"TowerFall.DrillArrow",
      ////"TowerFall.SpeedBootsPickup",
      //"TowerFall.Miasma",
      //"TowerFall.KingIntro",
      //"TowerFall.SwitchBlockControl",
      ////"TowerFall.SwitchBlock",
      //"TowerFall.LevelEntity",
      ////"TowerFall.ShiftBlock",
      ////"TowerFall.MirrorPickup",
      ////"TowerFall.BoltArrow",
      //"TowerFall.SmallShock",
      //"TowerFall.WaterDrop",
      ////"TowerFall.ProximityBlock",
      ////"TowerFall.MoonGlassBlock",
      //"TowerFall.HotCoals",
      //"TowerFall.RainDrops",
      ////"TowerFall.LoopPlatform",
      //"TowerFall.PirateBanner",
      //"TowerFall.GhostShipWindow",
      ////"TowerFall.RotatePlatform",
      //"TowerFall.Mud",
      ////"TowerFall.SensorBlock",
      ////"TowerFall.BrambleArrow",
      ////"TowerFall.Brambles",
      ////"TowerFall.CrumbleBlock",
      ////"TowerFall.TriggerArrow",
      ////"TowerFall.PrismArrow",
      ////"TowerFall.CrumbleWall",
      //"Monocle.ParticleSystem",
      ////"TowerfallAi.Mod.PlayTag",

      //from doc
      //amaranthBoss
      //amaranthShot
      //arrow
      //bat
      //batBomb
      //batSuperBomb
      //bird
      //birdman
      //brambles
      //cataclysmBlade
      //cataclysmBlock
      //cataclysmBullet
      //cataclysmEye
      //cataclysmMissile
      //cataclysmShieldOrb
      //chain
      //chest
      //crackedPlatform
      //crackedWall
      //crown
      //crumbleBlock
      //crumbleWall
      //cultist
      //cyclopsEye
      //cyclopsFist
      //cyclopsPlatform
      //cyclopsShot
      //dreadEye
      //dreadFlower
      //dreadTentacle
      //dummy
      //enemyAttack
      //evilCrystal
      //exploder
      //explosion
      //fakeWall
      //flamingSkull
      //floorMiasma
      //ghost
      //ghostPlatform
      //graniteBlock
      //hat
      //hotCoals
      //ice
      //icicle
      //jumpPad
      //kingReaper
      //kingReaperBeam
      //kingReaperBomb
      //kingReaperCrystal
      //lantern
      //laserArrow
      //lava
      //loopPlatform
      //miasma
      //mirrorPickup
      //mole
      //moonGlassBlock
      //movingPlatform
      //mud
      //orb
      //player
      //playerCorpse
      //portal
      //prism
      //prismArrow
      //proximityBlock
      //purpleArcherPortal
      //sensorBlock
      //shiftBlock
      //shockCircle
      //slime
      //spikeball
      //switchBlock
      //technoMage
      //technoMissile
      //tornado
      //worm

    };    
    public const string InputName = "TFModFortRiseAIModule.AgentConnectionRemote";

    public static void Init() { }

    static List<JObject> entities;
    static CancellationToken cancelAgentCommunication;
    static string scenarioMessage;

    public static bool IsReset { get; private set; }
    public static bool Ready { get; private set; }

    public static int CountRemoteConnections(List<AgentConfig> agentConfigs) {
      if (agentConfigs == null || agentConfigs.Count == 0) return 0;

      int count = 0;
      foreach(AgentConfig agentConfig in agentConfigs) {
        if (agentConfig.type == AgentConfig.Type.Remote) count++;
      }
      return count;
    }

    public static int CountHumanConnections(List<AgentConfig> agentConfigs)
    {
      if (agentConfigs == null || agentConfigs.Count == 0) return 0;

      int count = 0;
      foreach (AgentConfig agentConfig in agentConfigs)
      {
        if (agentConfig.type == AgentConfig.Type.Human) count++;
      }
      return count;
    }

    /**
    *  Return the number of bot which can join the game 
    *  (if 2 joystick are connected, only 2 bot are autorithed for players, the keyboard is not take in account)
    *  In training mode, max slot open, no human player
    */
    public static int getNumberOfSlotOpen(int nbHuman)
    {
        int numberOfSlotOpen = 0;
        numberOfSlotOpen = TFGame.Players.Length; //todo getplayer 8 or 4
        return numberOfSlotOpen;
    }

    public static int PrepareAgentConnections(List<AgentConfig> agentConfigs)
    {
      DisconnectAllAgents();
      int indexAgent;
      int nbHuman = CountHumanConnections(agentConfigs);
      int nbSlotOpen = getNumberOfSlotOpen(nbHuman);

      if (AiMod.ModAITraining)
      {
        indexAgent = nbHuman;
      }
      else
      {
        indexAgent = TFGame.Players.Length - nbSlotOpen;
       
      }
      for (int i = 0; i < agentConfigs.Count && i < TFGame.Players.Length; i++)
      {
        AgentConfig agentConfig = agentConfigs[i];

        if (agentConfig.type == AgentConfig.Type.Human)
        {
          Logger.Info("Agent {0} is human".Format(i));
          AgentConnections.Add(null);
          continue;
        }
        AgentConnection agentConnection;
        if (agentConfig.type == AgentConfig.Type.Remote)
        {
          Logger.Info("Registering remote connection slot for agent {0}.".Format(indexAgent));
          AgentConnectionRemote remoteConnection = new AgentConnectionRemote(indexAgent);
          remoteConnections.Add(remoteConnection);
          agentConnection = remoteConnection;
        }
        else
        {
          throw new Exception("Unsupported agent type {0}".Format());
        }

        AgentConnections.Add(agentConnection);
        TFModFortRiseAIModule.nbPlayerType[indexAgent]++;
        AiMod.agents[indexAgent] = agentConnection;
        if (TFModFortRiseAIModule.currentPlayerType[indexAgent] == PlayerType.NAIMod ||
            TFModFortRiseAIModule.currentPlayerType[indexAgent] == PlayerType.None)
        {
          TFModFortRiseAIModule.currentPlayerType[indexAgent] = PlayerType.AiMod;
          TFGame.PlayerInputs[i] = AiMod.agents[i];
        }
        indexAgent++;
      }

      return remoteConnections.Count;
    }

    public static void AssignRemoteConnections(List<RemoteConnection> connections, CancellationToken cancelAgentCommunication) {
      if (connections.Count != remoteConnections.Count) {
        throw new Exception($"Expected {remoteConnections.Count} connections. Got {connections.Count}");
      }
      Agents.cancelAgentCommunication = cancelAgentCommunication;
      for (int i = 0; i < connections.Count; i++) {
        remoteConnections[i].Connection = connections[i];
      }
      Logger.Info("All remote agent connections assigned");
      Ready = true;
    }

    public static void NotifyLevelLoad(Level level) {
      scenarioSent = false;
      StateScenario stateScenario = new StateScenario();

      int xSize = level.Tiles.Grid.CellsX;
      int ySize = level.Tiles.Grid.CellsY;

      stateScenario.mode = AiMod.ModAITraining ? AiMod.Config.mode : MainMenu.CurrentMatchSettings.Mode.ToString();
      stateScenario.grid = new int[xSize, ySize];

      for (int x = 0; x < xSize; x++) {
        for (int y = 0; y < ySize; y++) {
          stateScenario.grid[x, ySize - y - 1] = level.Tiles.Grid[x, y] ? 1 : 0;
        }
      }

      scenarioMessage = JsonConvert.SerializeObject(stateScenario);

      levelLoaded = true;
    }

    static void SendScenario(Level level) {
      Logger.Info("Send scenario to agents.");

      List<Task> tasks = new List<Task>();

      List<Entity> listPlayer = level.Session.CurrentLevel[GameTags.Player];
      listPlayerAIIndexPlaying.Clear();
      for (var i = 0; i < listPlayer.Count; i++)
      {
        // save player at the first frame, because when player died, the player will change in session , died player disappear
        if (TFModFortRiseAIModule.currentPlayerType[((Player)listPlayer[i]).PlayerIndex] == PlayerType.Human) continue;

        listPlayerAIIndexPlaying.Add(((Player)listPlayer[i]).PlayerIndex);
      }

      // Send all state inits.
      for (int i = 0; i < AgentConnections.Count; i++) {
        var connection = AgentConnections[i];
        if (connection == null) continue;

        if (!listPlayerAIIndexPlaying.Contains(i))
        {
          continue;
        }
        else
        {
          string initMessage = JsonConvert.SerializeObject(new StateInit { index = connection.index });
          Logger.Info("Sending stateInit to agent {0}.".Format(connection.index));
          connection.Send(initMessage, frame);
        }
        var task = Task.Run(async () =>
        {
          Message reply = await connection.ReceiveAsync(AiMod.Config.agentTimeout, cancelAgentCommunication);
          if (!reply.success)
          {
            throw new Exception("Agent didn't ack state init: {0}".Format(reply.message));
          }
        });
        tasks.Add(task);
      }

      Logger.Info("Wait for all agents to ack state init.");
      WaitAllAndClear(tasks);
      
      for (int i = 0; i < AgentConnections.Count; i++) {
        var connection = AgentConnections[i];
        if (connection == null) continue;


        if (!listPlayerAIIndexPlaying.Contains(i))
        {
          continue;
        }

        Logger.Info("Notify level load to agent {0}.".Format(connection.index));
        connection.Send(scenarioMessage, frame);
        var task = Task.Run(async () => {
          Message reply = await connection.ReceiveAsync(AiMod.Config.agentTimeout, cancelAgentCommunication);
          if (!reply.success) {
            throw new Exception("Agent didn't ack state init: {0}".Format(reply.message));
          }
        });
        tasks.Add(task);
      }

      Logger.Info("Wait for all agents to ack state scenario.");
      WaitAllAndClear(tasks);

      scenarioSent = true;
      Logger.Info("All agents received scenario.");
    }

    private static void WaitAllAndClear(List<Task> tasks) {
      Task.WhenAll(tasks).Wait();
      tasks.Clear();
    }

    /// <summary>
    /// Gets notified that the session has restarted.
    /// </summary>
    public static void SessionRestarted() {
      levelLoaded = false;
      scenarioSent = false;
      IsReset = false;
    }

    /// <summary>
    /// Recreate the entities in the initial state.
    /// </summary>
    public static void Reset(List<JObject> entities, CancellationToken cancelAgentCommunication) {
      ExtEntity.Reset();
      frame = 0;
      Agents.entities = entities;
      Agents.cancelAgentCommunication = cancelAgentCommunication;
      IsReset = true;
    }

    /// <summary>
    /// Recreate the entities and reposition the players.
    /// </summary>
    static void RecreateEntities(Level level) {
      // Only recreate entities in Sandbox mode.
      if (AiMod.Config.mode != GameModes.Sandbox) return;

      // Adding and removing entities is lazy. Make sure all adds and removes are processed before changing the entities.
      level.UpdateEntityLists();

      Logger.Info($"Recreating entities. Players: {level.Players.Count}");
      foreach (var e in level.Layers[0].Entities) {
        if (getStateFunctions.ContainsKey(e.GetType())) {
          e.RemoveSelf();
        }
      }

      level.UpdateEntityLists();

      int playerIndex = 0;
      if (entities != null) {
        foreach (var e in entities) {
          if (e.GetValue("type").Value<string>() == "archer") {
            var player = EntityCreator.CreatePlayer(e, playerIndex, level.Session.MatchSettings.GetPlayerAllegiance(playerIndex));
            level.Add(player);
            playerIndex++;
          } else {
            Entity entity = EntityCreator.CreateEntity(e);
            level.Add(entity);
          }
        }
      }
      
      if (playerIndex < AgentConnections.Count) {
        throw new Exception($"Not all players have reset instructions. Expected: {AgentConnections.Count}. Received:{playerIndex}");
      }

      level.UpdateEntityLists();
      foreach(var e in level.Layers[0].Entities) {
        Logger.Info($"Entity: {e.GetType()}");
      }
    }

    public static void RefreshInputFromAgents(Level level) {

      // Game has to be reset at least once in Sandbox mode.
      if (AiMod.Config.mode == GameModes.Sandbox && !IsReset) return;

      if (cancelAgentCommunication.IsCancellationRequested) {
        return;
      }

      if (AgentConnections == null || AgentConnections.Count == 0) return;

      if (level.Frozen) return;

      if (!levelLoaded) return;

      if (!scenarioSent) {

        SendScenario(level);
      }

      if (frame == 0) {

        RecreateEntities(level);
      }

      RefreshStateUpdate(level);
      stateUpdate.dt = Engine.TimeMult;
      stateUpdate.id = frame;

      string serializedStateUpdate = JsonConvert.SerializeObject(stateUpdate);

      draws.Clear();
      List<Task> tasks = new List<Task>();

      // Start receiving all messages
      for (int i = 0; i< AgentConnections.Count; i++) {

        AgentConnection connection = AgentConnections[i];
        if (connection == null) continue;

        //send state not playing only once for python AI at the first frame
        if (!listPlayerAIIndexPlaying.Contains(i)) { 
          continue;
        }
        else
        {
          connection.Send(serializedStateUpdate, frame);
        }
        var task = Task.Run(async () => {
          Message message = await connection.ReceiveAsync(AiMod.Config.agentTimeout, cancelAgentCommunication);
          if (message.id != stateUpdate.id) {
            throw new Exception($"Ids don't match. Expected: {stateUpdate.id}, Actual: {message.id}");
          }

          if (message.draws != null) {
            draws.AddRange(message.draws);
          }

          connection.UpdateGameInput(message.actions);
        });
        tasks.Add(task);
      }
      WaitAllAndClear(tasks);
      frame++;
    }

    static Dictionary<Type, Func<Entity, StateEntity>> getStateFunctions = new Dictionary<Type, Func<Entity, StateEntity>>() {
      { typeof(AmaranthBoss), ExtEntity.GetState}, // Investigate
      { typeof(AmaranthShot), ExtEntity.GetState}, // Investigate
      { typeof(ArrowTypePickup),  (e) => MyArrowTypePickup.GetState((ArrowTypePickup)e) },
      //{ typeof(ArrowTypePickup), null },
      { typeof(Bat),  (e) => MyBat.GetState((Bat)e) },
      //{ typeof(Bat), null },
      { typeof(Birdman),  (e) => MyBirdman.GetState((Birdman)e) },
      //{ typeof(Birdman), null },

      { typeof(BoltArrow), ExtEntity.GetStateArrow },
      { typeof(BombArrow),  (e) => MyBombArrow.GetState((BombArrow)e) },
      //{ typeof(BombArrow), null },

      { typeof(BombPickup), (e) => ExtEntity.GetStateItem(e, TypesItems.Bomb) },
      { typeof(BrambleArrow), ExtEntity.GetStateArrow },
      { typeof(Brambles), ExtEntity.GetState },
      { typeof(CataclysmBlade), ExtEntity.GetState }, // Investigate
      { typeof(CataclysmBlock), ExtEntity.GetState }, // Investigate
      { typeof(CataclysmBullet), ExtEntity.GetState }, // Investigate
      { typeof(CataclysmEye), ExtEntity.GetState }, // Investigate
      { typeof(CataclysmMissile), ExtEntity.GetState }, // Investigate
      { typeof(CataclysmShieldOrb), ExtEntity.GetState }, // Investigate
      { typeof(CrackedPlatform),  (e) => MyCrackedPlatform.GetState((CrackedPlatform)e) },
      //{ typeof(CrackedPlatform), null },
      { typeof(CrackedWall),  (e) => MyCrackedWall.GetState((CrackedWall)e) },
      //{ typeof(CrackedWall), null },

      { typeof(Crown), ExtEntity.GetState },
      { typeof(CrumbleBlock), ExtEntity.GetState }, // Investigate
      { typeof(CrumbleWall), ExtEntity.GetState }, // Investigate
      { typeof(Cultist), ExtEntity.GetState },
      { typeof(CyclopsEye), ExtEntity.GetState }, // Investigate
      { typeof(CyclopsFist), ExtEntity.GetState }, // Investigate
      { typeof(CyclopsPlatform), ExtEntity.GetState }, // Investigate
      { typeof(CyclopsShot), ExtEntity.GetState }, // Investigate
      { typeof(DefaultArrow), ExtEntity.GetStateArrow },
      { typeof(DefaultHat), (e) => ExtEntity.GetState(e, Types.Hat) },
      { typeof(DreadEye), ExtEntity.GetState }, // Investigate
      { typeof(DreadFlower), ExtEntity.GetState }, // Investigate
      { typeof(DreadTentacle), ExtEntity.GetState }, // Investigate
      { typeof(DrillArrow), ExtEntity.GetStateArrow },
      { typeof(Dummy), ExtEntity.GetState }, // Investigate
      { typeof(EnemyAttack),  (e) => MyEnemyAttack.GetState((EnemyAttack)e) },
      //{ typeof(EnemyAttack), null },
      { typeof(EvilCrystal),  (e) => MyEvilCrystal.GetState((EvilCrystal)e) },
      //{ typeof(EvilCrystal), null },

      { typeof(Exploder), ExtEntity.GetState }, // Investigate
      { typeof(Explosion), ExtEntity.GetState },
      { typeof(FakeWall), ExtEntity.GetState },
      { typeof(FeatherArrow), ExtEntity.GetStateArrow },
      { typeof(FlamingSkull), ExtEntity.GetState }, // Investigate
      { typeof(FloorMiasma),  (e) => MyFloorMiasma.GetState((FloorMiasma)e) },
      //{ typeof(FloorMiasma), null },
      { typeof(Ghost),  (e) => MyGhost.GetState((Ghost)e) }, // Investigate
      //{ typeof(Ghost), null }, // Investigate

      { typeof(GhostPlatform), ExtEntity.GetState }, // Investigate
      { typeof(GraniteBlock), ExtEntity.GetState }, // Investigate
      { typeof(HotCoals), ExtEntity.GetState }, // Investigate
      { typeof(Ice), ExtEntity.GetState },
      { typeof(Icicle),  (e) => MyIcicle.GetState((Icicle)e) },
      //{ typeof(Icicle), null },

      { typeof(JumpPad), ExtEntity.GetState }, // Investigate
      { typeof(KingReaper),  (e) => MyKingReaper.GetState((KingReaper)e) },
      //{ typeof(KingReaper), null },
      { typeof(KingReaper.ReaperBeam),  (e) => MyReaperBeam.GetState((KingReaper.ReaperBeam)e) },
      //{ typeof(KingReaper.ReaperBeam), null },
      { typeof(KingReaper.ReaperBomb),  (e) => MyReaperBomb.GetState((KingReaper.ReaperBomb)e) },
      //{ typeof(KingReaper.ReaperBomb), null },
      { typeof(KingReaper.ReaperCrystal),  (e) => MyReaperCrystal.GetState((KingReaper.ReaperCrystal)e) }, // Investigate
      //{ typeof(KingReaper.ReaperCrystal), null }, // Investigate
      { typeof(Lantern),  (e) => MyLantern.GetState((Lantern)e) },
      //{ typeof(Lantern), null },

      { typeof(LaserArrow), ExtEntity.GetStateArrow },
      { typeof(Lava),  (e) => MyLava.GetState((Lava)e) },
      //{ typeof(Lava), null },

      { typeof(LoopPlatform), ExtEntity.GetState }, // Investigate
      { typeof(Miasma),  (e) => MyMiasma.GetState((Miasma)e) },
      //{ typeof(Miasma), null },

      { typeof(MirrorPickup), (e) => ExtEntity.GetStateItem(e, TypesItems.Mirror) }, // Investigate
      { typeof(Mole), ExtEntity.GetState }, // Investigate
      { typeof(MoonGlassBlock), ExtEntity.GetState }, // Investigate
      { typeof(MovingPlatform), ExtEntity.GetState }, // Investigate
      { typeof(Orb),  (e) => MyOrb.GetState((Orb)e) },
      //{ typeof(Orb), null },
      { typeof(OrbPickup),  (e) => MyOrbPickup.GetState((OrbPickup)e) },
      //{ typeof(OrbPickup), null },
      { typeof(Player),  (e) => MyPlayer.GetState((Player)e) },
      //{ typeof(Player), null },
      { typeof(PlayerCorpse),  (e) => MyPlayerCorpse.GetState((PlayerCorpse)e) },
      //{ typeof(PlayerCorpse), null },

      { typeof(Prism), ExtEntity.GetState }, // Investigate
      { typeof(PrismArrow), ExtEntity.GetStateArrow },
      { typeof(ProximityBlock), ExtEntity.GetState }, // Investigate
      { typeof(PurpleArcherPortal), ExtEntity.GetState }, // Investigate
      { typeof(QuestSpawnPortal),  (e) => MyQuestSpawnPortal.GetState((QuestSpawnPortal)e) }, // Investigate
      //{ typeof(QuestSpawnPortal), null }, // Investigate

      { typeof(SensorBlock), ExtEntity.GetState }, // Investigate
      { typeof(ShieldPickup), (e) => ExtEntity.GetStateItem(e, TypesItems.Shield) }, // Investigate
      { typeof(ShiftBlock),  (e) => MyShiftBlock.GetState((ShiftBlock)e) }, // Investigate
      //{ typeof(ShiftBlock), null }, // Investigate

      { typeof(ShockCircle), ExtEntity.GetState }, // Investigate
      { typeof(Skeleton),  (e) => MySkeleton.GetState((Skeleton)e) },
      //{ typeof(Skeleton), null },
      { typeof(Slime),  (e) => MySlime.GetState((Slime)e) },
      //{ typeof(Slime), null },

      { typeof(SpeedBootsPickup), (e) => ExtEntity.GetStateItem(e, "speedBoots") },
      { typeof(Spikeball),  (e) => MySpikeball.GetState((Spikeball)e) },
      //{ typeof(Spikeball), null },

      { typeof(SteelHat), ExtEntity.GetState }, // Investigate
      { typeof(SuperBombArrow),  (e) => MySuperBombArrow.GetState((SuperBombArrow)e) },
      //{ typeof(SuperBombArrow), null },
      { typeof(SwitchBlock),  (e) => MySwitchBlock.GetState((SwitchBlock)e) },
      //{ typeof(SwitchBlock), null },

      { typeof(TechnoMage), ExtEntity.GetState },
      { typeof(TechnoMage.TechnoMissile), ExtEntity.GetState },
      { typeof(Tornado), ExtEntity.GetState }, // Investigate
      { typeof(ToyArrow), ExtEntity.GetStateArrow },
      { typeof(TreasureChest),  (e) => MyTreasureChest.GetState((TreasureChest)e) },
      //{ typeof(TreasureChest), null },

      { typeof(TriggerArrow), ExtEntity.GetStateArrow },
      { typeof(WingsPickup), (e) => ExtEntity.GetStateItem(e, TypesItems.Wings) },
      { typeof(WoodenHat), ExtEntity.GetState }, // Investigate
      { typeof(Worm), ExtEntity.GetState }, // Investigate
    };

    private static void RefreshStateUpdate(Level level) {

      if (AgentConnections == null || AgentConnections.Count == 0) return;

      stateUpdate.entities.Clear();

      foreach (var ent in level.Layers[0].Entities)
      {
        Type type = ent.GetType();
        if (level.Session.MatchSettings.Mode != Modes.Quest && level.Session.MatchSettings.Mode != Modes.DarkWorld) { 
          if (type.ToString() != "TowerFall.Player" && !listEntityToIgnore.Contains(type.ToString()))
          {
            //get entity to ignore on log
            listEntityToIgnore.Add(type.ToString());
            // cd "/c/Program Files (x86)/Steam/steamapps/common/TowerFall/modcompilkenobi/logs"; grep "1 ==" *
          }
          if (listEntityToIgnore.Contains(type.ToString())) continue; //ignore all except player, TODO modify
        }

        Func<Entity, StateEntity> getState;
        if (!getStateFunctions.TryGetValue(type, out getState)) continue;

        StateEntity state;
        if (getState != null) {
          state = getState(ent);
        } else {
          MethodInfo methodInfo = type.GetMethod("GetState");
          if (methodInfo == null) throw new Exception("Type {0} does not have GetState".Format(type));
          var getStateFromEntityFuncPtr = methodInfo.MethodHandle.GetFunctionPointer();
          Func<StateEntity> getStateFromEntity = (Func<StateEntity>)Activator.CreateInstance(typeof(Func<StateEntity>), ent, getStateFromEntityFuncPtr);
          state = getStateFromEntity();
        }

        if (state != null) {
          stateUpdate.entities.Add(state);
        }
      }
    }

    public static void Draw() {
      foreach (var draw in draws) {
        draw.Draw();
      }
    }

    public static void DisconnectAllAgents() {

    if (AgentConnections != null) {
        foreach (var connection in AgentConnections) {
          if (connection == null) continue;
          connection.Dispose();
        }
        AgentConnections.Clear();
      }

      if (remoteConnections != null) {
        remoteConnections.Clear();
      }

      Ready = false;
    }
  }
}
