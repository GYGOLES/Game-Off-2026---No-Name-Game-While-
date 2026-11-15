using System;
using UnityEngine;
using UnityEngine.Localization;

public sealed class PlayerCarry
{
    private readonly PlayerConfig _config;

    private float _currentWeight;
    private int _currentBagCollected;

    private float _totalWeight;

    public float CurrentWeight => _currentWeight;
    public int CurrentBagCollected => _currentBagCollected;

    public event Action<float> WeightChanged;
    public event Action<int> BagIntChange;

    public event Action OnDrop;

    public event Action<float> TotalWeightChange;
    public event Action<LocalizedString, Sprite> OnShowText;
    public event Action OnCollect;

    public event Action<int> OnFirstDropped;
    public event Action<int> OnFirstCollected;
    private bool _firstTimeCollected;
    private bool _firstTimeDropped;

    public PlayerCarry(PlayerConfig config) => _config = config;

    public InteractResult Check(ItemWeight weight)
    {
        if(_currentWeight + weight.Weight > _config.MaxWeight || _currentBagCollected >= _config.MaxBagStorage)return InteractResult.FullBag;
        return InteractResult.Success;
    }
    public InteractResult TryAdd(ItemWeight weight)
    {
        InteractResult result = Check(weight);
        if(result != InteractResult.Success)return result;

        if (!_firstTimeCollected)
        {
            OnFirstCollected?.Invoke(1);
            _firstTimeCollected = true;
        }

        _currentWeight += weight.Weight;
        _currentBagCollected ++;

        _totalWeight += weight.Weight;

        TotalWeightChange?.Invoke(_totalWeight);

        WeightChanged?.Invoke(_currentWeight);
        BagIntChange?.Invoke(_currentBagCollected);

        return InteractResult.Success;
    } 
    public bool FullDrop()
    {
        if(_currentBagCollected <= 0)return false;

        if (!_firstTimeDropped)
        {
            OnFirstDropped?.Invoke(2);
            _firstTimeDropped = true;
        }

        _currentBagCollected = 0;
        _currentWeight = 0f;
        WeightChanged?.Invoke(_currentWeight);
        BagIntChange?.Invoke(_currentBagCollected);
        OnDrop?.Invoke();
        
        return true;
    }

    public void Show(PaperInfo i)
    {
        if(i == null)
        {
            Debug.Log("Paper null");
            return;
        }

        OnShowText?.Invoke(i.Text, i.Sprite);
        OnCollect?.Invoke();
    }
}
