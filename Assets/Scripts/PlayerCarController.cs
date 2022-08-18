using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestructionDerby.Car
{
    public class PlayerCarController : MonoBehaviour
    {
        public CarController carController;
        float? moveInput = null;
        float? turnInput = null;

        public void PlayerInput(float? vertical = null, float? horizontal = null)
        {
            if (vertical != null)
                moveInput = Input.GetAxisRaw("Vertical");
            if (horizontal != null)
                turnInput = Input.GetAxisRaw("Horizontal");
        }

        private void Update()
        {
            if (carController != null)
            {
                PlayerInput(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
                carController.SetInput(moveInput, turnInput);
            }
        }
    }
}