using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
namespace Assets.Scripts.Player.Hub
{    
    public sealed class PlayerMotor 
    {

        private readonly PlayerCamera _camera;
        private readonly PlayerMovement _movement;
        private readonly HeadBobingEffect _effect;
        private readonly PlayerHit _hit;
        private readonly PlayerSounds _sounds;

        private readonly CharacterController _controller;
        private readonly Transform _orientation;
        public PlayerMotor(PlayerCamera camera, PlayerMovement movement, HeadBobingEffect ef, PlayerSounds sounds, PlayerHit hit, PlayerInteract interact, PlayerConfig config)
        {
            _camera = camera;
            _movement = movement;
            _effect = ef;
            _hit = hit;
            _sounds = sounds;

            _orientation = config.Orientation;
            _controller = config.Controller;
        }
        public void Tick(PlayerContext ctx, float dt)
        {
            _camera.Tick(_controller.transform, _orientation);
            _hit.Tick(ctx);
        
            _movement.GravityTick(ctx, _controller.isGrounded, dt);
            _movement.MoveTick(ctx, _controller.transform);

            _controller.Move(ctx.velocity*dt);
            _sounds.Tick(ctx);
        }
        public void LateTick(PlayerContext ctx, float dt)
        {
            _effect.Tick(ctx, dt);
        }
    }
}
