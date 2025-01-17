using System.Reflection;
using Monocle;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;
using MonoMod.Utils;

namespace TFModFortRiseAIModule {
  public static class MyBirdman
  {
    public static StateEntity GetState(this Birdman ent) {
      var aiState = new StateEntity { type = "birdman" };

      //var dynData = DynamicData.For(ent);


      if ((Counter)Util.GetFieldValue("attackCooldown", typeof(Birdman), ent, BindingFlags.Public | BindingFlags.Instance) 
          && !(bool)Util.GetFieldValue("canArrowAttack", typeof(Birdman), ent, BindingFlags.Public | BindingFlags.Instance)) {
        aiState.state = "resting";
      } else {
        switch (ent.State) {
          case 0:
            aiState.state = "idle";
            break;
          case 1:
          //case Birdman.ST_ATTACK:
            aiState.state = "attack";
            break;
        }
      }
      
      ExtEntity.SetAiState(ent, aiState);
      //dynData.Dispose();
      return aiState;
    }
  }
}
