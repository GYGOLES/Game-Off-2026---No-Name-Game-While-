using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Assets.Scripts.Inputmanager
{
    [Flags]
    public enum BindType{started = 1<<0, performed = 1<<1,canceled = 1<<2}


    public enum InputPhase{started, performed, canceled}
    public enum InputMode{None, Gameplay, UI}

    public interface IInputService
    {
        public InputMode CurrentMode{get;}

        public event Action<Vector2> Move;
        public event Action<Vector2> Look;

        public event Action<InputPhase> Interact;

        public void SetGameplay();
        public void SetUI();
    }
    public class InputServices : IInputService, IInitializable, IDisposable
    {
        private sealed class InputBindings
        {
            public readonly InputAction action;
            public readonly BindType type;
            public readonly Action<InputAction.CallbackContext> ctx;

            public InputBindings(InputAction a, BindType t, Action<InputAction.CallbackContext> c)
            {
                action = a;
                type = t;
                ctx = c;
            }
        }

        private readonly InputManager actions;
        public InputMode CurrentMode{get; private set;}

        readonly List<InputBindings> binds;

        const BindType PC  = BindType.performed | BindType.canceled;
        const BindType SC  = BindType.started | BindType.canceled;
        const BindType All = BindType.started | BindType.performed | BindType.canceled;

        public event Action<Vector2> Move;
        public event Action<Vector2> Look;

        public event Action<InputPhase> Interact;


        static private InputPhase ToPhase(InputAction.CallbackContext ctx)
        {
            return ctx.phase switch
            {
                InputActionPhase.Started => InputPhase.started,
                InputActionPhase.Performed => InputPhase.performed,
                InputActionPhase.Canceled => InputPhase.canceled,
                _ => throw new ArgumentOutOfRangeException(nameof(ctx), ctx.phase,"Unsupported input phase")
            };
        }

        public InputServices()
        {
            actions = new InputManager();
            var p = actions.Player;

            binds = new()
            {
                new(p.Move, PC, c => Move?.Invoke(c.ReadValue<Vector2>())), 
                new(p.Look, PC, c => Look?.Invoke(c.ReadValue<Vector2>())), 
                new(p.Interact, All, c => Interact?.Invoke(ToPhase(c))), 
            };
        }
        public void Initialize()
        {
            if (CurrentMode != InputMode.None) return;
            
            foreach(var bind in binds)
            {
                Subscribe(bind.action, bind.type, bind.ctx);
                Debug.Log($"{"actions Subbed"}");
            }

            ApplyMode(InputMode.Gameplay);
        }

        public void Dispose()
        {
            foreach(var bind in binds)
            {
                UnSubscribe(bind.action, bind.type, bind.ctx);
                Debug.Log($"{"actions unSubbed"}");
            }

            binds.Clear();
            actions.Disable();
            actions.Dispose();
        }

        void Subscribe(InputAction action, BindType type, Action<InputAction.CallbackContext> ctx)
        {
            if(type.HasFlag(BindType.started)){action.started += ctx;}
            if(type.HasFlag(BindType.performed)){action.performed += ctx;}
            if(type.HasFlag(BindType.canceled)){action.canceled += ctx;}
        }
        void UnSubscribe(InputAction action, BindType type, Action<InputAction.CallbackContext> ctx)
        {
            if(type.HasFlag(BindType.started)){action.started -= ctx;}
            if(type.HasFlag(BindType.performed)){action.performed -= ctx;}
            if(type.HasFlag(BindType.canceled)){action.canceled -= ctx;}
        }

        public void SetGameplay()
        {
            if(CurrentMode == InputMode.Gameplay)return;
            ApplyMode(InputMode.Gameplay);
        }

        public void SetUI()
        {
            if(CurrentMode == InputMode.UI)return;
            ApplyMode(InputMode.UI);
        }

        void ApplyMode(InputMode mode)
        {
            bool gameplay = mode == InputMode.Gameplay;

            if(gameplay) {actions.UI.Disable(); actions.Player.Enable();}
            else{actions.Player.Disable(); actions.UI.Enable();}

            CurrentMode = mode;
            Cursor.lockState = gameplay ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !gameplay;
        }
    }
}

