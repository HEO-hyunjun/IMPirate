using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositions : MonoBehaviour
{
    [SerializeField]
    private Transform[] positions;

    private int index = 0;

    public Vector3 GetSpawnPosition()
    {
        Vector3 spawnPosition = positions[index++].position;
        if (index >= positions.Length)
            index = 0;
        return spawnPosition;
    }
}
