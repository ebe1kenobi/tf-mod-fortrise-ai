using System;

namespace TFModFortRiseAIModule {
  public class OperationException : Exception {
    public OperationException(string message) : base(message) { }
  }
}
