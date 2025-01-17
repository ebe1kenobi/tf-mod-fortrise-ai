using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using TowerFall;

namespace TFModFortRiseAIModule
{
  public class MyVersusRoundResults// : VersusRoundResults
  {
    internal static void Load()
    {
      //On.TowerFall.Entity.ctor += Render_patch;
    }

    internal static void Unload()
    {
    }
    public MyVersusRoundResults() { }

    public static void Render_patch()
    {
      //if (this.Components == null)
      //  return;

      //for (var i = 0; i < this.crowns.Length; i++)
      //{
      //  this.crowns[i].Position.X = 100; // 126 origin
      //}

      //for (var i = 0; i < this.Components.Count; i++)
      //{
      //  if (this.Components[i].GetType().ToString() != "Monocle.Text") continue;
      //  Text text = (Text)this.Components[i];

      //  if (text.text.Length == 0) continue;
      //  if (!text.text[0].ToString().Equals("P")) continue;
      //  if (text.text[1].ToString().Equals(" ")) continue; //second pass for NAI 1 AI 1 P 1
      //  int playerIndex = int.Parse(text.text[1].ToString()) - 1;
      //  if (!TFModFortRiseAIModule.CurrentPlayerIs(PlayerType.Human, playerIndex))
      //  {
      //    text.text = TFModFortRiseAIModule.GetPlayerTypePlaying(playerIndex) + " " + (playerIndex + 1);
      //    text.Position.X -= 30;
      //  }
      //  else if (text.Position.X != 30)
      //  {
      //    text.text = TFModFortRiseAIModule.GetPlayerTypePlaying(playerIndex) + " " + (playerIndex + 1);
      //    text.Position.X -= 30;
      //  }
      //}
      //base.Render();
    }
  }
}
