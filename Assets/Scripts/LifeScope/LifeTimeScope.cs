using Assets.Scripts.Inputmanager;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<InputServices>(Lifetime.Singleton).As<IInputService>().As<IInitializable>();
    }
}
