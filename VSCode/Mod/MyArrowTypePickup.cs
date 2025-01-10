using Monocle;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;
using MonoMod.Utils;

namespace TFModFortRiseAIModule {
  public static class MyArrowTypePickup
  {
    public static StateEntity GetState(this Entity ent) {
      var item = ent as ArrowTypePickup;

      var arrowTypePickup = DynamicData.For(item);

      var state = new StateItem {
        type = Types.Item,
        itemType = "arrow" + arrowTypePickup.Get("arrowType").ToString()
        //itemType = "arrow" + item.arrowType.ToString()
      };
      ExtEntity.SetAiState(ent, state);
      return state;
    }
  }
}
