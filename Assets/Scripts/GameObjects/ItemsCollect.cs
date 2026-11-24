
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
    private Vector3 _startLocalScale;

    private int _layerDefault;
    private int _layerTarget;
    void Awake()
    {
        _collider = GetComponent<Collider>();
        _startPosition = gameObject.transform.localPosition;
        _startLocalScale = transform.localScale;
        _layerDefault = gameObject.layer;
        _layerTarget = LayerMask.NameToLayer("garbage_bag");
    }

    void OnEnable()
    {
        gameObject.transform.localPosition = _startPosition;
        transform.localScale = _startLocalScale;
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
        if(carry.Check(_item) != InteractResult.Success)return InteractResult.FullBag;
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
        AnimObject(hand, carry);

        Collected?.Invoke(this);
        return InteractResult.Success;
    }

    void AnimObject(PlayerHand h, PlayerCarry c)
    {
        _collider.enabled = false;
        gameObject.layer = _layerTarget;
        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;
        _CollectAnimation = Tween.Custom(0f, 1f, duration: 0.6f, onValueChange: t =>
        {
            Vector3 pos = Vector3.Lerp(startPos, h.Bag.position + Vector3.up*0.4f, t);
            Vector3 scale = Vector3.Lerp(startScale, new Vector3(0.1f, 0.1f, 0.1f), t);
            pos.y += Mathf.Sin(t * Mathf.PI) * 0.5f;

            transform.position = pos;
            transform.localScale = scale;
        }).OnComplete(() => 
        {
            gameObject.layer = _layerDefault;
            gameObject.SetActive(false); 
            _collider.enabled = true;
            h.SetFullBag(c.MaxCollected);
        });
    }
}
