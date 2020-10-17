﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.GetComponent<PlayerCharacter>())
        {
            collider.gameObject.GetComponent<PlayerCharacter>().life--;
            collider.gameObject.GetComponent<PlayerSpawner>().Respawn();
        }
    }
}