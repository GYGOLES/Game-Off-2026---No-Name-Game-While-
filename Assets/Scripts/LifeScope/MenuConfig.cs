using UnityEngine;
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
    [SerializeField] GameObject _disclaimer;
    [SerializeField] GameObject _setLanguage;

    public GameObject SetLanguagePanel => _setLanguage;
    public GameObject SetDisclaimer => _disclaimer;
    public const string SetLanguageKey = "language_key";

    public void Init()
    {
        _setLanguage.SetActive(PlayerPrefs.GetInt(SetLanguageKey, 1) == 1);
        _disclaimer.SetActive(PlayerPrefs.GetInt(SetLanguageKey, 1) == 0);
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