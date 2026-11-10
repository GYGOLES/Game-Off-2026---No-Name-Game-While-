using System;
using Assets.Scripts.Inputmanager;
using UnityEditor.Rendering;
using UnityEngine;
namespace Assets.Scripts.Player.Hub
{
    public class PlayerMovement : IDisposable
    {
        private readonly IPlayerInput _input;
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        private readonly PlayerCarry _carry;

        private Vector2 _move;
        private bool _sprint;
        private bool _losedStamina;
        private int _speedDecreased;
        private float _deceleration = 1;

        private float _currentSpeed;
        public PlayerMovement(IPlayerInput input, PlayerConfig config, PlayerStates states, PlayerCarry carry)
        {
            _input = input;
            _config = config;
            _states = states;
            _carry = carry;

            _input.Move += OnMove;
            _input.Sprint += OnSprint;
            _states.StaminaDepleted += LosedStamina;
            _carry.BagIntChange += SpeedDecreased;

            _speedDecreased = 0;
        }

        private void OnMove(Vector2 v) => _move = v;
        private void OnSprint(bool v) => _sprint = v;
        private void LosedStamina(bool v) => _losedStamina = v;
        private void SpeedDecreased(int t) => _speedDecreased = t;

        public void MoveTick(PlayerContext ctx , Transform body)
        {
            Vector3 forward = body.transform.TransformDirection(Vector3.forward);
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = body.transform.TransformDirection(Vector3.right);
            right.y = 0f;
            right.Normalize();

            bool sprinting = _sprint&&_move.sqrMagnitude > 0.0001f&&!_losedStamina;
            float baseSpeed = sprinting? _config.SprintSpeed  : _config.WalkSpeed ;
            float targetSpeed = _speedDecreased >= _config.MaxBagStorage? _config.MaxDeceleration : _speedDecreased >= _config.MaxBagStorage*0.5 ? _config.MiddleDeceleration : _speedDecreased >= _config.MaxBagStorage * 0.2? _config.MinDeleceleration : 1;


            _deceleration = Mathf.Lerp(_deceleration, targetSpeed, Time.deltaTime * 12);
            _currentSpeed = baseSpeed * _deceleration;
            Debug.Log(_deceleration);
           
            Vector3 wishDir = (_move.x*right + _move.y*forward)*_currentSpeed;
            ctx.velocity.x = wishDir.x;
            ctx.velocity.z = wishDir.z;
            ctx.currentInput = _move;
            ctx.IsSprinting = sprinting;
        }

        public void GravityTick(PlayerContext ctx, bool g, float dt)
        {
            if (g && ctx.velocity.y < 0f)
            {
                ctx.velocity.y = _config.StayGround;
            }
            else
            {
                ctx.velocity.y -= _config.Gravity*dt;
            }
        }
        public void Dispose()
        {
            if(_input == null)return;

            _input.Move -= OnMove;
            _input.Sprint -= OnSprint;
            _states.StaminaDepleted -= LosedStamina;
            _carry.BagIntChange -= SpeedDecreased;
        }
    }
}

