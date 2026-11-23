using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

public class GameplaySettings : MonoBehaviour
{
    [SerializeField] Toggle _windowScreen;
    [SerializeField] Toggle _vsync;
    [SerializeField] TMPro.TMP_Dropdown _language;
    [SerializeField] Slider _sensivity;


    [Header("Volume")]
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _musicSlider;

    private string _windScreenKey = "apply_window_Screen";
    private string _vsyncKey = "apply_Vsync";
    private string _locale = "apply_locale";
    private string _volume = "apply_volume";

    private PlayerConfig _config;

    [Inject]
    public void Construct(PlayerConfig config) => _config = config; 

    void Start()
    {
        _windowScreen.SetIsOnWithoutNotify(PlayerPrefs.GetInt(_windScreenKey, 1) == 1);
        _vsync.SetIsOnWithoutNotify(PlayerPrefs.GetInt(_vsyncKey, 1) == 1);
        _sensivity.SetValueWithoutNotify(ToSlider(_config.Sensitivity));
        _musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(_volume, 0.75f));

        ApplyVsync(_vsync.isOn);
        ApplyWindowScreen(_windowScreen.isOn);
        ApplyVolume(_musicSlider.value);

        StartCoroutine(Language());
    }

    private IEnumerator Language()
    {
        yield return LocalizationSettings.InitializationOperation;
        int current = PlayerPrefs.GetInt(_locale, 1);
        var locales = LocalizationSettings.AvailableLocales.Locales;
        if(current >= 0 && current < locales.Count)
        {
            _language.SetValueWithoutNotify(current);
            LocalizationSettings.SelectedLocale = locales[current];
            _language.RefreshShownValue();
        }
    }

    public void SetWindowScreen()
    {
        ApplyWindowScreen(_windowScreen.isOn);
        PlayerPrefs.SetInt(_windScreenKey, _windowScreen.isOn? 1 : 0);
    }

    public void SetVsync()
    {
        ApplyVsync(_vsync.isOn);
        PlayerPrefs.SetInt(_vsyncKey, _vsync.isOn? 1 : 0);
    }

    public void SetLanguage(int index)
    {
        var locales = LocalizationSettings.AvailableLocales.Locales;
        if (index >= 0 && index < locales.Count)
        {
            _language.SetValueWithoutNotify(index);
            LocalizationSettings.SelectedLocale = locales[index];
            PlayerPrefs.SetInt(_locale, index);
        }
    }

    public void SetVolume(float f)
    {
        ApplyVolume(f);
        PlayerPrefs.SetFloat(_volume, f);
    }

    public void SetSensivtiy(float s) => _config.SetSensitivity(FromSlider(s));  

    private void ApplyVolume(float f) => _mixer.SetFloat("Volume", Mathf.Log10(Mathf.Clamp(f, 0.0001f, 1f))*20);
    private void ApplyWindowScreen(bool on) => Screen.fullScreenMode = on? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    private void ApplyVsync(bool on) => QualitySettings.vSyncCount = on? 1 : 0;
    
    static float ToSlider(float sens) => Mathf.Log(sens / PlayerConfig.MinSensitivity) / Mathf.Log(PlayerConfig.MaxSensitivity / PlayerConfig.MinSensitivity);
    static float FromSlider(float t) => PlayerConfig.MinSensitivity * Mathf.Pow(PlayerConfig.MaxSensitivity / PlayerConfig.MinSensitivity, t);
    public void SaveSettings() => _config.Save();
    public void QuitGame() => Application.Quit();

    void OnDisable() => SaveSettings();
}
