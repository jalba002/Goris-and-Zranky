using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "DATA_DEFAULT_PlayerProperties", menuName = "Data/Player Properties")]
public class PlayerProperties : ScriptableObject
{
    [Title("Walking")]
    public float WalkingSpeed = 5f;
    public float MaxWalkingSpeed = 6f;
    [ReadOnly] public float ScaledWalkingSpeed;

    [Title("Jumping")]
    [Tooltip("The value must be introduced already downscaled with >Time.deltaTime<")]
    public float JumpingSpeed = 0.1f;
    
    [Title("Turning")]
    //public float TurningSpeedDeg = 180;
    public float TurningSpeed = 720f;

    [Title("Gravity")]
    [Tooltip("Input the gravity value as a positive number.")]
    public float GravitySpeed = 5f;
    public float MaxGravity = 5f;

    [Title("Other info.")]
    [ReadOnly] public float ScaledGravitySpeed;
    [ReadOnly] public float ScaledJumpingSpeed;

    //public int CoyoteFrames = 10;
#if UNITY_EDITOR
    [Title("Physics Data", "Calculations are done automatically.")]
    //[ShowInInspector, ReadOnly] private float TimeToReachMaxHeight => (JumpingSpeed / GravitySpeed);
    //[ShowInInspector, ReadOnly] public float ScaledTimeToReachMaxHeight => (ScaledJumpingSpeed / ScaledGravitySpeed);
    //[ShowInInspector, ReadOnly] public float MaxHeight => (Mathf.Pow(JumpingSpeed, 2) / (2 * GravitySpeed));
    [ShowInInspector, ReadOnly]
    [InfoBox("Max height the object will get displaced in Unity Units.")]
    public float ScaledMaxHeight => (Mathf.Pow(JumpingSpeed, 2) / (2 * GravitySpeed));
    
    public void ScaleSpeeds()
    {
        ScaledWalkingSpeed = WalkingSpeed * 0.0166f;
        ScaledJumpingSpeed = JumpingSpeed * 0.0166f;
        ScaledGravitySpeed = GravitySpeed * 0.0166f;
    }

    private void OnValidate()
    {
        ScaleSpeeds();
    }
#endif
}