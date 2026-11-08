using UnityEngine;

public class Garbage_Container : AudioAbs, IInteract
{
    public void Interact(PlayerCarry carry, PlayerHand hand)
    {
        if (carry.FullDrop())
        {
            hand.HandOccupied();
            PlayDefaultClips();
        }
    }
}
