using UnityEngine.InputSystem;

namespace Assets.Scripts.Inputmanager
{
    public static class InputExtensions 
    {
        public static InputPhase ToPhase(this InputAction.CallbackContext ctx) => ctx.phase switch
        {
            InputActionPhase.Started   => InputPhase.started,
            InputActionPhase.Performed => InputPhase.performed,
            InputActionPhase.Canceled  => InputPhase.canceled,
            _ => throw new System.ArgumentOutOfRangeException(nameof(ctx), ctx.phase, "Unsupported input phase")
        };
    }
}
