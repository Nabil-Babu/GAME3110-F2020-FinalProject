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

    //Only collide with Tile, Oppoenent layer, see Edit->Project Settings->Physics 2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);

        //If collide with Tile, Opponent, deactivate it and set it back to projectile pool
        if (owner != null)
        {
            gameObject.SetActive(false);
            owner.ReturnProjectile(this);
        }
    }
}
