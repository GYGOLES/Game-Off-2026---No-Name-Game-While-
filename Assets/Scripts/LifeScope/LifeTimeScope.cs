using Assets.Scripts.Inputmanager;
using Assets.Scripts.Player.Hub;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LifeTimeScope : LifetimeScope
{
    [SerializeField] PlayerConfig _playerConfig;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<InputHub>().As<IInputModeSwitch>().AsSelf();
        builder.RegisterEntryPoint<PlayerInput>().As<IPlayerInput>();

        _playerConfig.Load();
        builder.RegisterInstance(_playerConfig);

        builder.Register<PlayerCamera>(Lifetime.Singleton);
        builder.Register<PlayerMovement>(Lifetime.Singleton);
        
        builder.RegisterComponentInHierarchy<PlayerHub>();
    }
}
