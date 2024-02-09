using System;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.Hero
{
    public class HeroAnimator : MonoBehaviour, IAnimationStateReader
    {
        //просто аниматор игрока в отдельном классе, все как на курсе
        [SerializeField] public Animator _animator;

        [Inject] private IInputService _inputService;
        private readonly int _moveHash = Animator.StringToHash("Move");
        private readonly int _idleStateHash = Animator.StringToHash("Idle");

        private readonly int _heroAttackHash = Animator.StringToHash("Attack");
        private readonly int _stopActionStateHash = Animator.StringToHash("StopAction");
        private readonly int _carryActionStateHash = Animator.StringToHash("Carry");
        private int _carryLayerIndex;

        public CharacterController characterController;
        public event Action<AnimationState> StateEntered;
        public event Action<AnimatorState> StateExited;

        private void Start()
        {
            _carryLayerIndex = _animator.GetLayerIndex("Carry");
        }

        private void Update()
        {
            _animator.SetFloat(_moveHash, _inputService.Axis.magnitude, 0.1f, Time.deltaTime);
        }

        public void PlayActionAnimation(AnimatorState animationState) => StateFor(animationState);
        public void StopAction() => StateFor(AnimatorState.StopAction);
        public void ResetToIdle() => _animator.Play(_idleStateHash, -1);
        public void PlayCarryAnimation() => StateFor(AnimatorState.Carry);
        public void StopCarryAnimation() => StateFor(AnimatorState.StopCarry);
        public void EnteredState(AnimationState state) => StateEntered?.Invoke(state);
        public void ExitedState(AnimatorState stateHash) => StateExited?.Invoke(StateFor(stateHash));

        private AnimatorState StateFor(AnimatorState animationState)
        {
            switch (animationState)
            {
                case AnimatorState.Walking:
                    break;

                case AnimatorState.Attack:
                    _animator.SetTrigger(_heroAttackHash);
                    break;
                case AnimatorState.Carry:
                    _animator.SetBool(_carryActionStateHash, true);
                    _animator.SetLayerWeight(_carryLayerIndex, .6f);
                    break;
                case AnimatorState.StopCarry:
                    _animator.SetBool(_carryActionStateHash, false);
                    _animator.SetLayerWeight(_carryLayerIndex, 0);
                    break;
                case AnimatorState.StopAction:
                    _animator.SetTrigger(_stopActionStateHash);
                    // ResetToIdle();
                    break;
                default:
                    break;
            }

            return animationState;
        }
    }
}