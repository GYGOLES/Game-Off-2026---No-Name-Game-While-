using UnityEngine;
using UnityEngine.Localization;

public class PapperShow : MonoBehaviour, IInteract
{
    [SerializeField] LocalizedString _string;
    [SerializeField] Outline _outline;

    public LocalizedString Prompt => _string;

    public Outline Outline => _outline;

    public void Interact(PlayerCarry gameObject, PlayerHand hand)
    {
        
    }
}
