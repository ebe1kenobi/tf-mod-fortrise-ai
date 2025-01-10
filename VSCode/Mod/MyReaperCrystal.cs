using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseAIModule {
  public static class MyReaperCrystal
  {
    public static StateEntity GetState(this KingReaper.ReaperCrystal ent) {
      var aiState = new StateEntity { type = "kingReaperCrystal" };
      ExtEntity.SetAiState(ent, aiState);
      aiState.canHurt = true;
      return aiState;
    }
  }
}
