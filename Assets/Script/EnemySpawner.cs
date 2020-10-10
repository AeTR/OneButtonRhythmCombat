using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner me;

    public GameObject dog;
    public GameObject soldier;
    public GameObject heart;

    public RoundData[] roundDatas;
    public bool roundActive;
    public bool roundEnd;
    public int round;
    public int lastBeat;

    public int[] beats;
    public GameObject[] enemies;
    public int dataInt;
    
    public int beatTimer;
    public int nextBeat;

    public List<GameObject> liveEnemies;

    public Beat beat;

    public TextMesh roundCounter;

    // Start is called before the first frame update
    void Awake()
    {
        me = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(dog, transform.position - (Vector3.up * 0.1f), transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Instantiate(soldier, transform.position + (Vector3.up * 0.35f), transform.rotation);
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Instantiate(heart, transform.position + (Vector3.up * 0.5f), transform.rotation);
        }

        if(FencerSwordController.me.space && roundEnd && GameManager.me.timer > GameManager.me.timerLimit && round != 10)
        {
            round++;
            roundActive = true;
            roundEnd = false;
            RoundDataCollect();
        }

        if(GameManager.me.PlayerDead == true)
        {
            roundActive = false;
        }

        roundCounter.text = "" + round;
    }

    void FixedUpdate()
    {

        if (round > -1 && roundActive)
        {
            if (GameManager.me.timer == beatTimer)
            {
                beatTimer = GameManager.me.timer + 15;
                nextBeat++;
            }

            if(nextBeat == beats[dataInt])
            {
                if (enemies[dataInt] == dog)
                {
                    liveEnemies.Add(Instantiate(dog, transform.position - (Vector3.up * 0.1f) + Vector3.forward * 0.1f, transform.rotation));
                }

                if (enemies[dataInt] == soldier)
                {
                    liveEnemies.Add(Instantiate(soldier, transform.position + (Vector3.up * 0.35f), transform.rotation));
                }

                if (enemies[dataInt] == heart)
                {
                    liveEnemies.Add(Instantiate(heart, transform.position + (Vector3.up * 0.5f) - Vector3.forward * 0.1f, transform.rotation));
                }

                dataInt++;

                if (beats[dataInt] == 0)
                {
                    Debug.Log("Hit end of round");
                    for (int i = 0; i < beats.Length; i++)
                    {
                        dataInt = 0;
                        beats[i] = 0;
                        enemies[i] = null;
                        nextBeat = 0;
                    }

                    roundActive = false;
                }
            }
        }

        if(!roundActive && !roundEnd)
        {
            if (liveEnemies.Count == 0)
            {
                roundEnd = true;
            }
        }


    }

    void RoundDataCollect()
    {
        for (int i = 0; i < roundDatas[round].roundDataHolders.Length; i++)
        {
            beats[i] = roundDatas[round].roundDataHolders[i].beat + lastBeat;
            lastBeat = beats[i];
            enemies[i] = roundDatas[round].roundDataHolders[i].gObj;
        }

        dataInt = 0;
        lastBeat = 0;
        beatTimer = GameManager.me.timer + 15;
        beat.beatLimit = GameManager.me.timer + 15;
    }

    public void RoundReset()
    {
        for (int i = 0; i < beats.Length; i++)
        {
            dataInt = 0;
            beats[i] = 0;
            enemies[i] = null;
            nextBeat = 0;
        }
        round--;
        roundEnd = true;
    }

    public void RoundOver()
    {
        roundEnd = true;
    }
}   
