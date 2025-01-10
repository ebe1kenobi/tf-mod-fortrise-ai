﻿using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseAIModule {
  public static class MySlime
  {
    public static StateEntity GetState(this Slime ent) {
      var aiState = new StateSubType { type = "slime" };
      ExtEntity.SetAiState(ent, aiState);
      aiState.subType = ConversionTypes.SlimeTypes.GetB((Slime.SlimeColors)Util.GetPublicFieldValue("slimeColor", ent));
      return aiState;
    }
  }
}
