using UnityEngine.Localization;
public enum InteractResult
{
    None ,Success, NoBag, FullBag, EmptyBag, InHand, Failed
}
public interface IInteract 
{
    public LocalizedString Prompt{get;}
    public Outline Outline{get;}
    public InteractResult Interact(PlayerCarry carry, PlayerHand hand);
    public InteractResult Check(PlayerCarry carry, PlayerHand hand);
}
