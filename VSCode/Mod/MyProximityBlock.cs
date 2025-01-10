﻿using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseAIModule {
  public static class MyProximityBlock
  {
    public static StateEntity GetState(this ProximityBlock ent) {
      var  aiState = new StateProximityBlock { type = Types.ProximityBlock };
      
      ExtEntity.SetAiState(ent, aiState);
      aiState.collidable = ent.Collidable;

      return aiState;
    }
  }
}