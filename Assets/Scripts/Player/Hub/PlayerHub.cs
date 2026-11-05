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

        PlayerContext ctx = new PlayerContext();

        [Inject]
        public void Construct(PlayerCamera camera, PlayerMovement movement, HeadBobingEffect ef)
        {
            _pl = camera;
            _movement = movement;
            _effect = ef;
        }

        void Update()
        {
            _pl.Tick(_controller.transform, _orientation);

        
            _movement.GravityTick(ctx, _controller.isGrounded, Time.deltaTime);
            _movement.MoveTick(ctx, _controller.transform);

            _controller.Move(ctx.velocity*Time.deltaTime);
        }
        void LateUpdate()
        {
            _effect.Tick(ctx, Time.deltaTime);
        }
    }
}
