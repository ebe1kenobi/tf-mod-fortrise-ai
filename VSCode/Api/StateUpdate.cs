﻿using System.Collections.Generic;

namespace TFModFortRiseAIModule {
  public class StateUpdate : State {
    public StateUpdate() {
      type = "update";
    }
    
    public List<StateEntity> entities = new List<StateEntity>();

    public float dt;
    public int id;
  }
}
