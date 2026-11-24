using System;
using UnityEngine;
using UnityEngine.Localization;

public sealed class PlayerCarry
{
    private readonly PlayerConfig _config;
    private readonly PlayerFear _fear;

    private int _currentBagCollected;


    public int CurrentBagCollected => _currentBagCollected;

    public event Action<int> BagIntChange;

    public event Action OnDrop;

    public event Action<LocalizedString, int> OnShowText;
    public event Action OnCollect;

    public event Action<int> OnFirstDropped;
    public event Action<int> OnFirstCollected;
    private bool _firstTimeCollected;
    private bool _firstTimeDropped;

    public PlayerCarry(PlayerConfig config, PlayerFear fear)
    {
        _config = config;
        _fear = fear;
    }

    public InteractResult Check(ItemWeight weight)
    {
        if(_currentBagCollected >= _config.MaxBagStorage)return InteractResult.FullBag;
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

        _fear.DrainFear(weight.Weight);

        _currentBagCollected ++;

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
        if(_fear == null)
        {
            Debug.Log("Fear");
            return;
        }
        _fear.TriggerJumpscare(i.Jumpscare);
        _fear.GetFear(i.Fear);
        OnShowText?.Invoke(i.Text, i.Index);
        OnCollect?.Invoke();
    }
}
