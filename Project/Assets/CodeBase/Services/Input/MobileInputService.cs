using UnityEngine;

namespace CodeBase.Services.Input
{
  public class MobileInputService : InputService
  {
    public override Vector3 Axis => SimpleInputAxis();
  }
}