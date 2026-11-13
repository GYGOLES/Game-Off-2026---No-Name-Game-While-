using System;
using UnityEngine;

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

        _currentBagCollected = 0;
        _currentWeight = 0f;
        WeightChanged?.Invoke(_currentWeight);
        BagIntChange?.Invoke(_currentBagCollected);
        OnDrop?.Invoke();
        
        return true;
    }
}
