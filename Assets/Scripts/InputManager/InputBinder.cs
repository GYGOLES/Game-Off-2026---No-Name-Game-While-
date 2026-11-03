using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Inputmanager
{
    [Flags]
    public enum BindType{started = 1<<0, performed = 1<<1,canceled = 1<<2}

    public enum InputPhase{started, performed, canceled}

    public class InputBinder : IDisposable
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
        private readonly List<InputBindings> _binds = new();

        public void Bind(InputAction a, BindType t, Action<InputAction.CallbackContext> h)
        {
            if (t.HasFlag(BindType.started))   a.started   += h;
            if (t.HasFlag(BindType.performed)) a.performed += h;
            if (t.HasFlag(BindType.canceled))  a.canceled  += h;
            _binds.Add(new(a, t, h));
        }
        public void Dispose()
        {
            foreach(var b in _binds)
            {
                if (b.type.HasFlag(BindType.started))  b.action.started   -= b.ctx;
                if (b.type.HasFlag(BindType.performed))  b.action.performed   -= b.ctx;
                if (b.type.HasFlag(BindType.canceled))  b.action.canceled   -= b.ctx;
            }
            _binds.Clear();
        }
    }
}
