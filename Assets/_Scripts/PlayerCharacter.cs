﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum USER_LAYER
{
    TILE = 8,
    OPPONENT,
    PLAYER,
    OPPONENT_PROJECTILE,
    PLAYER_PROJECTILE
}

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
[DefaultExecutionOrder(-100)] //ensure this script runs before all other player scripts to prevent laggy input
public class PlayerCharacter : MonoBehaviour
{
    #region Variables

    Rigidbody2D rb = null; //player's rigid body
    CapsuleCollider2D capsuleCollider2D = null; //Player's capsule collider

    [Header("Attribute")]
    public float moveSpeed = 3.0f;
    public float jumpForce = 10f; //How strong does player jump
    public float shootCoolTime = 0.5f; //Projectile shoot cool time
    public bool IsPlayer2 = false; //Temporary flag to control player2
    public int hp = 10;

    Vector2 moveDir = Vector2.zero; //player's movement direction

    bool shouldJump = false; //Check if player should jump
    bool canShoot = true; //Check if player can shoot projectile

    [Header("Projectile")]
    public int maxNumOfProjectile = 10; //Max number of projectile
    public GameObject projectilePrefab = null; //Prefab for projectile
    Queue<Projectile> listOfProjectile = null; //Queue for projectile pool
    public float projectileSpawnXOffset = 1f; //How far does projectile spawn from player
    public float projectileSpawnYOffset = 1f; //How far does projectile spawn from player

    bool isFacingRight = false; //Is character facing right side? for Characte flip

    [SerializeField] LayerMask tileLayerMask; //Used to check if player is on ground
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        //Create queue for projectile pool
        listOfProjectile = new Queue<Projectile>();
        for(int i = 0; i < maxNumOfProjectile; ++i)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            
            //Set layer 
            if (gameObject.layer == (int)USER_LAYER.PLAYER)
            {
                projectile.layer = (int)USER_LAYER.PLAYER_PROJECTILE;
            }
            else if (gameObject.layer == (int)USER_LAYER.OPPONENT)
            {
                projectile.layer = (int)USER_LAYER.OPPONENT_PROJECTILE;
            }

            //Add to pool
            listOfProjectile.Enqueue(projectile.GetComponent<Projectile>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerMove();
        HandleInput();

        IsPlayerOnGround();
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
        //Input for player1
        if (IsPlayer2 == false)
        {
            //Horizontal move
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveDir.x = -1;

                //Characte flip
                if (isFacingRight == true)
                {
                    isFacingRight = false;
                    transform.Rotate(0f, 180f, 0f);
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                moveDir.x = 1;

                //Characte flip
                if (isFacingRight == false)
                {
                    isFacingRight = true;
                    transform.Rotate(0f, 180f, 0f);
                }
            }
            else
            {
                moveDir.x = 0f;
            }


            //Jump
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (IsPlayerOnGround() == true)
                {
                    shouldJump = true;
                }
            }

            //Shoot
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (canShoot == true)
                {
                    ShootProjectile();
                }
            }
        }
        //Input for player2
        else if(IsPlayer2 == true)
        {
            //Horizontal move
            if (Input.GetKey(KeyCode.A))
            {
                moveDir.x = -1;

                //Characte flip
                if (isFacingRight == true)
                {
                    isFacingRight = false;
                    transform.Rotate(0f, 180f, 0f);
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveDir.x = 1;

                //Characte flip
                if (isFacingRight == false)
                {
                    isFacingRight = true;
                    transform.Rotate(0f, 180f, 0f);
                }
            }
            else
            {
                moveDir.x = 0f;
            }


            //Jump
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (IsPlayerOnGround() == true)
                {
                    shouldJump = true;
                }
            }

            //Shoot
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (canShoot == true)
                {
                    ShootProjectile();
                }
            }
        }
    }

    void PlayerMove()
    {
        float moveX = Input.GetAxis("Horizontal");

        Vector3 moveVector = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
        //transform.Translate(moveVector);
        rb.velocity = moveVector;
    }

    void ShootProjectile()
    {
        //Get projectile from list
        if (listOfProjectile.Count != 0)
        {
            Projectile projectile = listOfProjectile.Dequeue();

            //Activate projectile
            projectile.gameObject.SetActive(true);

            //Set projectile in front of player
            Vector3 forwardVec = -transform.right;
            Vector3 upwardVec = transform.up;
            projectile.transform.position = transform.position + (forwardVec * projectileSpawnXOffset) + (upwardVec * projectileSpawnYOffset);

            //Set projectile move direction
            projectile.SetProjectileDirection(forwardVec);

            //Set owner of this projectile
            projectile.owner = this;

            //Can't shoot projectile continousely
            canShoot = false;
            Invoke("ResetShootCoolDown", shootCoolTime);
        }
    }

    void ResetShootCoolDown()
    {
        canShoot = true;
    }

    //Return projectile to pool
    public void ReturnProjectile(Projectile projectile)
    {
        listOfProjectile.Enqueue(projectile);
    }

    bool IsPlayerOnGround()
    {
        //Do capsule cast to downward of player so that it checks if player is on ground
        RaycastHit2D result = Physics2D.CapsuleCast(capsuleCollider2D.bounds.center, capsuleCollider2D.bounds.size, CapsuleDirection2D.Vertical, 0f, Vector2.down, 0.1f, tileLayerMask);

        //Debug.Log(result.collider);

        return (result.collider != null);
    }
}
