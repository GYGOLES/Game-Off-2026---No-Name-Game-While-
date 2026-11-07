using UnityEngine;

public class ItemsCollect : AudioAbs, IInteract
{
    public void Interact(GameObject gameObject)
    {
        PlaySound();

        
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
