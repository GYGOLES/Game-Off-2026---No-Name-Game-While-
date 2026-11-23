
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MenuLifeTimeScope : LifetimeScope
{
    [SerializeField] PlayerConfig _config;
    protected override void Configure(IContainerBuilder builder)
    {
        _config.Load();
        builder.RegisterInstance(_config);
    }
}
