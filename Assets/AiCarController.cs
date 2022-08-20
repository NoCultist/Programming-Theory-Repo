using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestructionDerby.Car
{
    public class AiCarController : MonoBehaviour
    {
        public CarController carController;

        [SerializeField] private Transform _target;
        public int lastCheckpoint = -1;

        private void Awake()
        {
            carController.isAi = true;
        }

        internal void SetCheckpoint(int index)
        {
            lastCheckpoint = index;
        }

        private void SetTarget()
        {
            _target = CheckpointManager.Instance.GetCheckpoint(lastCheckpoint + 1);
            if (lastCheckpoint == CheckpointManager.Instance.Count - 1)
                lastCheckpoint = -1;
        }

        public void Move()
        {
            SetTarget();
            Vector3 pos = new Vector3(_target.position.x, carController.transform.position.y, _target.position.z);
            //var targetRotation = Quaternion.LookRotation(_target.position - transform.position);
            var targetRotation = Quaternion.LookRotation(pos - carController.transform.position);

            carController.transform.rotation = Quaternion.Slerp(carController.transform.rotation, targetRotation, carController.car.TurnSpeed * Time.fixedDeltaTime);

            if (carController.isGrounded && carController.rb.velocity.z < carController.car.MaxForwardSpeed)
            {
                carController.rb.AddForce(carController.transform.forward * carController.car.ForwardAcceleration * 2, ForceMode.Acceleration); // Add Movement
            }
            else
                carController.rb.AddForce(carController.transform.up * -200f); // Add Gravity
        }

        private void FixedUpdate()
        {
            if (carController.CenterOfMass != null)
                carController.rb.centerOfMass = carController.CenterOfMass.localPosition;

            RaycastHit hit;
            carController.isGrounded = Physics.Raycast(carController.CenterOfMass.position, -carController.CenterOfMass.up, out hit, 1f, carController.groundLayer);

            Move();
        }

    }
}