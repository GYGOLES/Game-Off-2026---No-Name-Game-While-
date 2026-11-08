using UnityEngine;

public class Garbage_Bag : AudioAbs, IInteract
{
    public void Interact(PlayerCarry g, PlayerHand hand)
    {
        if (hand.PickUpBag())
        {
            PlayDefaultClips();
            gameObject.SetActive(false);
        }
    }
}
