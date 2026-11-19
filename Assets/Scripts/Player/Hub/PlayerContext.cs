using UnityEngine;

public class PlayerContext 
{
    public Vector3 velocity;
    public Vector3 currentInput;
    public bool IsMoving => currentInput.sqrMagnitude >= 0.001;

    public RaycastHit hit;

    public bool IsSprinting;
}
