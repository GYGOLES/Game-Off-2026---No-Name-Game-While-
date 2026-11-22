
using System;
using PrimeTween;
using UnityEngine;

public class PlayerHand 
{
    public readonly Transform Hand;
    public readonly Transform Bag;
    public bool Occupied => Hand.GetChild(0).gameObject.activeInHierarchy;
    public Vector3 StartBagPosition => _startBagPosition;

    public event Action<int> OnFirstTimeCollected;
    bool _collectFirstTime;

    private Vector3 _startBagPosition;
    private Tween _bagAnim;
    public PlayerHand(PlayerConfig config)
    {
        Hand = config.Hand;
        Bag = config.Garbage;
        _startBagPosition = Bag.localPosition;
    }

    public void DropBag()
    {
        if (Occupied)
        {
            GetBag(false);
            _bagAnim.Stop();
        }
    } 
    public bool PickUpBag()
    {
        if (Occupied)return false;
        if (!_collectFirstTime)
        {
            OnFirstTimeCollected?.Invoke(0);
            _collectFirstTime = true;
        }

        GetBag(true);
        return true;
    } 

    private void GetBag(bool t)
    {
        Transform bag = Hand.GetChild(0);
        bag.gameObject.SetActive(t);
        bag.localPosition = _startBagPosition;
    }

    public void AnimateBag()
    {
        _bagAnim = Tween.ShakeLocalPosition(Bag, Vector3.down*0.05f, 1, frequency: 2f);
    }
}
