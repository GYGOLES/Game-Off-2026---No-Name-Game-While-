using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class GameplaySettings : MonoBehaviour
{
    [SerializeField] Toggle _windowScreen;
    [SerializeField] Toggle _vsync;
    [SerializeField] TMPro.TMP_Dropdown _language;


    [Header("Volume")]
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _musicSlider;

    private string _windScreenKey = "apply_window_Screen";
    private string _vsyncKey = "apply_Vsync";
    private string _locale = "apply_locale";
    private string _volume = "apply_volume";

    
    void Awake()
    {
        int vsync = PlayerPrefs.GetInt(_vsyncKey, 1);
        int set = PlayerPrefs.GetInt(_windScreenKey, 1);
        
        
        _windowScreen.isOn = (set == 1);
        _vsync.isOn = (vsync == 1);
        QualitySettings.vSyncCount = vsync;
        Screen.fullScreen = (set == 1);

        StartCoroutine(Coroutine());
    }

    void Start()
    {
        float v = PlayerPrefs.GetFloat(_volume, 0.75f);
        _musicSlider.value = v;
        SetVolume(_musicSlider.value);
    }

    private IEnumerator Coroutine()
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
        Screen.fullScreen = _windowScreen.isOn;
        PlayerPrefs.SetInt(_windScreenKey, _windowScreen.isOn? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetVsync()
    {
        int value = _vsync.isOn? 1 : 0;
        QualitySettings.vSyncCount = value;
        PlayerPrefs.SetInt(_vsyncKey, value);
        PlayerPrefs.Save();
    }

    public void SetLanguage(int index)
    {
        var locales = LocalizationSettings.AvailableLocales.Locales;
        if (index >= 0 && index < locales.Count)
        {
            _language.SetValueWithoutNotify(index);
            LocalizationSettings.SelectedLocale = locales[index];
            PlayerPrefs.SetInt(_locale, index);
            PlayerPrefs.Save();
        }
    }
    public void SetVolume(float f)
    {
        float db = Mathf.Log10(Mathf.Clamp(f, 0.0001f, 1f))*20;
        _mixer.SetFloat("Volume", db);
        PlayerPrefs.SetFloat(_volume, f);
        PlayerPrefs.Save();
    }
}
