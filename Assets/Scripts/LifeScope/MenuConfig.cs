using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class MenuConfig
{
    [SerializeField] LanguageRefs _languageRefs;
    [SerializeField] LoadingScene _loadingScene;

    public LanguageRefs Language => _languageRefs;
    public LoadingScene Loading => _loadingScene;
}
[System.Serializable]
public sealed class LanguageRefs
{
    [SerializeField] EventSystem _system;
    [SerializeField] GameObject _currentButton;
    [SerializeField] GameObject _disclaimer;
    [SerializeField] GameObject _setLanguagePanel;

    public GameObject CurrentButton => _currentButton;
    public GameObject SetLanguagePanel => _setLanguagePanel;
    public GameObject SetDisclaimer => _disclaimer;
    public const string SetLanguageKey = "language_key";

    public void Init()
    {
        _setLanguagePanel.SetActive(PlayerPrefs.GetInt(SetLanguageKey, 1) == 1);
        _disclaimer.SetActive(PlayerPrefs.GetInt(SetLanguageKey, 1) == 0);
        _system.firstSelectedGameObject = _setLanguagePanel.activeSelf? _currentButton : _disclaimer;

    }

    public void SaveLanguage() => PlayerPrefs.SetInt(SetLanguageKey, 0);
    public void DeleteKey() => PlayerPrefs.DeleteKey(SetLanguageKey);
}
[System.Serializable]
public sealed class LoadingScene
{
    [SerializeField] GameObject _loadingPanel;
    [SerializeField] Image _loadingBar;

    public GameObject LoadingPanel => _loadingPanel;
    public Image LoadingBar => _loadingBar;
}