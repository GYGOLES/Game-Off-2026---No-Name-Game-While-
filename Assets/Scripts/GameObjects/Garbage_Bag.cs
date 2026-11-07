using UnityEngine;

public class Garbage_Bag : AudioAbs, IInteract
{
    public void Interact(PlayerCarry g, PlayerHand hand)
    {
        if (hand.PickUpBag())
        {
            PlaySound();
            gameObject.SetActive(false);
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
