using UnityEngine;
using VContainer;
namespace Assets.Scripts.Player.Hub
{    
    public class PlayerHub : MonoBehaviour
    {
        private readonly PlayerContext _ctx = new PlayerContext();

        private PlayerMotor _motor;

        [Inject]
        public void Construct(PlayerMotor motor)
        {
            _motor = motor;
        }

        void Update()
        {
            _motor.Tick(_ctx, Time.deltaTime);
        }
        void LateUpdate()
        {
            _motor.LateTick(_ctx, Time.deltaTime);
        }
    }
}
