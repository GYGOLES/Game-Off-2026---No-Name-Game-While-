using System;
using UnityEngine;

public sealed class PlayerCarry
{
    private readonly PlayerConfig _config;

    private float _currentWeight;
    private int _currentBagCollected;

    private int _totalBagCollected;
    private float _totalWeight;

    public float CurrentWeight => _currentWeight;
    public int CurrentBagCollected => _currentBagCollected;

    public event Action<float> WeightChanged;
    public event Action<int> BagIntChange;

    public event Action<int> TotalBagChange;
    public event Action<float> TotalWeightChange;

    public PlayerCarry(PlayerConfig config) => _config = config;

    public bool TryAdd(ItemWeight weight)
    {
        if(_currentWeight + weight.Weight > _config.MaxWeight || _currentBagCollected >= _config.MaxBagStorage)return false;

        _currentWeight += weight.Weight;
        _currentBagCollected ++;

        _totalWeight += weight.Weight;
        _totalBagCollected ++;

        TotalWeightChange?.Invoke(_totalWeight);
        TotalBagChange?.Invoke(_totalBagCollected);

        WeightChanged?.Invoke(_currentWeight);
        BagIntChange?.Invoke(_currentBagCollected);

        return true;
    }

    public void Remove(ItemWeight item)
    {
        _currentWeight = Mathf.Max(0f, _currentWeight - item.Weight);
        _currentBagCollected --;
        WeightChanged?.Invoke(_currentWeight);
        BagIntChange?.Invoke(_currentBagCollected);
    }   

    public bool FullDrop()
    {
        if(_currentBagCollected <= 0)return false;

        _currentBagCollected = 0;
        _currentWeight = 0f;
        WeightChanged?.Invoke(_currentWeight);
        BagIntChange?.Invoke(_currentBagCollected);
        
        return true;
    }
}
