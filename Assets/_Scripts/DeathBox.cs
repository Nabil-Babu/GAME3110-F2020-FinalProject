using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathBox : MonoBehaviour
{
    public GameObject diePanel;
    //public GameObject playerObject;
    //public GameObject textLife;

    private void Start()
    {
        //playerObject = GameObject.Find("Player");
        //textLife = GameObject.Find("PlayerLife");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.GetComponent<PlayerCharacter>())
        {
            collider.gameObject.GetComponent<PlayerCharacter>().life--;
            collider.gameObject.GetComponent<PlayerSpawner>().Respawn();

            if (collider.gameObject.GetComponent<PlayerCharacter>().life == 2)
            {
                collider.gameObject.GetComponent<PlayerCharacter>().textLife.GetComponent<Text>().text = "2";
                //textLife.GetComponent<Text>().text = "2";
            }

            if (collider.gameObject.GetComponent<PlayerCharacter>().life == 1)
            {
                collider.gameObject.GetComponent<PlayerCharacter>().textLife.GetComponent<Text>().text = "2";
                //textLife.GetComponent<Text>().text = "1";
            }

            if (collider.gameObject.GetComponent<PlayerCharacter>().life == 0)
            {
                Destroy(collider.gameObject.GetComponent<PlayerCharacter>());

                diePanel.SetActive(true);
                //Destroy(playerObject);
                Debug.Log("0");
                //collider.gameObject.GetComponent<PlayerCharacter>().OnGameOver();
            }
        }
    }
}
