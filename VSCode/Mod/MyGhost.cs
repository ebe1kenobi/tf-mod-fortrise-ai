using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseAIModule {
  public static class MyGhost
  {
    public static StateEntity GetState(this Ghost ent) {
      var aiState = new StateSubType { type = "ghost" };
      ExtEntity.SetAiState(ent, aiState);
      aiState.subType = ConversionTypes.GhostTypes.GetB((Ghost.GhostTypes)Util.GetPublicFieldValue("ghostType", ent));
      return aiState;
    }
  }
}
