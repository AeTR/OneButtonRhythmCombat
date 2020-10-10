using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager me;

    public bool PlayerDead;
    public bool readyToReset;

    public BoxCollider2D killbc;

    public int timer;
    public int timerLimit;

    public TextMesh tm;

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }

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

        if(EnemySpawner.me.round == 10 && EnemySpawner.me.roundEnd)
        {
            tm.text = "Victory      R to Reset";
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
