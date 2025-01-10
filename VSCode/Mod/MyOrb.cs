using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;
using MonoMod.Utils;

namespace TFModFortRiseAIModule {
  public static class MyOrb
  {
    public static StateEntity GetState(this Orb ent) {
      var aiState = new StateFalling { type = Types.Orb };
      ExtEntity.SetAiState(ent, aiState);
      var dynData = DynamicData.For(ent);
      aiState.falling = (bool)dynData.Get("falling");
      return aiState;
    }
  }
}
