using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public Vector2 ogPos;
    public Quaternion ogRot;
    public Vector2 baseVelocity;
    public Vector2 velocity;

    public SpriteRenderer sr;
    public BoxCollider2D bc;

    public int health;
    public bool stunned;
    public bool knockback;
    public bool jumpback;
    public bool dead;

    public float jumpLimit;
    public float jumpVel;
    public float t;

    public int knockLimit;
    public int knockTimer;
    public int stunLimit;
    public int stunTimer;

    public Sprite[] sprites;
    public int sprLimit;

    public AudioSource aS;
    public AudioClip stun;
    public AudioClip stab;
    public AudioClip kill;

    public ParticleSystem psDeath;
    public ParticleSystem psStun;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        aS = GetComponent<AudioSource>();

        ogPos = transform.position;
        ogRot = transform.rotation;

        sr.sprite = sprites[0];

        aS.pitch = Random.Range(0.8f, 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(jumpback)
        {
            sr.sprite = sprites[3];
        }

        if(stunned)
        {
            sr.sprite = sprites[2];
        }

        if (dead)
        {
            sr.color = Color.black;
        }
    }

    void FixedUpdate()
    {
        if(stunned)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(FencerSwordController.me.bc.transform.position.x, transform.position.y), 0.75f);         
            if(GameManager.me.timer > stunTimer)
            {
                stunned = false;
                if (!dead)
                {
                    jumpback = true;
                    velocity = new Vector2(-baseVelocity.x, jumpVel);
                }
            }
        }

        if (jumpback)
        {
            //velocity = new Vector3(-baseVelocity.x, Mathf.Lerp(velocity.y, -jumpVel, t));
            velocity = new Vector3(-baseVelocity.x, 0);
            transform.Rotate(0, 0, -5);
            //if(Vector2.Distance(transform.position, new Vector2(transform.position.x, 0)) < 0.2f)
            if(GameManager.me.timer > jumpLimit)
            {
                jumpback = false;
                transform.position = new Vector2(transform.position.x, ogPos.y);
                transform.rotation = ogRot;
                velocity = baseVelocity;
                sr.sprite = sprites[0];
            }
        }

        if (GameManager.me.PlayerDead)
        {
            velocity = Vector2.zero;
        }


        if (!GameManager.me.PlayerDead && sprLimit < GameManager.me.timer)
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


        transform.Translate(velocity, Space.World);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Destroy")
        {
            EnemySpawner.me.liveEnemies.Remove(this.gameObject);

            Destroy(this.gameObject);
        }       

        if (!knockback && !dead)
        {
            if (col.gameObject.tag == "Kill" && stunned && !GameManager.me.PlayerDead)
            {
                health--;

                if (health == 1)
                {
                    stunned = false;
                    jumpback = true;
                    jumpLimit = GameManager.me.timer + 60;
                    aS.clip = stab;
                    aS.Play();
                    ScreenShake.me.ScreenShakeFunc();
                    transform.position = transform.position + (Vector3.up * 0.5f);
                    velocity = new Vector2(-baseVelocity.x, 0);
                }
                else
                {
                    aS.clip = kill;
                    aS.Play();
                    psDeath.Play();
                    ScreenShake.me.ScreenShakeFunc();
                    dead = true;
                }
            }

            if (!stunned)
            {
                if (col.gameObject.tag == "Player")
                {
                    if (!FencerSwordController.me.space && FencerSwordController.me.resetTimer > -1)
                    {
                        stunned = true;
                        psStun.Play();
                        aS.clip = stun;
                        aS.Play();
                        ScreenShake.me.ScreenShakeFunc();
                        stunTimer = GameManager.me.timer + stunLimit;
                        FencerSwordController.me.Guard();
                        Debug.Log("Stunned");
                    }
                    else
                    {
                        GameManager.me.PlayerDead = true;
                        Debug.Log(col.gameObject + " Killed Player");
                    }
                }
            }
        }

    }
}
