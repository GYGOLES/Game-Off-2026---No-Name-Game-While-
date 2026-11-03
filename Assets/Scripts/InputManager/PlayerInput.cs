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
    }
    public class PlayerInput : IPlayerInput, IInitializable, IDisposable
    {
        private readonly InputBinder _binder = new();
        private readonly InputManager.PlayerActions _p;

        public event Action<Vector2> Move;
        public event Action<Vector2> Look;
        public event Action<InputPhase> Interact;

        public PlayerInput(InputHub h) => _p = h.Actions.Player;

        public void Initialize()
        {
            _binder.Bind(_p.Move, BindType.performed | BindType.canceled, h => Move?.Invoke(h.ReadValue<Vector2>()));
            _binder.Bind(_p.Look, BindType.performed | BindType.canceled, h => Look?.Invoke(h.ReadValue<Vector2>()));
            _binder.Bind(_p.Interact, BindType.started| BindType.performed | BindType.canceled, h => Interact?.Invoke(InputExtensions.ToPhase(h)));

            Debug.Log($"{"Input: Intialized"}");
        }

        

        public void Dispose() => _binder.Dispose();
    }

}
