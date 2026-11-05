using Assets.Scripts.Inputmanager;
using Assets.Scripts.Player.Hub;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LifeTimeScope : LifetimeScope
{
    [SerializeField] AudioPool _pool;

    [SerializeField] PlayerConfig _playerConfig;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<InputHub>().As<IInputModeSwitch>().AsSelf();
        builder.RegisterEntryPoint<PlayerInput>().As<IPlayerInput>();
        builder.RegisterEntryPoint<AudioService>().As<IAudioService>().As<IStartable>();

        _playerConfig.Load();
        builder.RegisterInstance(_playerConfig);
        builder.RegisterInstance(_pool);

        builder.Register<PlayerCamera>(Lifetime.Singleton);
        builder.Register<PlayerMovement>(Lifetime.Singleton);
        builder.Register<PlayerInteract>(Lifetime.Singleton);

        builder.Register<PlayerHit>(Lifetime.Singleton);
        builder.Register<PlayerMotor>(Lifetime.Singleton);

        builder.RegisterComponentInHierarchy<HeadBobingEffect>();
        builder.RegisterComponentInHierarchy<PlayerSounds>();
        builder.RegisterComponentInHierarchy<PlayerHub>();
    }
}
