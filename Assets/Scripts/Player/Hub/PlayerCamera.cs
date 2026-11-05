using System;
using Assets.Scripts.Inputmanager;
using UnityEngine;
namespace Assets.Scripts.Player.Hub
{    
    public sealed class PlayerCamera : IDisposable
    {
        private readonly IPlayerInput _input;
        private readonly PlayerConfig _config;
        private readonly Camera _camera;

        private Vector2 _look;

        private float _yaw = 0f;
        private float _pitch = 0f;
        public PlayerCamera(IPlayerInput input, PlayerConfig config)
        {
            _input = input;
            _config = config;

            _input.Look += OnLook;
        }
        private void OnLook(Vector2 l) => _look = l;
        public void Tick(Transform body, Transform orientation)
        {
            float vertical = _config.InvertY ? -_look.y : _look.y;
            _pitch = Mathf.Clamp(_pitch - vertical*_config.Sensitivity, -_config.LookLimit, _config.LookLimit);
            _yaw += _look.x*_config.Sensitivity;

            orientation.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
            body.localRotation = Quaternion.Euler(0f, _yaw, 0f);
        }

        public void Dispose()
        {
            if(_input == null)return;

            _input.Look -= OnLook;
        }
    }
}
