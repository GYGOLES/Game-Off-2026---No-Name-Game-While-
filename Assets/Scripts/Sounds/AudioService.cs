using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
public interface IAudioService
{
    public void PlaySound(AudioClip clip, Vector3 position, float volume = 0.2f, float pitch = 1);
}
public class AudioService : IAudioService, IStartable, IDisposable
{
    readonly Queue<AudioSource> freeSources = new ();
    readonly List<AudioSource> allsources = new ();
    readonly CancellationTokenSource ctx = new ();
    readonly IObjectResolver resolver;

    readonly AudioPool pool;
    private Transform root;
    public AudioService(AudioPool pool, IObjectResolver resolver)
    {
        this.resolver = resolver;
        this.pool = pool;
    }
    public void Dispose()
    {
        ctx.Cancel();
        ctx.Dispose();
        if (root) UnityEngine.Object.Destroy(root.gameObject);
    }

    public void PlaySound(AudioClip clip, Vector3 position, float volume = 0.2F, float pitch = 1)
    {
        if(clip == null||freeSources.Count == 0)return;

        var source = freeSources.Dequeue();
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.gameObject.SetActive(true);
        source.Play();
        RealeaseAfter(source, clip.length / pitch, ctx.Token).Forget();
    }

    public void Start()
    {
        root = new GameObject("AudioPool").transform;
        
        for (int i = 0; i < pool.PoolSize; i++)
        {
            var src = this.resolver.Instantiate(pool.Prefab, root);
            src.gameObject.SetActive(false);
            allsources.Add(src);
            freeSources.Enqueue(src);
        }
    }

    async UniTaskVoid RealeaseAfter(AudioSource source, float delay, CancellationToken token)
    {
        await UniTask.WaitForSeconds(delay, cancellationToken: token);
        if(token.IsCancellationRequested)return;
        source.gameObject.SetActive(false);
        freeSources.Enqueue(source);
    }
}
