using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[DefaultExecutionOrder(-100)] //ensure this script runs before all other player scripts to prevent laggy input
public class PlayerCharacter : MonoBehaviour
{
    #region Variables

    Rigidbody2D rb; //player's rigid body

    [SerializeField]
    float moveSpeed = 3.0f;

    Vector2 moveDir = Vector2.zero; //player's movement direction

    bool shouldJump = false; //Check if player should jump
    [SerializeField]
    float jumpForce = 10f; //How strong does player jump

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerMove();
        HandleInput(); 
    }

    private void FixedUpdate()
    {
        //Jump
        if(shouldJump == true)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            shouldJump = false;
        }

        //Change player's velocity
        Vector2 tempVel = rb.velocity;
        tempVel.x = moveDir.x * moveSpeed;
        rb.velocity = tempVel;
    }

    void HandleInput()
    {
        //Horizontal move
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir.x = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDir.x = 1;
        }
        else
        {
            moveDir.x = 0f;
        }


        //Jump
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            shouldJump = true;
        }
    }

    void PlayerMove()
    {
        float moveX = Input.GetAxis("Horizontal");

        Vector3 moveVector = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
        //transform.Translate(moveVector);
        rb.velocity = moveVector;
    }
}
