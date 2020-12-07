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
        //Debug.Log("Projectile Start");
        rb = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
    }

    public void SetProjectileDirection(Vector2 moveDir)
    {
        rb.velocity = moveDir * speed;
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        PlayerCharacter player = GetComponent<PlayerCharacter>();
        //If projectile is player's
        //if (gameObject.layer == (int)USER_LAYER.PLAYER_PROJECTILE &&
        //    collision.gameObject.layer == (int)USER_LAYER.OPPONENT)
        //{
        //    collision.gameObject.GetComponent<PlayerCharacter>().hp -= 1;
        //    collision.gameObject.GetComponent<PlayerCharacter>().hpBar2.fillAmount -= 0.25f;

        //    if (collision.gameObject.GetComponent<PlayerCharacter>().hp == 0)
        //    {
        //        collision.gameObject.GetComponent<PlayerCharacter>().life -= 1;
        //        collision.gameObject.GetComponent<PlayerCharacter>().PlayerRevive_2();
        //        collision.gameObject.GetComponent<PlayerSpawner>().Respawn();

        //        if (collision.gameObject.GetComponent<PlayerCharacter>().life == 0)
        //        {
        //            Debug.Log("0");
        //            collision.gameObject.GetComponent<PlayerCharacter>().OnGameOver();
        //        }
        //    }
            
        //}
        if(gameObject.layer == (int)USER_LAYER.OPPONENT_PROJECTILE &&
            collision.gameObject.layer == (int)USER_LAYER.PLAYER)
        {
            collision.gameObject.GetComponent<PlayerCharacter>().hp -= 1;
            collision.gameObject.GetComponent<PlayerCharacter>().hpBar.fillAmount -= 0.25f;

            if (collision.gameObject.GetComponent<PlayerCharacter>().hp == 0)
            {
                collision.gameObject.GetComponent<PlayerCharacter>().life -= 1;
                collision.gameObject.GetComponent<PlayerCharacter>().PlayerRevive_1();
                collision.gameObject.GetComponent<PlayerSpawner>().Respawn();

                if (collision.gameObject.GetComponent<PlayerCharacter>().life == 0)
                {
                    Debug.Log("0");
                    collision.gameObject.GetComponent<PlayerCharacter>().OnGameOver();
                }
            }
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
