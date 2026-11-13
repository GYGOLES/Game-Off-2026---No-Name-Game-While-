using Assets.Scripts.Inputmanager;
using Assets.Scripts.Player.Hub;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using VContainer;

public class PlayerUI : MonoBehaviour
{
    [System.Serializable]
    private struct ResultMessage
    {
        public InteractResult result;
        public LocalizedString text;
    }
    private PlayerStates _states;
    private PlayerCarry _carry;
    private PlayerConfig _config;
    private PlayerInteract _interact;
    private PlayerMotor _motor;
    private PlayerHand _hand;
    private IInputModeSwitch _mode;


    [Header("UI")]
    [SerializeField] TextMeshProUGUI _bagStorage;
    [SerializeField] TextMeshProUGUI _stamina;
    [SerializeField] Image _staminaImage;
    

    [Header("Message")]
    [SerializeField] TextMeshProUGUI _interaction;
    [SerializeField] ResultMessage[] _messages;

    [Header("Timer")]
    [SerializeField] TimerCounter _timerCounter;
    [SerializeField] TextMeshProUGUI _timerText;

    [Header("EndShift")]
    [SerializeField] GameObject _EndShift;
    [SerializeField] TextMeshProUGUI _scoreTotalCollected;
    [SerializeField] TextMeshProUGUI _shiftDay;

    [Header("ItemInShift")]
    [SerializeField] TextMeshProUGUI _Items;
    [SerializeField] GarbageLocator[] _locators;
    [SerializeField] Transform _spawnLocation;

    [Header("Visual")]
    [SerializeField] GameObject _garbageTexture;
    private Vector3 _lowerPosition;

    private IInteract _currentInteract;
    private InteractResult _currentResult;

    private LocalizedString _currentLocalized;
    private Outline _currentOutline;
    private int _day = 1;
    private GarbageLocator _locator;

    private bool _allCollected;


    [Inject]
    public void Construct(PlayerStates states, PlayerMotor motor, PlayerCarry carry, PlayerHand hand, PlayerConfig config, PlayerInteract interact, IInputModeSwitch inputMode)
    {
        _states = states;
        _carry = carry;
        _config = config;
        _interact = interact;   
        _motor = motor;
        _mode = inputMode;
        _hand = hand;

        _carry.BagIntChange   += BagText;
        _carry.OnDrop += DropBag;

        if (_locator != null)
        {
            _locator.Changed      += GarbageText;
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
        }
        if (_locator != null)
        {
            _locator.Changed -= GarbageText;
            _locator.AllCollected -= OnCollected; 
        }
        if (_timerCounter != null) _timerCounter.TimerDeath -= ShowScore;
        if(_currentInteract != null) ClearPrompt();
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
        if (_EndShift.activeSelf) return;
        if(_mode == null)return;

        _EndShift.SetActive(true);   
        _mode.SetUI();
        Time.timeScale = 0f;
    }

    private void OnCollected()
    {
        _allCollected = true;
    }

    public void Stamina()
    {
        if(_stamina == null || _states == null)return;

        _stamina.text = _states.Stamina.ToString("F0");
        _staminaImage.fillAmount = _states.StaminaNormalized;
    }

    public void Timer()
    {
        if(_timerCounter == null)return;

        _timerText.text = _timerCounter.CurrentTimer.ToString("F0");
    }

    public void GarbageText(int remain, int total)
    {
        _Items.text = remain.ToString("F0");
        _scoreTotalCollected.text = (total - remain).ToString("F0") + "/" + _locator.Total.ToString("F0");
    }

    void ShowInteractText()
    {
        IInteract interact = null;

        if (_interact.OnInteract(out RaycastHit hit))
            interact = hit.collider.GetComponentInParent<IInteract>();

        SetPrompt(interact);
    }   

    private void SetPrompt(IInteract interact)
    {
        InteractResult r = interact != null ? _interact.Check(interact) : InteractResult.None;

        if (ReferenceEquals(interact, _currentInteract) && r == _currentResult) return;

        ClearPrompt();
        
        _currentInteract = interact;
        _currentResult = r;
        if(interact == null)return;

        LocalizedString text = _currentResult == InteractResult.Success? interact.Prompt : ShowText(_currentResult);
 
        if (text != null && !text.IsEmpty)
        {
            _currentLocalized = text;
            _currentLocalized.StringChanged += LocalizedText;
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
            _currentLocalized.StringChanged -= LocalizedText;
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

    public void SetNextShift(int i)
    {
        _day += i;
        _shiftDay.text = _day.ToString();
        _timerCounter.SetTimerBack();

        _motor.Teleport(_spawnLocation);
        _carry.FullDrop();
        _hand.PickUpBag();
        

        SetLocator(_day - 1);

        ResumeGameplay();
        
    }

    void ResumeGameplay()
    {
        if(_mode == null)return;

        _EndShift.SetActive(false);
        _mode.SetGameplay();
        Time.timeScale = 1f;
    }

    public void RestartShift()
    {
        _shiftDay.text = _day.ToString();
        _timerCounter.SetTimerBack();
        _locator.RestartLocator();
        _motor.Teleport(_spawnLocation);
        SetLocator(_day - 1);
        _carry.FullDrop();
        _hand.PickUpBag();
        _allCollected = false;
        _states.ResetStamina();
        ResumeGameplay();
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

    private LocalizedString ShowText(InteractResult r)
    {
        for (int i = 0; i < _messages.Length; i++)
        if (_messages[i].result == r)
            return _messages[i].text;
        return null;
    }

    private void LocalizedText(string value) => _interaction.text = value;

    void Update()
    {
        Stamina(); Timer(); ShowInteractText();
    }
}
