using UnityEngine;
using UnityEngine.Localization;

public interface IInteract 
{
    public LocalizedString String{get;}
    public void Interact(PlayerCarry gameObject, PlayerHand hand);
}
