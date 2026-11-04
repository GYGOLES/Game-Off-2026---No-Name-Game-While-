using UnityEngine;
using VContainer;
namespace Assets.Scripts.Player.Hub
{    
    public class PlayerHub : MonoBehaviour
    {
        [SerializeField] CharacterController _controller;
        [SerializeField] Transform _orientation;

        private PlayerCamera _pl;
        private PlayerMovement _movement;

        PlayerContext ctx = new PlayerContext();

        [Inject]
        public void Construct(PlayerCamera camera, PlayerMovement movement)
        {
            _pl = camera;
            _movement = movement;
        }

        void Update()
        {
            _pl.Tick(_controller.transform, _orientation);

        
            _movement.GravityTick(ctx, _controller.isGrounded);
            _movement.MoveTick(ctx, _controller.transform);

            _controller.Move(ctx.velocity*Time.deltaTime*5);
        }
    }
}
