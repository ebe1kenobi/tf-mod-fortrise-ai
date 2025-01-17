using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;
using MonoMod.Utils;

namespace TFModFortRiseAIModule {
  public static class MyCultist
  {
    public static StateEntity GetState(this Cultist ent) {
      var aiState = new StateEntity();

      var dynData = DynamicData.For(ent);
      aiState.type = ConversionTypes.CultistTypes.GetB((TowerFall.Cultist.CultistTypes)dynData.Get("type"));
      //aiState.type = ConversionTypes.CultistTypes.GetB(ent.type);
      ExtEntity.SetAiState(ent, aiState);
      dynData.Dispose();
      return aiState;
    }
  }
}
