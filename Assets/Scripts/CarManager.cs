using System.Linq;
using UnityEngine;

namespace DestructionDerby.Car
{
    
    public class CarManager : MonoBehaviour
    {
        
        //public GameObject[] carGroups;
        //public int difficulty = 0;

        [field: SerializeField] public LayerMask GroundLayer { get; private set; }

        [SerializeField]private CarController[] cars;
        public CarController[] Cars => cars;
        private int playerCarIndex;


        private void Start()
        {
            //switch (difficulty)
            //{
            //    case 0:
            //        foreach (var carGroup in carGroups)
            //        {
            //            carGroup.gameObject.SetActive(false);
            //        }
            //        break;
            //    case 1:
            //        foreach (var carGroup in carGroups)
            //        {
            //            carGroup.gameObject.SetActive(false);
            //        }
            //        carGroups[0].gameObject.SetActive(true);
            //        break;
            //    case 2:
            //        foreach (var carGroup in carGroups)
            //        {
            //            carGroup.gameObject.SetActive(true);
            //        }
            //        break;


            //}



            
        }

        private int GetPlayerCarIndex() //returns -1 if no player
        {
            int index = -1;
            for (int i = 0; i < cars.Length; i++) { 
                if (cars[i].transform.parent.TryGetComponent(out PlayerCarController player)){
                    index = i; 
                    break;
                } 
            }
            return index;
        }


        //Singleton
        private static CarManager instance;
        public static CarManager Instance
        {
            get { return instance; }
        }

        private void Awake()
        {
            if (instance == this) return;
            if (instance != null)
                Destroy(gameObject);
            else
            {
                instance = this;
            }

            cars = FindObjectsOfType<CarController>();
            if (cars.Length == 0)
            {
                Debug.LogError("No cars detected!");
            }
            else
            {
                playerCarIndex = GetPlayerCarIndex();
                Debug.Log("PlayerCarIndex:" + playerCarIndex);
            }
        }
        //End of Singleton
    }
}