using FortRise;
using Microsoft.Xna.Framework;
using MonoMod.Utils;
using TowerFall;
using static TowerFall.Bat;

namespace TFModFortRiseAIModule {
  public static class MyBat
  {
    public static StateEntity GetState(this Bat ent) {
      var aiState = new StateEntity();
      var bat = DynamicData.For(ent);

      aiState.type = ConversionTypes.BatTypes.GetB((BatType)bat.Get("batType"));
      //aiState.type = ConversionTypes.BatTypes.GetB(ent.batType);
      ExtEntity.SetAiState(ent, aiState);
      return aiState;
    }
  }
}
