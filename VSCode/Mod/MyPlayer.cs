﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monocle;
using FortRise;
using TowerFall;
using MonoMod.Utils;

namespace TFModFortRiseAIModule
{
  public static class MyPlayer
  {
    public static StateEntity GetState(this Player ent)
    //public static StateEntity GetState(this Player ent)
    {
      Logger.Info("MyPlayer.GetState");
      //var ent = DynamicData.For(p);

      var aiState = new StateArcher() { type = "archer" };

      ExtEntity.SetAiState(ent, aiState);
      aiState.playerIndex = ent.PlayerIndex;
      aiState.shield = ent.HasShield;
      aiState.wing = ent.HasWings;
      aiState.state = ent.State.ToString().FirstLower();
      aiState.arrows = new List<string>();
      List<ArrowTypes> arrows = ent.Arrows.Arrows;
      for (int i = 0; i < arrows.Count; i++)
      {
        aiState.arrows.Add(arrows[i].ToString().FirstLower());
      }
      aiState.canHurt = ent.CanHurt;
      aiState.dead = ent.Dead;
      aiState.facing = (int)ent.Facing;
      aiState.onGround = ent.OnGround;
      var dynData = DynamicData.For(ent);
      aiState.onWall = dynData.Invoke<bool>("CanWallJump", Facing.Left) || dynData.Invoke<bool>("CanWallJump", Facing.Right);
      //aiState.onWall = ent.CanWallJump(Facing.Left) || ent.CanWallJump(Facing.Right);
      Vector2 aim = Calc.AngleToVector(ent.AimDirection, 1);
      aiState.aimDirection = new Vec2
      {
        x = aim.X,
        y = -aim.Y
      };
      aiState.team = AgentConfigExtension.GetTeam(ent.TeamColor);
      dynData.Dispose();

      return aiState;
    }
  }
}
