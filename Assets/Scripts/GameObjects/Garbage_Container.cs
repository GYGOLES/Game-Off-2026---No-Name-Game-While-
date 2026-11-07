using UnityEngine;

public class Garbage_Container : AudioAbs, IInteract
{
    public void Interact(PlayerCarry carry, PlayerHand hand)
    {
        if (carry.FullDrop())
        {
            hand.HandOccupied();
            PlaySound();
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
