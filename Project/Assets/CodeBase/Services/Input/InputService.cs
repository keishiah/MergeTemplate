using UnityEngine;

namespace CodeBase.Services.Input
{
    public abstract class InputService : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        private const string Button = "Fire";

        public abstract Vector3 Axis { get; }
        

        protected static Vector3 SimpleInputAxis()
        {
            return new Vector3(SimpleInput.GetAxis(Horizontal), 0, SimpleInput.GetAxis(Vertical));
        }
    }
}