using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;
using MonoMod.Utils;

namespace TFModFortRiseAIModule {
  public static class MyIcicle
  {
    public static StateEntity GetState(this Icicle ent) {
      var aiState = new StateFalling { type = Types.Icicle };
      ExtEntity.SetAiState(ent, aiState);
      var dynData = DynamicData.For(ent);
      aiState.falling = (bool)dynData.Get("falling");
      dynData.Dispose();
      return aiState;
    }
  }
}
