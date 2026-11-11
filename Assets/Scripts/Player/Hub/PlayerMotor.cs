using UnityEngine;
namespace Assets.Scripts.Player.Hub
{    
    public sealed class PlayerMotor 
    {

        private readonly PlayerCamera _camera;
        private readonly PlayerMovement _movement;
        private readonly HeadBobingEffect _effect;
        private readonly PlayerHit _hit;
        private readonly PlayerStates _states;
        private readonly PlayerSounds _sounds;

        private readonly CharacterController _controller;
        private readonly Transform _orientation;
        public PlayerMotor(PlayerCamera camera, PlayerMovement movement, HeadBobingEffect ef, PlayerSounds sounds, PlayerHit hit, PlayerInteract interact, PlayerConfig config, PlayerStates states)
        {
            _camera = camera;
            _movement = movement;
            _effect = ef;
            _hit = hit;
            _sounds = sounds;
            _states = states;

            _orientation = config.Orientation;
            _controller = config.Controller;
        }
        public void Tick(PlayerContext ctx, float dt)
        {
            _camera.Tick(_controller.transform, _orientation);
            _hit.Tick(ctx);
        
            _movement.GravityTick(ctx, _controller.isGrounded, dt);
            _movement.MoveTick(ctx, _controller.transform);

            _states.Tick(ctx, dt);

            _controller.Move(ctx.velocity*dt);
            _sounds.Tick(ctx);
        }
        public void LateTick(PlayerContext ctx, float dt)
        {
            _effect.Tick(ctx, dt);
        }

        public void Teleport(Transform targetPosition)
        {
            _controller.enabled = false;
            _movement.TeleportInPosition(_controller.transform, targetPosition);
            _controller.enabled = true;
        }
    }
}
