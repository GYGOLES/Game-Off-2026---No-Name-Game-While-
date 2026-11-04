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

    public void MoveTick(PlayerContext ctx , Transform body)
    {
        Vector3 forward = body.transform.TransformDirection(Vector3.forward);
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = body.transform.TransformDirection(Vector3.right);
        right.y = 0f;
        right.Normalize();

        Vector3 wishDir = _move.x*right + _move.y*forward;
        ctx.velocity.x = wishDir.x;
        ctx.velocity.z = wishDir.z;
    }

    public void GravityTick(PlayerContext ctx, bool g)
    {
        Debug.Log(g);
        if (!g)
        {
            ctx.velocity.y -= 20;
        }
    }
    public void Dispose()
    {
        if(_input == null)return;
        _input.Move -= OnMove;
    }
}
