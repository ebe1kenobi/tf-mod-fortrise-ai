using Monocle;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseAIModule {
  public static class MyMiasma
  {
    public static StateEntity GetState(this Miasma ent) {
      var aiState = new StateMiasma { type = Types.Miasma };
      ExtEntity.SetAiState(ent, aiState);
      aiState.left = ((ColliderList)ent.Collider).colliders[0].Right;
      aiState.right = ((ColliderList)ent.Collider).colliders[1].Left;
      return aiState;
    }
  }
}
