
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MenuLifeTimeScope : LifetimeScope
{
    [SerializeField] MenuConfig _configMenu;
    [SerializeField] BackgroundSounds _backgrounds;
    [SerializeField] PlayerConfig _config;
    protected override void Configure(IContainerBuilder builder)
    {
        _config.Load();
        builder.RegisterInstance(_config);
        builder.RegisterInstance(_configMenu);
        builder.RegisterInstance(_backgrounds);
        _backgrounds.SetIgnore();
    }
}
