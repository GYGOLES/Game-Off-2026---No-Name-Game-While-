
using UnityEngine;

public class PlayerHand 
{
    public readonly Transform _hand;
    public bool Occupied => _hand.GetChild(0).gameObject.activeInHierarchy;

    public PlayerHand(PlayerConfig config) => _hand = config.Hand;

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
        Transform bag = _hand.GetChild(0);
        bag.gameObject.SetActive(t);
    }  
}
