//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    Rigidbody2D rb; //Projectile's rigid body

    public float speed = 5f; //projectile speed

    public PlayerCharacter owner = null; //Who ownes this projectile?

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Projectile Start");
        rb = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void SetProjectileDirection(Vector2 moveDir)
    {
        rb.velocity = moveDir * speed;
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);

        //If projectile is player's
        if(gameObject.layer == (int)USER_LAYER.PLAYER_PROJECTILE &&
            collision.gameObject.layer == (int)USER_LAYER.OPPONENT)
        {
            collision.gameObject.GetComponent<PlayerCharacter>().hp -= 1;
        }
        else if(gameObject.layer == (int)USER_LAYER.OPPONENT_PROJECTILE &&
            collision.gameObject.layer == (int)USER_LAYER.PLAYER)
        {
            collision.gameObject.GetComponent<PlayerCharacter>().hp -= 1;
        }

        //    if (collision.gameObject.layer == USER_LAYER.PLAYER)
        //{
        //    //Debug.Log("Collide with chafracter");
        //    //If projectile collide with opposite layer(either opponent or player)
        //    if(gameObject.layer != collision.gameObject.layer)
        //    {

        //    }
        //}    

        //If collide with Tile, Opponent, deactivate it and set it back to projectile pool
        if (owner != null)
        {
            gameObject.SetActive(false);
            owner.ReturnProjectile(this);
        }
    }
}
