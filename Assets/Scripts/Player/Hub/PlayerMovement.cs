using System;
using Assets.Scripts.Inputmanager;
using UnityEngine;

public class PlayerMovement : IDisposable
{
    private readonly IPlayerInput _input;

    private Vector2 _move;
    public PlayerMovement(IPlayerInput input)
    {
        _input = input;
        _input.Move += OnMove;
    }

    private void OnMove(Vector2 v) => _move = v;

    public void Tick(CharacterController body, float dt)
    {
        Vector3 forward = body.transform.TransformDirection(Vector3.forward);
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = body.transform.TransformDirection(Vector3.right);
        right.y = 0f;
        right.Normalize();

        Vector3 wishDir = _move.x*right + _move.y*forward;

        body.Move(wishDir*dt);
    }

    public void Dispose()
    {
        if(_input == null)return;
        _input.Move -= OnMove;
    }
}
