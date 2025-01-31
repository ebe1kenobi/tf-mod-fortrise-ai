using System;
using FortRise;
using TowerFall;
using MonoMod.ModInterop;

namespace TFModFortRiseAIModule
{
  [ModImportName("com.fortrise.TFModFortRiseGameModePlaytag")]
  public static class PlayTagImport
  {
    public static Func<Modes, bool> IsGameModePlayTag;
    public static Func<int, bool> IsPlayTagCountDownOn;
    public static Func<int, bool> IsPlayerPlayTag;
  }
}