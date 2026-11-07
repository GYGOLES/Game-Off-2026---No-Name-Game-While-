
using UnityEngine;
public class ItemsCollect : AudioAbs, IInteract
{
    [SerializeField] ItemWeight _item;
    public void Interact(PlayerCarry carry, PlayerHand hand)
    {
        if(_item != null)
        {
            if (carry.TryAdd(_item)&&hand.Occupied)
            {
                PlaySound();
                gameObject.SetActive(false);
                enabled = true;
            }
            else
            {
                Debug.LogError("Full Bag");
            }
        }
        else
        {
            Debug.LogWarning("ITEM NULL");
        }

        
    }

    void PlaySound()
    {
        if(_audio != null&&_audio.ArraClips.Length > 0)
        {
            PlayAtPosition(GetRandomClip(_audio.ArraClips), transform.position);
        }
        else
        {
            Debug.LogError("NO CLIP IN OBJECT");
        }
    }
}
