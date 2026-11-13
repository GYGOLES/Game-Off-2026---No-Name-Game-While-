using Assets.Scripts.Inputmanager;
using UnityEngine;
using UnityEngine.Localization;

public class Garbage_Bag : AudioAbs, IInteract
{
    [SerializeField] LocalizedString _string;
    [SerializeField] Outline _outline;

    public LocalizedString Prompt => _string;

    public Outline Outline => _outline;

    public InputPhase Phase => InputPhase.started;

    public InteractResult Check(PlayerCarry carry, PlayerHand hand)
    {
        if(hand.Occupied)return InteractResult.InHand;
        return InteractResult.Success;
    }

    public InteractResult Interact(PlayerCarry g, PlayerHand hand)
    {
        InteractResult r = Check(g, hand);
        Debug.Log($"{r} Garbage_Bag");
        if(r != InteractResult.Success)return r;

        hand.PickUpBag();
        PlayDefaultClips();
        return InteractResult.Success; 
    }
}
