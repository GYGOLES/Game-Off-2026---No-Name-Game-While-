using Assets.Scripts.Inputmanager;
using Assets.Scripts.Player.Hub;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using VContainer;

public class PlayerUI : MonoBehaviour
{
    private PlayerStates _states;
    private PlayerCarry _carry;
    private PlayerConfig _config;
    private PlayerInteract _interact;
    private PlayerMotor _motor;
    private IInputModeSwitch _mode;


    [Header("UI")]
    [SerializeField] TextMeshProUGUI _bagStorage;
    [SerializeField] TextMeshProUGUI _stamina;
    [SerializeField] TextMeshProUGUI _interaction;

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
    private LocalizedString _currentLocalized;
    private Outline _currentOutline;
    private int _day = 1;
    private GarbageLocator _locator;

    bool _allCollected;


    [Inject]
    public void Construct(PlayerStates states, PlayerMotor motor, PlayerCarry carry, PlayerConfig config, PlayerInteract interact, IInputModeSwitch inputMode)
    {
        _states = states;
        _carry = carry;
        _config = config;
        _interact = interact;   
        _motor = motor;
        _mode = inputMode;

        _carry.BagIntChange   += BagText;
        _carry.TotalBagChange += TotalCollected;
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
            _carry.TotalBagChange -= TotalCollected;
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

    private void TotalCollected(int g)
    {
        _scoreTotalCollected.text = g.ToString("F0") + "/" + _locator.Total.ToString("F0");
    }

    public void Stamina()
    {
        if(_stamina == null || _states == null)return;

        _stamina.text = _states.Stamina.ToString("F0");
    }

    public void Timer()
    {
        if(_timerCounter == null)return;

        _timerText.text = _timerCounter.CurrentTimer.ToString("F0");
    }

    public void GarbageText(int remain, int total)
    {
        _Items.text = remain.ToString("F0");
        _scoreTotalCollected.text = remain.ToString("F0") + "/" + _locator.Total.ToString("F0");
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
        if (ReferenceEquals(interact, _currentInteract)) return;

        ClearPrompt();
        
        _currentInteract = interact;
        if(interact == null)return;

        LocalizedString text = interact.Prompt;

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
        _allCollected = false;
        _shiftDay.text = _day.ToString();
        _timerCounter.SetTimerBack();
        _locator.RestartLocator();
        _carry.FullDrop();

        _motor.Teleport(_spawnLocation);
        SetLocator(_day - 1);
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

    private void LocalizedText(string value) => _interaction.text = value;

    void Update()
    {
        Stamina(); Timer(); ShowInteractText();
    }
}
