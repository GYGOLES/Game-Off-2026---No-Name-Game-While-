using UnityEngine;
using UnityEngine.Localization;

public class PapperShow : MonoBehaviour, IInteract
{
    [SerializeField] LocalizedString _string;

    public LocalizedString String => _string;
    public void Interact(PlayerCarry gameObject, PlayerHand hand)
    {
        
    }
}
