
using System;
using UnityEngine;
using UnityEngine.Localization;
public class ItemsCollect : AudioAbs, IInteract
{
    [SerializeField] ItemWeight _item;
    [SerializeField] LocalizedString _string;

    [SerializeField] Outline _outline;

    public LocalizedString Prompt => _string;

    public Outline Outline => _outline;

    public event Action<ItemsCollect> Collected;

    public InteractResult Check(PlayerCarry carry, PlayerHand hand)
    {
        if(_item == null)return InteractResult.Failed;
        if(!hand.Occupied)return InteractResult.NoBag;
        if(carry.Check(_item) != InteractResult.Success) return InteractResult.FullBag;
        return InteractResult.Success;
    }

    public InteractResult Interact(PlayerCarry carry, PlayerHand hand)
    {
        InteractResult r = Check(carry, hand);
        Debug.Log($"{r} items");
        if(r != InteractResult.Success)return r;

        carry.TryAdd(_item); 
        PlayDefaultClips();
        gameObject.SetActive(false);
        Collected?.Invoke(this);
        enabled = false;
        return InteractResult.Success;
    }
}
