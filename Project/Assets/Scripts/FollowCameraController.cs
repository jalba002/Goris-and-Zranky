using UnityEngine;
// ReSharper disable InconsistentNaming

namespace ECM.Examples
{
    public sealed class FollowCameraController : MonoBehaviour
    {
        #region PUBLIC FIELDS

        [SerializeField]
        private CharacterController _target;

        [SerializeField]
        private float _distanceToTarget = 15.0f;
        
        [SerializeField]
        private float _shoulderHeight = 2.0f;
        
        [SerializeField]
        private float _velocityStrength = 2.0f;

        [SerializeField]
        private float _followSpeed = 3.0f;

        #endregion

        #region PROPERTIES

        public CharacterController target
        {
            get { return _target; }
            set { _target = value; }
        }

        public float distanceToTarget
        {
            get { return _distanceToTarget; }
            set { _distanceToTarget = Mathf.Max(0.0f, value); }
        }
        
        public float shoulderHeight
        {
            get { return _shoulderHeight; }
            set { _shoulderHeight = Mathf.Max(0.0f, value); }
        }
        
        public float velocityStrength
        {
            get { return _velocityStrength; }
            set { _velocityStrength = Mathf.Max(0.0f, value); }
        }

        public float followSpeed
        {
            get { return _followSpeed; }
            set { _followSpeed = Mathf.Max(0.0f, value); }
        }

        private Vector3 cameraRelativePosition
        {
            get
            {
                //Vector3 tg = target.velocity;
                //tg.y = 0f;
                return (target.transform.position + target.transform.up * shoulderHeight)- transform.forward * distanceToTarget;
            }
        }

        #endregion

        #region MONOBEHAVIOUR

        public void OnValidate()
        {
            distanceToTarget = _distanceToTarget;
            followSpeed = _followSpeed;
        }

        public void Awake()
        {
            transform.position = cameraRelativePosition;
        }

        public void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, cameraRelativePosition, followSpeed * Time.deltaTime);
        }

        public void ForceNewPos()
        {
            transform.position = cameraRelativePosition;
        }

        #endregion
    }
}