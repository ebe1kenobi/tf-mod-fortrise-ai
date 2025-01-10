using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;
using MonoMod.Utils;

namespace TFModFortRiseAIModule {
  public static class MyLantern
  {
    public static StateEntity GetState(this Lantern ent) {
      var aiState = new StateFalling { type = Types.Lantern };
      ExtEntity.SetAiState(ent, aiState);
      var dynData = DynamicData.For(ent);
      aiState.falling = (bool)dynData.Get("falling");
      return aiState;
    }
  }
}
