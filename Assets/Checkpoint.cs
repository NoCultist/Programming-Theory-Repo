using UnityEngine;

namespace DestructionDerby.Car
{ 
    public class Checkpoint : MonoBehaviour
    {
        public int Index;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent(out AiCarController AiCar))
            {
                if (AiCar.lastCheckpoint == Index - 1)
                {
                    AiCar.SetCheckpoint(Index);
                }

            }
        }
    }
}