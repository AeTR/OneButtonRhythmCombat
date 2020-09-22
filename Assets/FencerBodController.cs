using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FencerBodController : MonoBehaviour
{
    public static FencerBodController me;

    public SpriteRenderer sr;
    public Sprite[] spr_breathe;

    int spr_counter;
    public int timer_Limit;
    public int timerLimit;

    public Vector2 deadVelocity;
    public Vector3 ogPos;
    public float t;
    public Vector2 v;

    public GameObject parent;

    void Awake()
    {
        me = this;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        timerLimit = GameManager.me.timer + timer_Limit;
        parent = transform.parent.gameObject;
        ogPos = parent.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.me.timer == timerLimit)
        {
            if (spr_counter + 1 < spr_breathe.Length)
            {
                spr_counter++;
            }
            else
            {
                spr_counter = 0;
            }

            if(spr_counter == 0 || spr_counter == 2)
            {
                timerLimit = GameManager.me.timer + (timer_Limit * 2);
            }
            else
            {
                timerLimit = GameManager.me.timer + timer_Limit;
            }
        }

        if(GameManager.me.PlayerDead)
        {
            v = new Vector2(deadVelocity.x, Mathf.Lerp(v.y, deadVelocity.y, t));
            parent.transform.Translate(v);
            if(Vector2.Distance(ogPos, transform.position) > 1)
            {
                GameManager.me.readyToReset = true;
            }
        }

        sr.sprite = spr_breathe[spr_counter];

    }
}
