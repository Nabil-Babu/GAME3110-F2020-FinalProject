using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject spawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = spawnPoint.transform.position;
    }

    public void Respawn()
    {
        transform.position = spawnPoint.transform.position;
    }


}
