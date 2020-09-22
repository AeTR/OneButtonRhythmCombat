using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public Vector2 baseVelocity;
    public Vector2 velocity;
    public float jumpVel;

    public float disLimit;
    public bool jumped;
    public float t;

    public SpriteRenderer sr;
    public BoxCollider2D bc;

    public bool knockBack;
    public bool dead;

    public Sprite[] sprites;
    public int sprLimit;

    public float startY;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        startY = transform.position.y;
    }

    void Update()
    {

        if(dead == true)
        {
            sr.color = Color.black;
        }
        
    }


    void FixedUpdate()
    {
        if (Vector2.Distance(this.transform.position, FencerSwordController.me.transform.position) < disLimit && !jumped)
        {
            velocity = new Vector2(baseVelocity.x, jumpVel);
            jumped = true;
            sr.sprite = sprites[2];
        }

        if (jumped && !knockBack)
        {
            velocity = Vector2.Lerp(velocity, new Vector2(baseVelocity.x, -jumpVel), t);
        }

        if (knockBack)
        {
            velocity = Vector2.Lerp(velocity, new Vector2(0, -jumpVel), t);
        }

        if (knockBack && Vector2.Distance(transform.position, new Vector2(transform.position.x, startY)) < 0.1f)
        {
            knockBack = false;
            jumped = false;
            velocity = baseVelocity;
            sr.sprite = sprites[0];
        }

        if (GameManager.me.PlayerDead)
        {
            velocity = Vector2.zero;
        }

        if (!GameManager.me.PlayerDead && !knockBack && !jumped && sprLimit < GameManager.me.timer)
        {
            if (sr.sprite == sprites[0])
            {
                sr.sprite = sprites[1];
            }
            else if (sr.sprite == sprites[1])
            {
                sr.sprite = sprites[0];
            }
            sprLimit = GameManager.me.timer + 15;
        }

        transform.Translate(velocity);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Destroy")
        {
            EnemySpawner.me.liveEnemies.Remove(this.gameObject);

            Destroy(this.gameObject);
        }

        if (col.gameObject.tag == "Kill")
        {
            Kill();
        }

        if(col.gameObject.tag == "Break")
        {
            velocity = new Vector2(-baseVelocity.x * 2, 2 * jumpVel);
            knockBack = true;
        }

        if(col.gameObject.tag == "Player" && !dead)
        {
            GameManager.me.PlayerDead = true;
            transform.parent = col.gameObject.transform;
            Debug.Log(col.gameObject + " Killed Player");
        }
    }

    public void Kill()
    {
        dead = true;
    }
}
