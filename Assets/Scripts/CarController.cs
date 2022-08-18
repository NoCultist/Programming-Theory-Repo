using UnityEngine;

namespace DestructionDerby.Car
{
    public class CarController : MonoBehaviour
    {
        public Rigidbody rb;
        public CarSO car;
        public float alignToGroundTime = 2;

        private LayerMask groundLayer;
        [SerializeField] protected Transform centerOfMass;
        protected bool isGrounded;
        private Vector2 input = Vector2.zero;

        private void Start()
        {
            groundLayer = CarManager.Instance.GroundLayer;
            rb = GetComponent<Rigidbody>();
            rb.mass = car.Mass;
        }

        private void FixedUpdate()
        {
            if (centerOfMass != null) rb.centerOfMass = centerOfMass.localPosition;
            isGrounded = Physics.Raycast(centerOfMass.position, -centerOfMass.up, 1f, groundLayer);
            Move();
        }

        public void SetInput(float? vertical = null, float? horizontal = null)
        {
            if (vertical != null) input.y = vertical.Value;
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
