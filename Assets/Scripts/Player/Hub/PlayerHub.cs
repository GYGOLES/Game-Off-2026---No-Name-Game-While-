using UnityEngine;
using VContainer;
namespace Assets.Scripts.Player.Hub
{    
    public class PlayerHub : MonoBehaviour
    {
        [SerializeField] CharacterController _controller;
        [SerializeField] Transform _orientation;
        [SerializeField] Transform _headBobing;

        private PlayerCamera _pl;
        private PlayerMovement _movement;
        private HeadBobingEffect _effect;
        private PlayerHit _hit;

        private PlayerSounds _sounds;

        PlayerContext ctx = new PlayerContext();

        [Inject]
        public void Construct(PlayerCamera camera, PlayerMovement movement, HeadBobingEffect ef, PlayerSounds sounds, PlayerHit hit)
        {
            _pl = camera;
            _movement = movement;
            _effect = ef;
            _hit = hit;
            _sounds = sounds;
            
        }

        void Update()
        {
            _pl.Tick(_controller.transform, _orientation);
            _hit.Tick(ctx);
        
            _movement.GravityTick(ctx, _controller.isGrounded, Time.deltaTime);
            _movement.MoveTick(ctx, _controller.transform);

            _controller.Move(ctx.velocity*Time.deltaTime);
            _sounds.Tick(ctx);
        }
        void LateUpdate()
        {
            _effect.Tick(ctx, Time.deltaTime);
        }
    }
}
