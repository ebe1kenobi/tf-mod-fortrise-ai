using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseAIModule {
  public static class MyEvilCrystal {
    public static StateEntity GetState(this EvilCrystal ent) {
      var aiState = new StateSubType { type = "evilCrystal" };
      ExtEntity.SetAiState(ent, aiState);
      //TODO test GetPrivateFieldValue 

      aiState.subType = ConversionTypes.CrystalTypes.GetB((EvilCrystal.CrystalColors)Util.GetPrivateFieldValue("crystalColor", ent));
      //aiState.subType = ConversionTypes.CrystalTypes.GetB((EvilCrystal.CrystalColors)Util.GetPublicFieldValue("crystalColor", ent));
      return aiState;
    }
  }
}
