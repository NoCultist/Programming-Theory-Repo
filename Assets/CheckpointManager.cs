using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace DestructionDerby.Car
{
    public class CheckpointManager : MonoBehaviour
    {
        private static CheckpointManager instance;
        public static CheckpointManager Instance => instance;

        private void Awake()
        {
            instance = this;

            var _checkpoints = GetComponentsInChildren<Checkpoint>();
            var qry = from p in _checkpoints
                      orderby p.Index
                      select p;

            List<Checkpoint> all = new List<Checkpoint>();
            Array.ForEach(qry.ToArray(), p => all.Add(p));
            checkpoints = all.ToArray();
        }

        [SerializeField] Checkpoint[] checkpoints;
        public int Count => checkpoints.Length;
        private void Start()
        {


        }

        public Transform GetCheckpoint(int index)
        {
            if (index < checkpoints.Length)
                return checkpoints[index].transform;
            else
                return checkpoints[0].transform;
        }
    }
}