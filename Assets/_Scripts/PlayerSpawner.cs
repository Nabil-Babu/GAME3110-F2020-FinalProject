using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Vector3 spawnPoint = Vector3.zero;
    public void Respawn()
    {
        transform.position = spawnPoint;
    }


}
