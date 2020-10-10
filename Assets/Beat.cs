using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
    AudioSource aS;
    public int beatLimit;
    void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!EnemySpawner.me.roundEnd && !GameManager.me.PlayerDead)
        {
            if(GameManager.me.timer == beatLimit)
            {
                aS.Play();
                beatLimit = GameManager.me.timer + 30;
            }
        }
    }
}
