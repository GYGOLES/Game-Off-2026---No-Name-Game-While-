using TMPro;
using UnityEngine;
using VContainer;

public class PlayerUI : MonoBehaviour
{
    private PlayerStates _states;
    private PlayerCarry _carry;
    private PlayerConfig _config;

    [SerializeField] TextMeshProUGUI _bagStorage;
    [SerializeField] TextMeshProUGUI _stamina;

    [Header("Timer")]
    [SerializeField] TimerCounter _timerCounter;
    [SerializeField] TextMeshProUGUI _timerText;

    [Header("EndShift")]
    [SerializeField] GameObject _EndShift;
    [SerializeField] TextMeshProUGUI _scoreTotalCollected;

    [Header("ItemInShift")]
    [SerializeField] TextMeshProUGUI _Items;
    [SerializeField] GarbageLocator _locator;

    [Inject]
    public void C(PlayerStates states, PlayerCarry carry, PlayerConfig config)
    {
        _states = states;
        _carry = carry;
        _config = config;

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

    void Update()
    {
        Stamina(); Timer(); 
    }
}
