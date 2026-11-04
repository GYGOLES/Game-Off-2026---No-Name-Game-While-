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

        [Inject]
        public void Construct(PlayerCamera camera, PlayerMovement movement)
        {
            _pl = camera;
            _movement = movement;
        }

        void Update()
        {
            _pl.Tick(_controller.transform, _orientation);
            _movement.Tick(_controller, Time.deltaTime);
        }
    }
}
