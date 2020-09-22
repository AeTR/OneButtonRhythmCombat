using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager me;

    public bool PlayerDead;
    public bool readyToReset;

    public BoxCollider2D killbc;

    public int timer;
    public int timerLimit;

    void Awake()
    {
        me = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer++;

        if(timer == timerLimit)
        {
            killbc.enabled = false;
            FencerBodController.me.parent.transform.position = FencerBodController.me.ogPos;
            FencerBodController.me.v = Vector2.zero;
        }

        if(timer < timerLimit)
        {
            PlayerDead = false;
        }
    }

    void Update()
    {
        if(readyToReset && timer > timerLimit && FencerSwordController.me.space)
        {
            killbc.enabled = true;
            readyToReset = false;
            PlayerDead = false;          
            EnemySpawner.me.RoundReset();
            timerLimit = timer + 15;
        }
    }
}
