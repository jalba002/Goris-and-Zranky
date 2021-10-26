using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DATA_DEFAULT_PlayerProperties", menuName = "Data/Player Properties")]
public class PlayerProperties : ScriptableObject
{
    [Title("Walking")]
    public float WalkingSpeed = 10f;
    public float MaxWalkingSpeed = 10f;

    [Title("Jumping")]
    [Tooltip("The value must be introduced already downscaled with >Time.deltaTime<")]
    public float JumpingSpeed = 10f;
    public int CoyoteFrames = 10;
    
    [Title("Turning")]
    public float TurningSpeed = 720f;

    [Title("Gravity")]
    [Tooltip("Input the gravity value as a positive number.")]
    public float GravitySpeed = 20f;
    public float MaxGravity = 20f;
}