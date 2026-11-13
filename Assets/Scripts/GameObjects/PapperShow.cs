using UnityEngine;
using UnityEngine.Localization;

public class PapperShow : MonoBehaviour, IInteract
{
    [SerializeField] LocalizedString _string;
    [SerializeField] Outline _outline;

    public LocalizedString Prompt => _string;

    public Outline Outline => _outline;

    public InteractResult Check(PlayerCarry carry, PlayerHand hand)
    {
        throw new System.NotImplementedException();
    }

    public InteractResult Interact(PlayerCarry carry, PlayerHand hand)
    {
        throw new System.NotImplementedException();
    }
}
