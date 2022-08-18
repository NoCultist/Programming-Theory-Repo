using UnityEngine;

namespace DestructionDerby.Car
{
    [CreateAssetMenu(menuName = "Custom/CarSO")]
    [System.Serializable]
    public class CarSO : ScriptableObject
    {
        public GameObject Prefab;
        public float Mass;
        public float MaxForwardSpeed;
        public float MaxBackwardSpeed;
        public float ForwardAcceleration;
        public float BackwardAcceleration;
        public float TurnSpeed;

        private void OnValidate()
        {
            if (Mass <= 0) Mass = 0.001f;
            if (MaxForwardSpeed < 0) MaxForwardSpeed = 0;
            if (MaxBackwardSpeed < 0) MaxBackwardSpeed = 0;
            if (ForwardAcceleration < 0) ForwardAcceleration = 0;
            if (BackwardAcceleration < 0) BackwardAcceleration = 0;
            if (TurnSpeed < 0) TurnSpeed = 0;
        }
    }
}