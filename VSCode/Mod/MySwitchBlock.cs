﻿using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;
using MonoMod.Utils;

namespace TFModFortRiseAIModule {
  public static class MySwitchBlock {
    public static StateEntity GetState(this SwitchBlock ent) {
      var aiState = new StateSwitchBlock { type = Types.SwitchBlock };

      ExtEntity.SetAiState(ent, aiState);
      aiState.collidable = ent.Collidable;
      var dynData = DynamicData.For(ent);
      aiState.warning = ((bool)dynData.Get("drawFlicker")) || ((float)dynData.Get("DrawWarning")) > 0;

      return aiState;
    }
  }
}