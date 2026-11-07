using System;
using UnityEngine;

public sealed class PlayerCarry
{
    private readonly PlayerConfig _config;

    private float _totalWeight;
    private int _BagCollection;

    public float TotalWeight => _totalWeight;
    public float BagCollection => _BagCollection;

    public event Action<float> WeightChanged;
    public event Action<int> BagIntChange;

    public PlayerCarry(PlayerConfig config) => _config = config;
    public bool TryAdd(ItemWeight weight)
    {
        if(_totalWeight + weight.Weight > _config.MaxWeight || _BagCollection >= _config.MaxBagStorage)return false;

        _totalWeight += weight.Weight;
        _BagCollection ++;

        WeightChanged?.Invoke(_totalWeight);
        BagIntChange?.Invoke(_BagCollection);

        return true;
    }

    public void Remove(ItemWeight item)
    {
        _totalWeight = Mathf.Max(0f, _totalWeight - item.Weight);
        _BagCollection --;
        WeightChanged?.Invoke(_totalWeight);
        BagIntChange?.Invoke(_BagCollection);
    }   

    public void FullDrop(ItemWeight item)
    {
        if(_BagCollection > 0)
        {
            _BagCollection = 0;
            _totalWeight = 0f;
            WeightChanged?.Invoke(_totalWeight);
            BagIntChange?.Invoke(_BagCollection);
            item = null;
        }
    }
}
