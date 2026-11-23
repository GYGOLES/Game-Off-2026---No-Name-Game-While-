using UnityEngine;

[System.Serializable]
public class MenuConfig
{
    [SerializeField] LanguageRefs _languageRefs;

    public LanguageRefs Language => _languageRefs;
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