using FortRise;
using Microsoft.Xna.Framework;
using MonoMod.Utils;
using TowerFall;
namespace TFModFortRiseAIModule {
  public static class MyTreasureChest {
    public static StateEntity GetState(this TreasureChest ent) {
      if (ent.State == TreasureChest.States.WaitingToAppear) return null;

      var aiState = new StateChest { type = Types.Chest };
      ExtEntity.SetAiState(ent, aiState);
      aiState.state = ent.State.ToString().FirstLower();

      var treasureChest = DynamicData.For(ent);
      aiState.chestType = treasureChest.Get("type").ToString().FirstLower();
      //aiState.chestType = ent.type.ToString().FirstLower();

      return aiState;
    }
  }
}
