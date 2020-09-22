using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public Vector2 ogPos;
    public Vector2 baseVelocity;
    public Vector2 velocity;

    public SpriteRenderer sr;
    public BoxCollider2D bc;

    public int health;
    public bool stunned;
    public bool knockback;
    public bool jumpback;
    public bool dead;

    public float t;

    public int knockLimit;
    public int knockTimer;
    public int stunLimit;
    public int stunTimer;

    public SpriteRenderer srHeart;
    public Sprite[] spritesHeart;
    public SpriteRenderer srShield;
    public Sprite[] spritesShield;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        ogPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (stunned)
        {
            srShield.sprite = spritesShield[1];
        }
        else
        {
            srShield.sprite = spritesShield[0];
        }

        if(health == 3)
        {
            srHeart.sprite = spritesHeart[2];
        }

        if (health == 2)
        {
            srHeart.sprite = spritesHeart[1];
        }

        if (health == 1)
        {
            srHeart.sprite = spritesHeart[0];
        }

        if (dead)
        {
            srHeart.color = Color.black;
            srShield.color = Color.black;
        }
    }

    void FixedUpdate()
    {
        if (stunned)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(FencerSwordController.me.bc.transform.position.x, transform.position.y), 0.75f);
            if (GameManager.me.timer > stunTimer)
            {
                stunned = false;
                if (!dead)
                {
                    jumpback = true;
                    velocity = new Vector2(-baseVelocity.x, 0);
                }
            }
        }

        if (jumpback)
        {
            sr.color = Color.blue;
            velocity = new Vector3(-baseVelocity.x, 0);
            if (Vector2.Distance(transform.position, new Vector2(transform.position.x, 0)) < 0.4f)
            {
                jumpback = false;
                transform.position = new Vector2(transform.position.x, ogPos.y);
                velocity = baseVelocity;
            }
        }

        if (knockback)
        {
            velocity = new Vector2(-baseVelocity.x * 1.5f, 0);

            if (GameManager.me.timer > knockTimer)
            {
                knockback = false;
                velocity = baseVelocity;
            }
        }

        if (GameManager.me.PlayerDead)
        {
            velocity = Vector2.zero;
        }

        transform.Translate(velocity);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "Destroy")
        {
            EnemySpawner.me.liveEnemies.Remove(this.gameObject);

            Destroy(this.gameObject);
        }    

        if (!knockback && !dead)
        {
            if (col.gameObject.tag == "Kill" && stunned)
            {
                health--;

                if (health > 0)
                {
                    stunned = false;
                    knockback = true;
                    Debug.Log("KnockedBack");
                    knockTimer = GameManager.me.timer + knockLimit;
                }
                else
                {
                    dead = true;
                }
            }

            if(col.gameObject.tag == "Break")
            {
                stunned = true;
                stunTimer = GameManager.me.timer + stunLimit;
                Debug.Log("Stunned");
            }

            if (!stunned)
            {
                if (col.gameObject.tag == "Player")
                {
                    GameManager.me.PlayerDead = true;
                    Debug.Log(col.gameObject + " Killed Player");
                }
            }
        }

    }
}
