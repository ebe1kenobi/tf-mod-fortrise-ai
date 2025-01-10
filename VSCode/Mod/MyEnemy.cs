using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;
using System.Reflection;

namespace TFModFortRiseAIModule {
  public static class ExtEnemy {
    public static bool IsDead(this Enemy e) {      
      return (bool)Util.GetFieldValue("dead", typeof(Enemy), e, BindingFlags.Instance | BindingFlags.Public);
    }
  }
}
