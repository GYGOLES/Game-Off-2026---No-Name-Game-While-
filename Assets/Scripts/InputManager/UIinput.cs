using System;
using VContainer.Unity;
namespace Assets.Scripts.Inputmanager
{    
    public interface IUInput
    {
        public event Action Pause;
    }
    public class UIInput : IUInput, IInitializable, IDisposable
    {
        InputManager.UIActions _ui;
        private readonly InputBinder _binder = new();
        
        public event Action Pause;
        public UIInput(InputHub inputs) => _ui = inputs.Actions.UI;

        public void Initialize()
        {
            _binder.Bind(_ui.Pause, BindType.performed, h => Pause?.Invoke());
        }
        
        public void Dispose() => _binder.Dispose();
    }
}
