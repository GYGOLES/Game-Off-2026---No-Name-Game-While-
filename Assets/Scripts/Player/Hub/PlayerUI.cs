using Assets.Scripts.Inputmanager;
using Assets.Scripts.Player.Hub;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using VContainer;

public class PlayerUI : MonoBehaviour
{
    private PlayerStates _states;
    private PlayerCarry _carry;
    private PlayerConfig _config;
    private MessageRefs _message;
    private PlayerInteract _interact;
    private PlayerMotor _motor;
    private PlayerHand _hand;
    private PlayerFear _fear;
    private IInputModeSwitch _mode;


    
    private TextMeshProUGUI _bagStorage;
    private Image _staminaImage;
    private Image _hold;
    

    
    private TextMeshProUGUI _interaction;

    private AudioSource _heartBeating;
    
    private GameObject _scoreTotal;
    private TextMeshProUGUI _scoreTotalCollected;
    private TextMeshProUGUI _scoreDay;

    
    private TextMeshProUGUI _items;
    [SerializeField] GarbageLocator[] _locators;
    private Transform _spawnLocation;

    
    private GameObject _paperObj;
    private Image _paperSprite;
    private TextMeshProUGUI _paperText;
    private LocalizedString _currentPaperText;

    private GameObject _tutorialPanel;
    private LocalizedString[] _tutorials;
    private TextMeshProUGUI _tutorial;
    private bool _acceptedTutorial;


    private GameObject _garbageTexture;
    private Vector3 _lowerPosition;

    private IInteract _currentInteract;
    private InteractResult _currentResult;
    private IPlayerInput _input; 

    private LocalizedString _currentLocalized;
    private Outline _currentOutline;
    private int _day = 1;
    private GarbageLocator _locator;

    private TimerCounter _timerCounter;
    private TextMeshProUGUI _timerText;

    private bool _allCollected;
    private bool _interacted;


    [Inject]
    public void Construct(PlayerStates states, PlayerMotor motor, PlayerCarry carry, PlayerHand hand, PlayerConfig config, UIConfig ui, PlayerInteract interact, PlayerFear fear, IInputModeSwitch inputMode, IPlayerInput input)
    {
        _states = states;
        _carry = carry;
        _config = config;
        _interact = interact;   
        _motor = motor;
        _mode = inputMode;
        _hand = hand;
        _input = input;
        _fear = fear;

        _timerCounter = ui.Timer.Counter; _timerText = ui.Timer.Text;
        _paperObj = ui.Paper.Panel; _paperSprite = ui.Paper.Image; _paperText = ui.Paper.Text;
        _interaction = ui.Messages.MessageObject; _bagStorage = ui.Messages.BagStorage; _hold = ui.Messages.Hold; _message = ui.Messages;
        _staminaImage = ui.Player.Stamina;
        _scoreTotal = ui.Scores.ScoreTotal; _scoreDay = ui.Scores.ScoreDay; _scoreTotalCollected = ui.Scores.ScoreTotalCollected;
        _items = ui.Map.TotalItems; _spawnLocation = ui.Map.Spawn; _garbageTexture = ui.Map.GarbageTexture; _heartBeating = ui.Map.HeartBeating;
        _tutorial = ui.Tutorial.Text; _tutorials = ui.Tutorial.Prompts; _tutorialPanel = ui.Tutorial.Panel; _acceptedTutorial = ui.Tutorial.Enabled;

        int i = PlayerPrefs.GetInt("AcceptedTutorial", 0);
        _acceptedTutorial = (i == 1);
        ShowTutorialText(0);

        _input.Interact += Interact;

        _carry.BagIntChange += BagText;
        _carry.OnDrop += DropBag;
        _carry.OnShowText += ShowPapperText;
        _carry.OnFirstCollected += ShowTutorialText;
        _carry.OnFirstDropped += ShowTutorialText;

        _hand.OnFirstTimeCollected += ShowTutorialText;

        _fear.Ambience += GainHeart;
        _fear.FearDrained += DrainHeart;

        if (_locator != null)
        {
            _locator.Changed  += GarbageText;
            _locator.AllCollected += OnCollected; 
        }

        _motor.Teleport(_spawnLocation);

        _timerCounter.TimerDeath += ShowScore;   
        
    }
    void OnDestroy()
    {
        if (_carry != null)
        {
            _carry.BagIntChange   -= BagText;
            _carry.OnDrop -= DropBag;
            _carry.OnShowText -= ShowPapperText;

            _carry.OnFirstCollected -= ShowTutorialText;
            _carry.OnFirstDropped -= ShowTutorialText;
        }
        if (_locator != null)
        {
            _locator.Changed -= GarbageText;
            _locator.AllCollected -= OnCollected; 
        }
        if(_hand != null) _hand.OnFirstTimeCollected -= ShowTutorialText;
        if(_input != null) _input.Interact -= Interact;
        if (_timerCounter != null) _timerCounter.TimerDeath -= ShowScore;
        if(_currentInteract != null) ClearPrompt();
        if(_fear != null)
        {
            _fear.Ambience -= GainHeart; 
            _fear.FearDrained -= DrainHeart;
        }

    }
    void Start()
    {
        _lowerPosition = _garbageTexture.transform.localPosition;
        BagText(_carry.CurrentBagCollected is var b ? (int)b : 0);
        SetLocator(_day - 1);
    }
    public void BagText(int f)
    {
        if(_bagStorage == null)return;

        _bagStorage.text = f.ToString() + "/" + _config.MaxBagStorage.ToString();

        ChangePosition(f);
    }

    private void ChangePosition(int f)
    {
        float t = Mathf.Clamp01((float)f/_config.MaxBagStorage);
        float pos = Mathf.Lerp(_lowerPosition.y, 0.05f, t);
        
        Vector3 p = _garbageTexture.transform.localPosition;
        p.y = pos;
        _garbageTexture.transform.localPosition = p;
    }

    private void DropBag()
    {
        _garbageTexture.transform.localPosition = _lowerPosition;

        if(_allCollected)
            ShowScore();
    }

    public void ShowScore()
    {
        if (_scoreTotal.activeSelf) return;
        if(_mode == null)return;

        ClearPrompt();
        _interaction.gameObject.SetActive(false);

        _scoreTotal.SetActive(true);   
        _mode.SetUI();
        Time.timeScale = 0f;
    }

    private void OnCollected()
    {
        _allCollected = true;
    }

    public void Stamina()
    {
        if(_staminaImage == null || _states == null)return;

        _staminaImage.fillAmount = _states.StaminaNormalized;
    }

    public void Timer()
    {
        if(_timerCounter == null)return;

        _timerText.text = _timerCounter.CurrentTimer.ToString("F0");
    }

    public void GarbageText(int remain, int total)
    {
        _items.text = remain.ToString("F0");
        _scoreTotalCollected.text = (total - remain).ToString("F0") + "/" + _locator.Total.ToString("F0");
    }

    private void ShowInteractText()
    {
        IInteract interact = null;

        if (_interact.OnInteract(out RaycastHit hit))
            interact = hit.collider.GetComponentInParent<IInteract>();

        SetPrompt(interact);
    }   

    private void Interact(InputPhase phase)
    {
        _interacted = phase == InputPhase.started;
        if(!_interacted) _hold.fillAmount = 0f;
    }

    private void ShowInteractionDuration()
    {
        bool show = _currentInteract != null && _currentInteract.Phase == InputPhase.performed && _currentResult == InteractResult.Success;

        _hold.fillAmount = show? _input.HoldProgress : 0f;
    }

    private void SetPrompt(IInteract interact)
    {
        InteractResult r = interact != null ? _interact.Check(interact) : InteractResult.None;

        if (ReferenceEquals(interact, _currentInteract) && r == _currentResult) return;

        ClearPrompt();
        
        _currentInteract = interact;
        _currentResult = r;
        if(interact == null)return;

        LocalizedString text = _currentResult == InteractResult.Success? interact.Prompt : _message.Get(_currentResult);
 
        if (text != null && !text.IsEmpty)
        {
            _currentLocalized = text;
            _currentLocalized.StringChanged += LocalizedInteractionText;
            _interaction.gameObject.SetActive(true);
        }

       _currentOutline = interact.Outline;
        if (_currentOutline != null)
            _currentOutline.enabled = true;
    }

    private void ClearPrompt()
    {
        if(_currentLocalized != null)
        {
            _currentLocalized.StringChanged -= LocalizedInteractionText;
            _currentLocalized = null;
        }
        if(_currentOutline != null)
        {
            _currentOutline.enabled = false;
            _currentOutline = null;
        }

        _currentInteract= null;

        if (_interaction != null && _interaction.gameObject.activeSelf)
            _interaction.gameObject.SetActive(false);
    }

    private void ClearPaperText()
    {
        if(_currentPaperText != null)
        {
            _currentPaperText.StringChanged -= LocalizedPapperText;
            _currentPaperText = null;
        }
    }

    public void SetNextShift(int i)
    {
        _day += i;
        _scoreDay.text = _day.ToString();

        Reset();

        SetLocator(_day - 1);

        ResumeGameplay();
        
    }

    void ResumeGameplay()
    {
        if(_mode == null)return;

        _scoreTotal.SetActive(false);
        _mode.SetGameplay();
        Time.timeScale = 1f;
    }

    public void RestartShift()
    {
        ShowTutorialText(0);
        _scoreDay.text = _day.ToString();
        
        _locator.RestartLocator();
        SetLocator(_day - 1);
        Reset();
        _states.ResetStamina();
        ResumeGameplay();
    }

    private void Reset()
    {
        _timerCounter.SetTimerBack();
        _motor.Teleport(_spawnLocation);
        _carry.FullDrop();
        _hand.DropBag();
    }

    private void SetLocator(int index)
    {
        _allCollected = false;
        if (_locator != null)
        {
            _locator.Changed -= GarbageText;
            _locator.AllCollected -= OnCollected;
            _locator.gameObject.SetActive(false);
        }

        _locator = _locators[index];
        _locator.gameObject.SetActive(true);
        _locator.Changed += GarbageText;
        _locator.AllCollected += OnCollected;
        
        GarbageText(_locator.Total, _locator.Total);
    }

    private void ShowPapperText(LocalizedString p, Sprite s)
    {
        ClearPaperText();
        
        if(p != null || !p.IsEmpty)
        {
            _paperObj.SetActive(true);
            _currentPaperText = p;
            _currentPaperText.StringChanged += LocalizedPapperText;
            _mode.SetUI();
            Time.timeScale = 0f;
        }

        if(s != null)
        {
            _paperSprite.sprite = s;
        }
        
    }
    private void ShowTutorialText(int index)
    {
        if (_acceptedTutorial)
        {
            _tutorialPanel.SetActive(false);
            return;
        }

        if(index == 2)
        {
            _acceptedTutorial = true;
            _tutorialPanel.SetActive(false);
            PlayerPrefs.SetInt("AcceptedTutorial", _acceptedTutorial ? 1 : 0);
            PlayerPrefs.Save();
            return;
        }

        if(_tutorials != null || _tutorials.Length > 0)
        {
            _tutorials[index].StringChanged += LocalizedTutorialText;
            _tutorial.gameObject.SetActive(true);
        }
    }

    private void LocalizedInteractionText(string value) => _interaction.text = value;
    private void LocalizedPapperText(string v) => _paperText.text = v;
    private void LocalizedTutorialText(string v) => _tutorial.text = v;

    private void SetPitch() => _heartBeating.pitch = _fear.FearNormalized >= 0.8f? 1.2f : _fear.FearNormalized >= 0.3f? 1f : 0.8f;
    private void SetVolume() => _heartBeating.volume = _fear.FearNormalized;
    private void GainHeart(float h)
    {   
        if(!_heartBeating.isPlaying)
            _heartBeating.Play();

        SetPitch();
        SetVolume();
    }

    private void DrainHeart()
    {
        SetPitch();
        SetVolume();
        
        if(_heartBeating.volume <= 0)
            _heartBeating.Stop();
    }

    void Update()
    {
        Stamina(); Timer(); 
        
        if(_scoreTotal.activeSelf || _paperObj.activeSelf)return;
        
        ShowInteractText(); if(_interacted)ShowInteractionDuration();

    }
}
