using UnityEngine;
using UnityEngine.Localization;

public class Garbage_Bag : AudioAbs, IInteract
{
    [SerializeField] LocalizedString _string;

    public LocalizedString String => _string;
    public void Interact(PlayerCarry g, PlayerHand hand)
    {
        if (hand.PickUpBag())
        {
            PlayDefaultClips();
            gameObject.SetActive(false);
        }
    }
}
