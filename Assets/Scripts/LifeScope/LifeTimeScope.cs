using Assets.Scripts.Inputmanager;
using VContainer;
using VContainer.Unity;

public class LifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<InputHub>().As<IInputModeSwitch>().AsSelf();
        builder.RegisterEntryPoint<PlayerInput>().As<IPlayerInput>();
    }
}
