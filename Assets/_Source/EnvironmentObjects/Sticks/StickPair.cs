using System;
using EnvironmentObjects.Obstacles;
using PoolSystem;
using UnityEngine;

namespace EnvironmentObjects.Sticks
{
    public class StickPair: BaseObstacle
    {
        [field: SerializeField] public Transform[] ItemSpawnPoints { get; private set; }
    }
}