using UnityEngine;
using UnityEngine.Localization;

public class Garbage_Container : AudioAbs, IInteract
{
    [SerializeField] LocalizedString _string;
    [SerializeField] Outline _outline;

    public LocalizedString Prompt => _string;

    public Outline Outline => _outline;

    public InteractResult Check(PlayerCarry carry, PlayerHand hand)
    {
        if(carry.CurrentBagCollected <= 0)return InteractResult.EmptyBag;
        return InteractResult.Success;
    }

    public InteractResult Interact(PlayerCarry carry, PlayerHand hand)
    {
        InteractResult r = Check(carry, hand);
        Debug.Log($"{r} Garbage_Container");
        if(r != InteractResult.Success)return r;

        carry.FullDrop();
        hand.HandOccupied();
        PlayDefaultClips();
        return InteractResult.Success;
    }
}
