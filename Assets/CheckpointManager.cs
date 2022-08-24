using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace DestructionDerby.Car
{
    public class CheckpointManager : MonoBehaviour
    {

        public int RaceLapCount = 3;
        private static CheckpointManager instance;
        private CarManager carManager;

        public static CheckpointManager Instance => instance;
        
        private void Awake()
        {
            instance = this;
        }

        [SerializeField] Checkpoint[] checkpoints;
        public int Count => checkpoints.Length;
        private void Start()
        {
            var _checkpoints = GetComponentsInChildren<Checkpoint>();
            var qry = from p in _checkpoints
                      orderby p.Index
                      select p;

            List<Checkpoint> all = new List<Checkpoint>();
            Array.ForEach(qry.ToArray(), p => all.Add(p));
            checkpoints = all.ToArray();

            carManager = CarManager.Instance;
            Debug.Log(carManager);
            foreach (CarController car in carManager.Cars)
            {
                places.Add(new CarPlace(car, car.isAi));
            }
        }

        public Transform GetCheckpoint(int index)
        {
            if (index < checkpoints.Length)
                return checkpoints[index].transform;
            else
                return checkpoints[0].transform;
        }


        List<CarPlace> places = new List<CarPlace>();

        class CarPlace
        {
            public CarController car;
            public (int lap, int nextChkpoint, float dist) positionValue;
            public bool isAi;
            public CarPlace(CarController car, bool isAi)
            {
                this.car = car;
                this.positionValue = (0, 0, 0);
                this.isAi = isAi;
            }
        }


        public TMPro.TMP_Text ui;
        public void UpdatePosition(CarController car, int Lap, int nextCheckpoint, float distanceToNextCheckpoint)
        {
            
            var pos = places.Find(x => x.car == car);
            int index = places.IndexOf(pos);
            places[index].positionValue = (Lap, nextCheckpoint, distanceToNextCheckpoint);
            places = places.OrderByDescending(x => x.positionValue.lap).ThenByDescending(x=>x.positionValue.nextChkpoint).ThenBy(x=>x.positionValue.dist).ToList();
            string log = "";
            foreach (CarPlace place in places)
            {
                log += place.car.transform.parent.gameObject.name + " Lap:" + place.positionValue.lap + " | chkpt:"+place.positionValue.nextChkpoint+ "\n";
            }
            if(places.Any(x=>x.positionValue.lap == 3))
            {
                log = places.Find(x => x.positionValue.lap == 3).car.transform.parent.name + " Won!";

                Time.timeScale = 0;
            }
            ui.text = log;
        }
    }
}