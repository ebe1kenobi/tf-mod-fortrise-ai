using System.Reflection;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseAIModule {
  public static class MyQuestSpawnPortal
  {
    public static StateEntity GetState(this QuestSpawnPortal ent) {
      if (!(bool)Util.GetFieldValue("appeared", typeof(QuestSpawnPortal), ent, BindingFlags.Instance | BindingFlags.Public)) {
        return null;
      }

      var aiState = new StateEntity {
        type = "portal",
      };

      ExtEntity.SetAiState(ent, aiState);
      return aiState;
    }
  }
}
