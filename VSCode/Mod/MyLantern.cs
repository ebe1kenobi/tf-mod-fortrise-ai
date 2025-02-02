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
      dynData.Dispose();
      return aiState;
    }
  }
}
