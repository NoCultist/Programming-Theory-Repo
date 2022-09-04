using UnityEngine;
using UnityEngine.Events;

namespace DestructionDerby.Car
{
    public class CarController : MonoBehaviour
    {

        private CheckpointManager _checkpointManager;
        public int lastCheckpoint = -1;
        public int lap = 0;

        [SerializeField] private UnityEvent<bool> OnToggleDrift;
        [SerializeField] private UnityEvent<bool> OnToggleAccelerate;



        internal void SetCheckpoint(int index)
        {
            lastCheckpoint = index;
        }

        public Rigidbody rb;
        public CarSO car;
        public float alignToGroundTime = 2;

        public LayerMask groundLayer;
        [SerializeField] protected Transform centerOfMass;
        public Transform CenterOfMass => centerOfMass;
        public bool isGrounded;
        private Vector2 input = Vector2.zero;
        public bool isAi = false;

        private void Start()
        {
            groundLayer = CarManager.Instance.GroundLayer;
            rb = GetComponent<Rigidbody>();
            rb.mass = car.Mass;
            _checkpointManager = CheckpointManager.Instance;
        }


        public float driftSensitivity = .5f;
        private void FixedUpdate()
        {
            var vel = transform.InverseTransformDirection(rb.velocity);
            if (Mathf.Abs(vel.z) > 0.1f && (vel.x > Mathf.Abs(vel.z)*driftSensitivity || vel.x < -Mathf.Abs(vel.z)*driftSensitivity))
            {
                OnToggleDrift?.Invoke(true);
            }
            else
            {
                OnToggleDrift?.Invoke(false); 
            }


            if (!isAi)
            {


                if (centerOfMass != null) rb.centerOfMass = centerOfMass.localPosition;
                isGrounded = Physics.Raycast(centerOfMass.position, -centerOfMass.up, 1f, groundLayer);
                Move();
            }
            UpdatePosition();
        }
        bool lapflag = true;
        
        public void UpdatePosition()
        {
            if (lastCheckpoint == _checkpointManager.Count - 1 && lapflag == false)
            {
                lap++;
                lapflag = true;
            }
            else if (lastCheckpoint != _checkpointManager.Count - 1 && lapflag == true)
            {
                lapflag = false;
            }
            if (lastCheckpoint == _checkpointManager.Count - 1)
            {

                lastCheckpoint = -1;

            }
            _checkpointManager.UpdatePosition(this, lap, lastCheckpoint + 1, Vector3.Distance(transform.position, _checkpointManager.GetCheckpoint(lastCheckpoint + 1).transform.position));
        }

        public void SetInput(float? vertical = null, float? horizontal = null)
        {
            if (vertical != null)
            {
                OnToggleAccelerate?.Invoke(true);
                input.y = vertical.Value;
            }
            else
            {
                OnToggleAccelerate?.Invoke(false);
            }
            if (horizontal != null) input.x = horizontal.Value * vertical.Value;
        }

        public void Move()
        {
            float newRot = input.x * car.TurnSpeed * 15 * Time.fixedDeltaTime;              //Calculate Rotation
            float inputForward = input.y switch {                                           //Calculate Movement
                < 0f => car.BackwardAcceleration / 1.25f * input.y,
                > 0f => car.ForwardAcceleration / 1.25f * input.y,
                _ => 0
            };

            if (isGrounded) transform.Rotate(0, newRot, 0, Space.World);                    //Apply Rotation
            if (isGrounded && rb.velocity.magnitude < car.MaxForwardSpeed) 
                rb.AddForce(transform.forward * inputForward * 3, ForceMode.Acceleration);  //Apply Movement
            else rb.AddForce(Vector3.up * -400f);                                           //Apply Gravity
        }
    }
}
