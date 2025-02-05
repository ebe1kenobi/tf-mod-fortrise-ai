﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;
using System;
using TowerFall;

namespace TFModFortRiseAIModule
{
  public class MyXGamepadInput
  {

    internal static void Load()
    {
      On.TowerFall.XGamepadInput.GetState += GetState_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.XGamepadInput.GetState -= GetState_patch;

    }

    public MyXGamepadInput(){}

    public static InputState GetState_patch(On.TowerFall.XGamepadInput.orig_GetState orig, global::TowerFall.XGamepadInput self)
    {
      MInput.XGamepadData xgamepad = self.XGamepad;
      Vector2 vector2 = xgamepad.DPad;
      if (vector2 == Vector2.Zero)
        vector2 = xgamepad.GetLeftStick();

      return new InputState()
      {

        MoveX = (double)Math.Abs(vector2.X) < 0.5 ? 0 : Math.Sign(vector2.X),
        MoveY = (double)Math.Abs(vector2.Y) < 0.800000011920929 ? 0 : Math.Sign(vector2.Y),
        AimAxis = (double)vector2.LengthSquared() < 0.090000003576278687 ? Vector2.Zero : vector2,
        JumpCheck = xgamepad.Check(Buttons.A),
        JumpPressed = xgamepad.Pressed(Buttons.A),
        ShootCheck = xgamepad.Check(Buttons.X),
        ShootPressed = xgamepad.Pressed(Buttons.X),
        AltShootCheck = xgamepad.Check(Buttons.B),
        AltShootPressed = xgamepad.Pressed(Buttons.B),
        DodgeCheck = xgamepad.LeftTriggerCheck(0.1f) || xgamepad.RightTriggerCheck(0.1f),
        DodgePressed = xgamepad.LeftTriggerPressed(0.1f) || xgamepad.RightTriggerPressed(0.1f),
        ArrowsPressed = xgamepad.Pressed(Buttons.Y)
      };
    }
  }
}
