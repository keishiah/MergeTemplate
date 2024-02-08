using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.Hero
{
    public class HeroMove : MonoBehaviour
    {
        //обычный мув игрока, как на курсе
        [Inject] private MobileInputService _mobileInputService;

        public float speed = 2.0f;
        private CharacterController _characterController;

        private void Start()
        {
            _characterController = GetComponentInParent<CharacterController>();
        }

        private void Update()
        {
            Vector3 inputDir = _mobileInputService.Axis;
            Vector3 movementVector = Vector3.zero;

            movementVector = inputDir.normalized;
            movementVector.y = 0;

            if (inputDir.sqrMagnitude > .01f)
            {
                transform.forward = movementVector;
            }

            movementVector += Physics.gravity / 2;
            _characterController.Move(speed * movementVector * Time.deltaTime);
        }
    }
}