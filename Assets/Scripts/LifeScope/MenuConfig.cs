using PrimeTween;
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
    [Header("System")]
    [SerializeField] EventSystem _system;
    [SerializeField] GameObject _currentButton;

    [Header("CanvasGroup")]
    [SerializeField] CanvasGroup _menuCanvas;
    [SerializeField] CanvasGroup _languageCanvas;
    [SerializeField] CanvasGroup _disclaimerCanvas;

    [Header("BackGround")]
    [SerializeField] RawImage _background;

    [Header("Duration")]
    [SerializeField] float _duration = 0.5f;

    public GameObject CurrentButton => _currentButton;
    public RawImage Background => _background;

    public CanvasGroup LanguageCanvas => _languageCanvas;
    public CanvasGroup DisclaimerCanvas => _disclaimerCanvas;

    private Tween _languageTween;
    private Tween _disclaimerTween;

    public const string SetLanguageKey = "language_key";

    public void Init()
    {
        Time.timeScale = 1f;

        _languageCanvas.gameObject.SetActive(PlayerPrefs.GetInt(SetLanguageKey, 1) == 1);
        _disclaimerCanvas.gameObject.SetActive(PlayerPrefs.GetInt(SetLanguageKey, 1) == 0);
        if(_disclaimerCanvas.gameObject.activeSelf)
            _disclaimerTween = Tween.Alpha(_disclaimerCanvas, 1, _duration*14);

        _system.firstSelectedGameObject = _languageCanvas.gameObject.activeSelf? _currentButton : _disclaimerCanvas.gameObject;
    }

    public void TweenLanguage()
    {
        if(!_languageCanvas.gameObject.activeSelf)return;
        
        _languageCanvas.interactable = false;
        _languageTween = Tween.Alpha(_languageCanvas, 0f, _duration)
        .OnComplete(() => 
        {
            _languageCanvas.gameObject.SetActive(false);
            _disclaimerCanvas.gameObject.SetActive(true);
            _disclaimerTween = Tween.Alpha(_disclaimerCanvas, 1f, _duration).OnComplete(() =>
            {
                _system.firstSelectedGameObject = _disclaimerCanvas.gameObject;
            });  
        });
    }
    public void TweenDisclaimer()
    {
        _disclaimerCanvas.interactable = false;

        _disclaimerTween = Tween.Alpha(_disclaimerCanvas, 0f, _duration).OnComplete(() =>
        {
            _disclaimerCanvas.gameObject.SetActive(false);
            _menuCanvas.gameObject.SetActive(true);

            _disclaimerTween = Tween.Alpha(_menuCanvas, 1f, _duration);
        });
    }
 
    public void SaveLanguage() => PlayerPrefs.SetInt(SetLanguageKey, 0);
    public void DeleteKey() => PlayerPrefs.DeleteKey(SetLanguageKey);

    public void OnDestroy()
    {
        _disclaimerTween.Stop();
        _languageTween.Stop();
    }

    public void Tick()
    {
        Rect target = _background.uvRect;

        target.x -= 0.01f * Time.deltaTime;
        target.y -= 0.005f * Time.deltaTime;

        _background.uvRect = target;
    }
}
[System.Serializable]
public sealed class LoadingScene
{
    [SerializeField] GameObject _loadingPanel;
    [SerializeField] Image _loadingBar;

    public GameObject LoadingPanel => _loadingPanel;
    public Image LoadingBar => _loadingBar;
}