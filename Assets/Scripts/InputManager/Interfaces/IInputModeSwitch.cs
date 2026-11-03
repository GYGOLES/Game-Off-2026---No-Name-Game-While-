namespace Assets.Scripts.Inputmanager
{
    public enum InputMode{None, Gameplay, UI}
    public interface IInputModeSwitch 
    {
        public void SetGameplay();
        public void SetUI();
        public InputMode CurrentMode{get;}   
    }
}
