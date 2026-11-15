using Assets.Scripts.Inputmanager;
using UnityEngine;
using UnityEngine.Localization;

public class PapperShow : AudioAbs, IInteract
{
    [SerializeField] LocalizedString _string;
    [SerializeField] Outline _outline;
    [SerializeField] PaperInfo _info;

    public LocalizedString Prompt => _string;
    
    public Outline Outline => _outline;

    public InputPhase Phase =>  InputPhase.started;

    public InteractResult Check(PlayerCarry carry, PlayerHand hand)
    {
        if(_info == null)return InteractResult.Failed;
        return InteractResult.Success;
    }

    public InteractResult Interact(PlayerCarry carry, PlayerHand hand)
    {
        var a = Check(carry, hand);
        Debug.Log($"{a} paper");
        if(a != InteractResult.Success)return a;

        PlayDefaultClips();
        carry.Show(_info);
        gameObject.SetActive(false);
        return InteractResult.Success;
    }
}
