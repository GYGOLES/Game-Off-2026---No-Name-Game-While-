using UnityEngine;
using UnityEngine.Localization;

public interface IInteract 
{
    public LocalizedString Prompt{get;}
    public Outline Outline{get;}
    public void Interact(PlayerCarry gameObject, PlayerHand hand);
}
