using UnityEngine;
using UnityEngine.Localization;

public class Garbage_Container : AudioAbs, IInteract
{
    [SerializeField] LocalizedString _string;
    [SerializeField] Outline _outline;

    public LocalizedString Prompt => _string;

    public Outline Outline => _outline;

    public void Interact(PlayerCarry carry, PlayerHand hand)
    {
        if (carry.FullDrop())
        {
            hand.HandOccupied();
            PlayDefaultClips();
        }
    }
}
