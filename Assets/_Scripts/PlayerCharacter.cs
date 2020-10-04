using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    #region Variables

    [SerializeField]
    float moveSpeed = 3.0f;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }


    void PlayerMove()
    {
        float moveX = Input.GetAxis("Horizontal");

        Vector3 moveVector = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(moveVector);
    }
}
