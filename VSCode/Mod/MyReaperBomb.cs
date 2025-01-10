﻿using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseAIModule {
  public static class MyReaperBomb
  {
    public static StateEntity GetState(this KingReaper.ReaperBomb ent) {
      var aiState = new StateEntity { type = "kingReaperBomb" };
      ExtEntity.SetAiState(ent, aiState);
      aiState.canHurt = true;
      return aiState;
    }
  }
}