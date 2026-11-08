using System.Collections.Generic;
using UnityEngine;
public enum SurfaceType
{
    Grass, Dirt, Chainlink, 
    Duct, Gravel, Ladder, 
    Metal, MetalGrate, Mud, 
    Sand, Slosh, Tile, 
    Wade, Wood, WoodenPanel,
    Concrete
}
public enum PlayerActions
{
    Interact
}
[System.Serializable]
public class PlayerActionSounds
{
    [SerializeField] PlayerActions actions;
    [SerializeField] AudioScriptableObj audio;

    public PlayerActions Actions => actions;
    public AudioScriptableObj Audio => audio;
}
[System.Serializable]
public class SurfaceSounds
{
    [SerializeField] SurfaceType surfaceType;
    [SerializeField] AudioScriptableObj audio;

    public SurfaceType SurfaceType => surfaceType;
    public AudioScriptableObj Audio => audio;
}
[System.Serializable]
public class MaterialsEnum
{
    [SerializeField] SurfaceType surfaceType;
    [SerializeField] PhysicsMaterial physicsMaterial;

    public SurfaceType SurfaceType => surfaceType;
    public PhysicsMaterial PhysicsMaterial => physicsMaterial;
}
public class PlayerSounds : AudioAbs
{
    [SerializeField] SurfaceSounds[] surface;
    [SerializeField] MaterialsEnum[] materials;
    [SerializeField] PlayerActionSounds[] actionSounds;
    private Dictionary<PhysicsMaterial, SurfaceType> materialsDictionary;
    private Dictionary<SurfaceType, AudioScriptableObj> audioDictionary;
    private Dictionary<PlayerActions,  AudioScriptableObj> playerActionsDictionary;

    private float cooldown = 0;

    [SerializeField] float baseRate = 0.5f;
    [SerializeField] float magnitude = 0.17f;

    [SerializeField] float moveRate = 0.7f;
    [SerializeField] float runRate = 0.6f;

    private float GetStepRate(PlayerContext ctx)
    {
        if(ctx.IsSprinting) return runRate;
        return moveRate;
    }
    void Awake()
    {
        materialsDictionary = new Dictionary<PhysicsMaterial, SurfaceType>();
        audioDictionary = new Dictionary<SurfaceType, AudioScriptableObj>();
        playerActionsDictionary = new Dictionary<PlayerActions, AudioScriptableObj>();
        foreach (var m in materials)
        {
            materialsDictionary[m.PhysicsMaterial] = m.SurfaceType;
        }
        foreach (var s in surface)
        {
            audioDictionary[s.SurfaceType] = s.Audio;
        }
        foreach (var item in actionSounds)
        {
            playerActionsDictionary[item.Actions] = item.Audio;
        }
    }

    public void Tick(PlayerContext ctx)
    {
        if(ctx.currentInput.sqrMagnitude > magnitude)
        {
            Rate(ctx);
        }
    }
    private void Rate(PlayerContext ctx)
    {
        cooldown -= Time.deltaTime;
        if(cooldown <= 0)
        {
            PlayEnough(ctx.hit);
            float stepRate = baseRate;
            stepRate *= GetStepRate(ctx);
            cooldown = stepRate;
        }
    }
    private void PlayEnough(RaycastHit GroundHit)
    {
        var hits = GroundHit;
        if(hits.collider != null)
        {
            PhysicsMaterial material = hits.collider.sharedMaterial;
            if(material != null&&materialsDictionary.TryGetValue(material, out SurfaceType surface))
            {
                if(audioDictionary.TryGetValue(surface, out AudioScriptableObj audio))
                {
                    PlayFootStep(audio, hits.point);
                }
                else
                {
                    PlayFootStep(this._audio, hits.point);
                }
            }
            else
            {
                PlayFootStep(this._audio, hits.point);
            }
        }
    }
    private void PlayFootStep(AudioScriptableObj audio, Vector3 pos)
    {
        if(audio == null || audio.ArraClips.Length <= 0)return;
        AudioClip clip = GetRandomClip(audio.ArraClips);
        PlayAtPosition(clip, pos, audio.Volume);
    }
    public void PlayAction(PlayerActions actions)
    {
        if(playerActionsDictionary.TryGetValue(actions, out AudioScriptableObj audio))
        {
            if(audio == null|| audio.Clip == null)return;
            PlayAtPosition(audio.Clip, transform.position, audio.Volume);
        }
    }
}
