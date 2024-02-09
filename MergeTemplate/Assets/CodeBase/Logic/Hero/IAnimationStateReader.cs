using UnityEngine;

namespace CodeBase.Logic.Hero
{
  public interface IAnimationStateReader
  {
    void EnteredState(AnimationState state);
    void ExitedState(AnimatorState state);
  }
}