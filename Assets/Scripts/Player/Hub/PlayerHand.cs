
using System;
using UnityEngine;

public class PlayerHand 
{
    public readonly Transform Hand;
    public readonly Transform Bag;
    public bool Occupied => Hand.GetChild(0).gameObject.activeInHierarchy;

    public event Action<int> OnFirstTimeCollected;
    bool _collectFirstTime;
    public PlayerHand(PlayerConfig config)
    {
        Hand = config.Hand;
        Bag = config.Garbage;
    }

    public void DropBag()
    {
        if (Occupied)
        {
            GetBag(false);
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
    }  
}
