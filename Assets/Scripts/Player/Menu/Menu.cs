using Assets.Scripts.Inputmanager;
using UnityEngine;
using VContainer;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject[] _menu;
    [SerializeField] GameObject[] _gameplay;
    [SerializeField] GameObject _options;
    [SerializeField] GameObject _pause;

    private IPlayerInput _pl;
    private IUInput _ui;
    private IInputModeSwitch _mode;

    [Inject]
    public void Construct(IPlayerInput input, IUInput ui, IInputModeSwitch modeSwitch)
    {
        _pl = input;
        _ui = ui;
        _mode = modeSwitch;
    }

    void OnEnable()
    {
        _pl.Pause += EnableMenu;
        _ui.Pause += DisableMenu;
    }
    void OnDisable()
    {
        _pl.Pause -= EnableMenu;
        _ui.Pause -= DisableMenu;
    }

    private void EnableMenu()
    {
        for (int i = 0; i < _menu.Length; i++)
        {
            _menu[i].SetActive(true);
        }

        _pause.SetActive(true);
        _options.SetActive(false);

        for (int i = 0; i < _gameplay.Length; i++)
        {
            _gameplay[i].SetActive(false);
        }
        u
        Time.timeScale = 0f;
        _mode.SetUI();
    }
    private void DisableMenu()
    {
        for (int i = 0; i < _menu.Length; i++)
        {
            _menu[i].SetActive(false);
        }

        _pause.SetActive(false);
        _options.SetActive(false);

        for (int i = 0; i < _gameplay.Length; i++)
        {
            _gameplay[i].SetActive(true);
        }
        Time.timeScale = 1f;
        _mode.SetGameplay();
    }
}
