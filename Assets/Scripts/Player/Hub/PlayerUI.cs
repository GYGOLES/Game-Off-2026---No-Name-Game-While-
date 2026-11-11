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

    [Header("ItemInShift")]
    [SerializeField] TextMeshProUGUI _Items;
    [SerializeField] GarbageLocator _locator;

    private LocalizedString _currentLocalized;

    [Inject]
    public void Construct(PlayerStates states, PlayerCarry carry, PlayerConfig config, PlayerInteract interact)
    {
        _states = states;
        _carry = carry;
        _config = config;
        _interact = interact;   

        _carry.BagIntChange   += BagText;
        _carry.TotalBagChange += TotalCollected;
        _locator.Changed      += GarbageText;
        _timerCounter.TimerDeath += ShowScore;    
    }
    void OnDestroy()
    {
        if (_carry != null)
        {
            _carry.BagIntChange   -= BagText;
            _carry.TotalBagChange -= TotalCollected;
        }
        if (_locator != null) _locator.Changed -= GarbageText;
        if (_timerCounter != null) _timerCounter.TimerDeath -= ShowScore;
    }
    void Awake()
    {
        BagText(_carry.CurrentBagCollected is var b ? (int)b : 0);
        TotalCollected(0);
        GarbageText(_locator.Total, _locator.Total);
    }
    public void BagText(int f)
    {
        if(_bagStorage == null)return;

        _bagStorage.text = f.ToString() + "/" + _config.MaxBagStorage.ToString();
    }

    public void ShowScore()
    {
        _EndShift.SetActive(true);   
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
        _Items.text = remain.ToString("F0") + "/" + total.ToString("F0");
    }

    void ShowInteractText()
    {
        LocalizedString pr = null;

        if (_interact.OnInteract(out RaycastHit hit))
        pr = hit.collider.GetComponentInParent<IInteract>()?.String;

        SetPrompt(pr);
    }   

    private void SetPrompt(LocalizedString pr)
    {
        if (pr == null || pr.IsEmpty)
        {
            ClearPrompt();
            return;
        }

        if (ReferenceEquals(pr, _currentLocalized)) return;

        if(_currentLocalized != null)
            _currentLocalized.StringChanged -= LocalizedText;

        _currentLocalized = pr;
        _currentLocalized.StringChanged += LocalizedText;
        _interaction.gameObject.SetActive(true);
    }

    private void ClearPrompt()
    {
        if(_currentLocalized != null)
        {
            _currentLocalized.StringChanged -= LocalizedText;
            _currentLocalized = null;
        }
        if(_interaction.gameObject.activeSelf)
            _interaction.gameObject.SetActive(false);
    }

    private void LocalizedText(string value) => _interaction.text = value;

    void Update()
    {
        Stamina(); Timer(); ShowInteractText();
    }
}
