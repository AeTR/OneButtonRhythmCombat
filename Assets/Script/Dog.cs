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
    public ParticleSystem ps;
    public AudioSource aS;

    public bool knockBack;
    public bool dead;

    public Sprite[] sprites;
    public int sprLimit;

    public float startY;

    public AudioClip bark;
    public AudioClip yelp;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        ps = GetComponentInChildren<ParticleSystem>();
        aS = GetComponent<AudioSource>();

        startY = transform.position.y;
    }

    void Update()
    {

        if(dead == true)
        {
            sr.color = Color.black;
            if(!ps.isPlaying)
            {
                ScreenShake.me.ScreenShakeFunc();
                ps.Play();
            }
        }
        
    }


    void FixedUpdate()
    {
        if (Vector2.Distance(this.transform.position, FencerSwordController.me.transform.position) < disLimit && !jumped)
        {
            velocity = new Vector2(baseVelocity.x, jumpVel);
            jumped = true;
            sr.sprite = sprites[2];
            aS.clip = bark;
            aS.pitch = Random.Range(0.8f, 1.2f);
            aS.Play();
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

        if (col.gameObject.tag == "Kill" && !GameManager.me.PlayerDead)
        {
            if(!dead)
            {
                dead = true;
                aS.clip = yelp;
                aS.Play();
            }
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
}
