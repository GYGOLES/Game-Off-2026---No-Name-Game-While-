using UnityEngine;
using UnityEngine.Localization;

public class Garbage_Container : AudioAbs, IInteract
{
    [SerializeField] LocalizedString _string;

    public LocalizedString String => _string;
    public void Interact(PlayerCarry carry, PlayerHand hand)
    {
        if (carry.FullDrop())
        {
            hand.HandOccupied();
            PlayDefaultClips();
        }
    }
}
