using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Interfaces;
using Sirenix.OdinInspector;

namespace Player
{
    [RequireComponent(typeof(UnityEngine.CharacterController))]
    public class PlayerController : RestartableObject
    {
        [Title("Properties")] [Tooltip("Instantiate properties.")]
        public bool InstantiateProperties = true;

        public PlayerProperties Properties;

        private Vector3 Velocity;
        private Vector3 Inertia;
        private bool ResetInertiaOnGround;

        public UnityEngine.CharacterController Controller;

        [Title("Movement")] public bool AlignToGameobject = false;
        [ShowIf("AlignToGameobject")] public GameObject m_Alignment;
        [HideIf("AlignToGameobject")] public bool UseWorldDirection = false;
        public bool RotateTowardsDirection = true;

        [ShowIf("RotateTowardsDirection")]
        [Tooltip("Character rotates towards the direction the player wants, ignoring collisions")]
        public bool RotateTowardsFinalVelocity = true;

        #region DebugZone

        [Title("Debug")] public bool debugMode = false;

        #endregion

        #region Inputs

        // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Interactions.html#multiple-controls-on-an-action

        #endregion

        #region Limiters

        private float LastWalkingSpeed;
        private float LastGravitySpeed;
        private float LastJumpingSpeed;
        private float LastXCameraRotation;

        #endregion

        #region Game-Specific Mechanics

        #endregion

        private PlayerInput _input;

        private bool controlsEnabled = true;

        private new void Awake()
        {
            base.Awake();
            Controller = GetComponent<UnityEngine.CharacterController>();
            Properties = InstantiateProperties ? Instantiate(Properties) : Properties;
            _input = GetComponent<PlayerInput>();
        }

        void Start()
        {
            // Velocity = Vector3.zero;
            _input.actions.FindAction("Pause").performed += Escape;
        }

        private void Update()
        {
            CheckHead();
        }

        private void CheckHead()
        {
            if ((Controller.collisionFlags & CollisionFlags.Above) != 0 && !Controller.isGrounded)
            {
                Velocity.y = 0f;
            }
        }
        
        private void FixedUpdate()
        {
            ApplyVerticalVelocity();
            Vector3 playerVel;
            Vector3 vel = MoveCharacter(out playerVel);

            if (RotateTowardsDirection)
                RotateToFaceMovement(playerVel);
        }

        void ApplyVerticalVelocity()
        {
            // Check if grounded. If it is, then adjust the position to stick the player to the ground.
            // If it is not grounded, then apply the gravity speed. UNLESS THERE MIGHT BE A DIFFERENT STATUS OR PROPULSED STATUS.
            if (Controller.isGrounded)
            {
                // Do not apply gravity. Stick it to the ground.
                // Velocity.y = 0f;
                if (Inertia.magnitude > 0f && ResetInertiaOnGround)
                {
                    Inertia = Vector3.zero; // Resets the inertia from the flying position when touching the ground.
                    ResetInertiaOnGround = false;
                }

                // Seems like setting new velocities moves the character controller much and makes it think its not grounded.
                // Velocity.y = Mathf.Max(0f, Velocity.y);
                // if (Velocity.y < -0.5f)
                //     Velocity.y = -0.5f;
            }
            else
            {
                //LastGravitySpeed = LimitValue(Properties.GravitySpeed * Time.deltaTime, LastGravitySpeed, 1.5f);
                Velocity.y -= ((Mathf.Abs(Velocity.y * 0.2f) + Properties.GravitySpeed) * Time.deltaTime);
                //Velocity.x += (1f - floorNormal.y) * floorNormal.x * (1f - slideFriction);
                //Velocity.z += (1f - floorNormal.y) * floorNormal.z * (1f - slideFriction);

                if (!ResetInertiaOnGround) ResetInertiaOnGround = true;
            }
        }

        void LimitVelocity()
        {
            if (Controller.isGrounded)
            {
                Velocity.x = Mathf.Clamp(Velocity.x, -Properties.MaxWalkingSpeed, Properties.MaxWalkingSpeed);
                Velocity.z = Mathf.Clamp(Velocity.z, -Properties.MaxWalkingSpeed, Properties.MaxWalkingSpeed);
            }

            if (Properties.MaxGravity >= 0f)
                Velocity.y = Mathf.Max(Velocity.y, -Properties.MaxGravity);
        }

        Vector3 MoveCharacter(out Vector3 playerVel)
        {
            LimitVelocity();
            playerVel = AlignVelocity(Velocity);
            Vector3 velocityVector = (playerVel + Inertia) * Time.deltaTime;

            Controller.Move(velocityVector);
            return velocityVector;
            // Maybe rely the movement calculation to an external manager so you can rotate camera?
        }

        Vector3 AlignVelocity(Vector3 vel)
        {
            if (AlignToGameobject)
            {
                return (m_Alignment.transform.TransformDirection(vel));
            }
            else if (UseWorldDirection)
            {
                return (vel);
            }
            else
            {
                return (transform.TransformDirection(vel));
            }
        }

        void RotateToFaceMovement(Vector3 velocityVector)
        {
            // Remove the vertical component of the rotation.
            Vector3 chosenDirection =
                RotateTowardsFinalVelocity ? velocityVector.normalized : Controller.velocity.normalized;
            chosenDirection.y = 0f;

            // Slowly rotate towards a vector, if the speed not enough, rotate and next frame it will try again.
            Vector3 rotatedVector = Vector3.RotateTowards(
                transform.forward,
                chosenDirection,
                Properties.TurningSpeed * Mathf.Deg2Rad * Time.deltaTime,
                0f);

            rotatedVector.y = 0f;
            transform.forward = rotatedVector;
        }

        #region Public Methods

        public void SetVelocity(Vector2 Velocity)
        {
            // Stuff for movement with platforms.
        }

        public void SetInertia(Vector3 Inertia)
        {
            this.Inertia = Inertia;
            ResetInertiaOnGround = false;
        }

        #endregion

        #region Limiters

        private float LimitValue(float OriginalValue, float ComparisonValue, float Threshold = 1.2f)
        {
            // idea here is to divide the original value by the comparison one. 
            // If it is superior than the threshold then return comparison.
            if (Math.Abs(ComparisonValue) < 0.0001f) return OriginalValue;
            return (OriginalValue / ComparisonValue) > Threshold ? ComparisonValue : OriginalValue;
        }

        #endregion

        #region Callbacks

        public void Jump(InputAction.CallbackContext context)
        {
            if (!controlsEnabled) return;
            // Debug.Log("Jumping input detected.");
            // Force remove from ground.
            if (Controller.isGrounded && context.performed)
            {
                //LastJumpingSpeed = LimitValue(Properties.JumpingSpeed * Time.deltaTime, LastJumpingSpeed);
                Velocity.y = Properties.JumpingSpeed;
            }
        }
        
        private void JumpSpeed()
        {
            // USED WHEN KILLING ZOMBIES. DELETE AFTER
            Velocity.y = Properties.JumpingSpeed;
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (!controlsEnabled) return;
            var movement = context.ReadValue<Vector2>();
            float WalkingSpeed = Properties.WalkingSpeed;
            //LastWalkingSpeed = LimitValue(Properties.WalkingSpeed * Time.deltaTime, LastWalkingSpeed, 1.1f);
            Velocity.x = movement.x * WalkingSpeed;
            Velocity.z = movement.y * WalkingSpeed;
        }
        
        // I hate it but okay...
        public void Escape(InputAction.CallbackContext context)
        {
            // Debug.Log("ESCAPE!");
            if (context.performed)
            {
                GameManager.GM.Pause();
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!enabled) return;
            // TODO Check here if the object is "Collidable" so we can treat collisions with the CharacterController.
            try
            {
                IPlayerCollide go = hit.gameObject.GetComponent<IPlayerCollide>();
                if (go != null)
                {
                    go.Collide(gameObject, hit.point);
                    if ((Controller.collisionFlags & CollisionFlags.Above) != 0)
                    {
                        go.CollideTop();
                    }
                    else if ((Controller.collisionFlags & CollisionFlags.Below) != 0)
                    {
                        if(go.CollideBottom(gameObject.transform.position))
                            JumpSpeed();
                    }
                }

                //Debug.Log("Player detected collision with " + hit.gameObject.name);
            }
            catch (NullReferenceException)
            {
            }
        }

        #endregion

        #region Getters

        public Vector3 GetVelocity()
        {
            return Velocity;
        }

        #endregion

        #region Enablers

        public void ToggleInput(bool enable)
        {
            _input.enabled = enable;
        }
        
        public void Kill()
        {
            this.controlsEnabled = false;
            //this.enabled = false;
        }

        public void ToggleControls(bool enable)
        {
            controlsEnabled = enable;
        }
        
        #endregion

        #region Debug

#if UNITY_EDITOR

        private void OnGUI()
        {
            if (!debugMode) return;
            EditorGUILayout.Vector3Field("Velocity", Velocity);
            EditorGUILayout.Vector3Field("Inertia", Inertia);
        }

        // private void OnDrawGizmosSelected()
        // {
        //     Gizmos.DrawSphere(this.gameObject.transform.position + Vector3.down * (Controller.height * 0.5f), 0.2f);
        // }
#endif

        #endregion

        #region Interfaces / Heritage


        public override void Restart()
        {
            GameManager.GM.RespawnPlayer();
        }

        #endregion
    }
}