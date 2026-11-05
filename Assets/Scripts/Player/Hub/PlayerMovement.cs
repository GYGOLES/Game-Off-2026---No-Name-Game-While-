using System;
using Assets.Scripts.Inputmanager;
using UnityEngine;
namespace Assets.Scripts.Player.Hub
{
    public class PlayerMovement : IDisposable
    {
        private readonly IPlayerInput _input;
        private readonly PlayerConfig _config;

        private Vector2 _move;
        private bool _sprint;

        private float _currentSpeed => _sprint ? _config.SprintSpeed : _config.WalkSpeed;
        public PlayerMovement(IPlayerInput input, PlayerConfig config)
        {
            _input = input;
            _config = config;
            _input.Move += OnMove;
            _input.Sprint += OnSprint;
        }

        private void OnMove(Vector2 v) => _move = v;
        private void OnSprint(bool v) => _sprint = v;

        public void MoveTick(PlayerContext ctx , Transform body)
        {
            Vector3 forward = body.transform.TransformDirection(Vector3.forward);
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = body.transform.TransformDirection(Vector3.right);
            right.y = 0f;
            right.Normalize();

           
            Vector3 wishDir = (_move.x*right + _move.y*forward)*_currentSpeed;
            ctx.velocity.x = wishDir.x;
            ctx.velocity.z = wishDir.z;
            ctx.currentInput = _move;
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
        }
    }
}

