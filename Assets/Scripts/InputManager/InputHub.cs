using System;
using UnityEngine;
using VContainer.Unity;

namespace Assets.Scripts.Inputmanager
{
    public class InputHub : IInputModeSwitch, IInitializable, IDisposable
    {
        public InputManager Actions{get; private set;} = new();
        public InputMode CurrentMode{get; private set;}

        
        public void Initialize() => ApplyMode(InputMode.None);

        public void Dispose()
        {
            Actions.Disable();
            Actions.Dispose();

            Debug.Log($"{"Disposed"}");
        }

        public void SetGameplay(){if(CurrentMode != InputMode.Gameplay) ApplyMode(InputMode.Gameplay);}
        public void SetUI(){if(CurrentMode != InputMode.UI) ApplyMode(InputMode.UI);}

        void ApplyMode(InputMode mode)
        {
            if (mode == InputMode.None) { Actions.Disable(); CurrentMode = mode; return; }

            bool gameplay = mode == InputMode.Gameplay;

            if(gameplay) {Actions.UI.Disable(); Actions.Player.Enable();}
            else{Actions.Player.Disable(); Actions.UI.Enable();}

            CurrentMode = mode;
            Cursor.lockState = gameplay ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !gameplay;
        }
    }
}
