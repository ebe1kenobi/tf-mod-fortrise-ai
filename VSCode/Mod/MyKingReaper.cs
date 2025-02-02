using Monocle;
using TowerFall;


namespace TFModFortRiseAIModule {
  public static class MyKingReaper
  {
    public static StateEntity GetState(this KingReaper ent) {
      var aiState = new StateKingReaper { type = "kingReaper" };
      ExtEntity.SetAiState(ent, aiState);
      aiState.shield = (Counter)Util.GetPrivateFieldValue("shieldCounter", ent);
      return aiState;
    }
  }
}
