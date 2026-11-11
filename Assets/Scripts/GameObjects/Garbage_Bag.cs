using UnityEngine;
using UnityEngine.Localization;

public class Garbage_Bag : AudioAbs, IInteract
{
    [SerializeField] LocalizedString _string;
    [SerializeField] Outline _outline;

    public LocalizedString Prompt => _string;

    public Outline Outline => _outline;

    public void Interact(PlayerCarry g, PlayerHand hand)
    {
        if (hand.PickUpBag())
        {
            PlayDefaultClips();
            gameObject.SetActive(false);
        }
    }
}
