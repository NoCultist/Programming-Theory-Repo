using UnityEngine;

namespace DestructionDerby.Car
{ 
    public class Checkpoint : MonoBehaviour
    {
        public int Index;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CarController Car))
            {
                if (Car.lastCheckpoint == Index - 1)
                {
                    Car.SetCheckpoint(Index);
                }
            }
        }
    }
}