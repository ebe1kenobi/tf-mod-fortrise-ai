using Monocle;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;


namespace TFModFortRiseAIModule {
  public static class MyKingReaper
  {
    public static StateEntity GetState(this KingReaper ent) {
      var aiState = new StateKingReaper { type = "kingReaper" };
      ExtEntity.SetAiState(ent, aiState);
      //TODO test GetPrivateFieldValue 
      aiState.shield = (Counter)Util.GetPrivateFieldValue("shieldCounter", ent);
      //aiState.shield = (Counter)Util.GetPublicFieldValue("shieldCounter", ent);
      return aiState;
    }
  }
}
