﻿using System.Threading.Tasks;

namespace TFModFortRiseAIModule {
  public static class TaskExtensions {
    public static bool IsAlive(this Task task) {
      return
        task.Status != TaskStatus.RanToCompletion &&
        task.Status != TaskStatus.Faulted &&
        task.Status != TaskStatus.Canceled;
    }
  }
}
