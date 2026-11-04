using System;
using UnityEngine;
using VContainer.Unity;

namespace Assets.Scripts.Inputmanager
{
    public interface IPlayerInput
    {
        public event Action<Vector2> Move;
        public event Action<Vector2> Look;

        public event Action<InputPhase> Interact;
        public event Action<bool> Sprint;
    }
    public class PlayerInput : IPlayerInput, IInitializable, IDisposable
    {
        private readonly InputBinder _binder = new();
        private readonly InputManager.PlayerActions _p;

        public event Action<Vector2> Move;
        public event Action<Vector2> Look;
        public event Action<InputPhase> Interact;
        public event Action<bool> Sprint;

        public PlayerInput(InputHub h) => _p = h.Actions.Player;

        public void Initialize()
        {
            _binder.Bind(_p.Move, BindType.performed | BindType.canceled, h => Move?.Invoke(h.ReadValue<Vector2>()));
            _binder.Bind(_p.Look, BindType.performed | BindType.canceled, h => Look?.Invoke(h.ReadValue<Vector2>()));
            _binder.Bind(_p.Interact, BindType.started| BindType.performed | BindType.canceled, h => Interact?.Invoke(InputExtensions.ToPhase(h)));
            _binder.Bind(_p.Sprint, BindType.started| BindType.performed | BindType.canceled, h => Sprint?.Invoke(h.performed));

            Debug.Log($"{"Input: Intialized"}");
        }

        

        public void Dispose() => _binder.Dispose();
    }

}
