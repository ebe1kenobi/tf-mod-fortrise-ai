﻿using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseAIModule {
  public static class MyReaperBeam
  {
    public static StateEntity GetState(this KingReaper.ReaperBeam ent) {
      var aiState = new StateReaperBeam { type = "kingReaperBeam" };
      ExtEntity.SetAiState(ent, aiState);
      aiState.canHurt = ent.Collidable;
      Vector2 normal = (Vector2)Util.GetPublicFieldValue("normal", ent);
      aiState.dir = new Vec2 {
        x = normal.X,
        y = normal.Y
      };
      aiState.width = 8;
      return aiState;
    }
  }
}