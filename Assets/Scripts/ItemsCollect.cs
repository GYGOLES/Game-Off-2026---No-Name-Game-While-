
using System;
using UnityEngine;
using UnityEngine.Localization;
public class ItemsCollect : AudioAbs, IInteract
{
    [SerializeField] ItemWeight _item;
    [SerializeField] LocalizedString _string;

    public LocalizedString String => _string;

    public event Action<ItemsCollect> Collected;
    public void Interact(PlayerCarry carry, PlayerHand hand)
    {
        if(_item != null)
        {
            if (hand.Occupied)
            {    
                if (carry.TryAdd(_item))
                {
                    PlayDefaultClips();
                    gameObject.SetActive(false);

                    Collected?.Invoke(this);
                    enabled = false;
                }
                else
                {
                    Debug.LogWarning("Full bag");
                }
            }
            else
            {
                Debug.Log("There's nothing in hands");
            }
            
        }
        else
        {
            Debug.LogWarning("ITEM NULL");
        }
    }

    
}
