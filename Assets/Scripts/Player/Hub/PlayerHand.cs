
using UnityEngine;

public class PlayerHand 
{
    public readonly Transform Hand;
    public readonly Transform Bag;
    public bool Occupied => Hand.GetChild(0).gameObject.activeInHierarchy;

    public PlayerHand(PlayerConfig config)
    {
        Hand = config.Hand;
        Bag = config.Garbage;
    }

    public void HandOccupied()
    {
        if (Occupied)
        {
            GetBag(false);
        }
    } 
    public bool PickUpBag()
    {
        if (Occupied)return false;
        GetBag(true);
        return true;
    } 

    private void GetBag(bool t)
    {
        Transform bag = Hand.GetChild(0);
        bag.gameObject.SetActive(t);
    }  
}
