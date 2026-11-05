using UnityEngine;

public class Test : AudioAbs, IInteract
{
    public void Interact(GameObject gameObject)
    {
        Debug.Log("INTERACT");
        PlayAtPosition(GetRandomClip(_audio.ArraClips), transform.position);
    }
}
