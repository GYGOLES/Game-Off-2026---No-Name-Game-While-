
using System;
using Assets.Scripts.Inputmanager;
using PrimeTween;
using UnityEngine;
using UnityEngine.Localization;
public class ItemsCollect : AudioAbs, IInteract
{
    [SerializeField] Collider _collider;
    [SerializeField] ItemWeight _item;
    [SerializeField] LocalizedString _string;

    [SerializeField] Outline _outline;
    private Tween _CollectAnimation;

    public LocalizedString Prompt => _string;
    public InputPhase Phase => InputPhase.started;

    public Outline Outline => _outline;

    public event Action<ItemsCollect> Collected;

    private Vector3 _startPosition;

    private int _layerDefault;
    private int _layerTarget;
    void Awake()
    {
        _collider = GetComponent<Collider>();
        _startPosition = gameObject.transform.localPosition;
        _layerDefault = gameObject.layer;
        _layerTarget = LayerMask.NameToLayer("garbage_bag");
    }

    void OnEnable()
    {
        gameObject.transform.localPosition = _startPosition;
        gameObject.layer = _layerDefault;
    }
    void OnDisable()
    {
        _CollectAnimation.Stop();
    }

    public InteractResult Check(PlayerCarry carry, PlayerHand hand)
    {
        if(_item == null)return InteractResult.Failed;
        if(!hand.Occupied)return InteractResult.NoBag;
        if(carry.Check(_item) != InteractResult.Success) return InteractResult.FullBag;
        return InteractResult.Success;
    }

    public InteractResult Interact(PlayerCarry carry, PlayerHand hand)
    {
        InteractResult r = Check(carry, hand);
        Debug.Log($"{r} items");
        if(r != InteractResult.Success)return r;

        carry.TryAdd(_item); 
        PlayDefaultClips();
        
        hand.AnimateBag();
        AnimObject(hand.Bag);

        Collected?.Invoke(this);
        return InteractResult.Success;
    }

    void AnimObject(Transform bag)
    {
        _collider.enabled = false;
        gameObject.layer = _layerTarget;
        Vector3 startPos = transform.position;
        _CollectAnimation = Tween.Custom(0f, 1f, duration: 0.6f, onValueChange: t =>
        {
            Vector3 pos = Vector3.Lerp(startPos, bag.position + Vector3.up*0.4f, t);
            pos.y = Mathf.Sin(t * Mathf.PI);
            transform.position = pos;
        }).OnComplete(() => 
        {
            gameObject.layer = _layerDefault;
            gameObject.SetActive(false); 
            _collider.enabled = true;
        });
    }
}
