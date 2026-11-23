using Assets.Scripts.Inputmanager;
using UnityEngine;
using VContainer;

public class Menu : MonoBehaviour
{
    private IPlayerInput _pl;
    private IUInput _ui;
    private IInputModeSwitch _mode;
    private UIConfig _uiConfig;

    [Inject]
    public void Construct(IPlayerInput input, IUInput ui, IInputModeSwitch modeSwitch, UIConfig uiConfig)
    {
        _pl = input;
        _ui = ui;
        _mode = modeSwitch;
        _uiConfig = uiConfig;
    }

    void OnEnable()
    {
        _pl.Pause += EnableMenu;
        _ui.Pause += DisableMenu;
        _ui.Pause += DisablePaper;

    }
    void OnDisable()
    {
        _pl.Pause -= EnableMenu;
        _ui.Pause -= DisableMenu;
        _ui.Pause -= DisablePaper;
    }

    private void EnableMenu()
    {
        if(_uiConfig.Scores.ScoreTotal.activeSelf || _uiConfig.Ending.EndingPanel.activeSelf)return;

        for (int i = 0; i < _uiConfig.Menu.MenuComponents.Length; i++)
        {
            _uiConfig.Menu.MenuComponents[i].SetActive(true);
        }

        _uiConfig.Menu.Pause.SetActive(true);
        _uiConfig.Menu.Options.SetActive(false);
        _uiConfig.Menu.Devs.SetActive(false);

        for (int i = 0; i < _uiConfig.Menu.GameplayComponents.Length; i++)
        {
            _uiConfig.Menu.GameplayComponents[i].SetActive(false);
        }
        SetUi();
    }
    public void DisableMenu()
    {
        if(_uiConfig.Scores.ScoreTotal.activeSelf || _uiConfig.Paper.Current.activeSelf || _uiConfig.Ending.EndingPanel.activeSelf)return;

        _uiConfig.Menu.GameplaySettings.SaveSettings();

        for (int i = 0; i < _uiConfig.Menu.MenuComponents.Length; i++)
        {
            _uiConfig.Menu.MenuComponents[i].SetActive(false);
        }

        _uiConfig.Menu.Pause.SetActive(false);
        _uiConfig.Menu.Options.SetActive(false);
        _uiConfig.Menu.Devs.SetActive(false);

        for (int i = 0; i < _uiConfig.Menu.GameplayComponents.Length; i++)
        {
            _uiConfig.Menu.GameplayComponents[i].SetActive(true);
        }
        SetGameplay();
    }

    public void DisablePaper()
    {
        if(!_uiConfig.Paper.Current.activeSelf)return;

        _uiConfig.Paper.Current.SetActive(false);
        SetGameplay();
    }

    private void SetUi()
    {
        _mode.SetUI();
        Time.timeScale = 0f;
    }
    public void SetGameplay()
    {
        _mode.SetGameplay();
        Time.timeScale = 1f;
    }
}
