using System;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;
using Monocle;
using MonoMod.Utils;

namespace TFModFortRiseAIModule {
  public static class MyBombArrow
  {
    public static StateEntity GetState(this BombArrow ent) {
      var state = (StateArrow)ExtEntity.GetStateArrow(ent);

      var dynData = DynamicData.For(ent);
      state.timeLeft = (float)Math.Ceiling(((Alarm)dynData.Get("explodeAlarm")).FramesLeft);
      //state.timeLeft = (float)Math.Ceiling(ent.explodeAlarm.FramesLeft);
      return state;
    }
  }
}
