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
    public bool roundActive; //True if more enemies need to be spawned
    public bool roundEnd; //True while enemies are alive
    public int round; //Current round
    public int lastBeat;

    public int[] beats;
    public GameObject[] enemies;
    public int dataInt;
    
    public int beatTimer; //Amount of frames between beats
    public int nextBeat; //Current beat

    public List<GameObject> liveEnemies; //List of all spawned, alive enemies

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
        if(FencerSwordController.me.space && roundEnd && GameManager.me.timer > GameManager.me.timerLimit && round != 10) //IF all enemies are dead and the player presses space, advance the round
        {
            round++;
            roundActive = true;
            roundEnd = false;
            RoundDataCollect();
        }

        if(GameManager.me.PlayerDead == true) //Stop spawning enemies
        {
            roundActive = false;
        }

        roundCounter.text = "" + round;
    }

    void FixedUpdate()
    {

        if (round > -1 && roundActive)
        {
            if (GameManager.me.timer == beatTimer) //"Beats" occur every 15 frames
            {
                beatTimer = GameManager.me.timer + 15;
                nextBeat++;
            }

            if(nextBeat == beats[dataInt]) //If the current beat = the beat the next enemy is meant to spawn on
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

                dataInt++; //Moves focus to next enemy

                if (beats[dataInt] == 0) //If there are no more enemies
                {
                    Debug.Log("Hit end of round");
                    for (int i = 0; i < beats.Length; i++) //Clear round data
                    {
                        dataInt = 0;
                        beats[i] = 0;
                        enemies[i] = null;
                        nextBeat = 0;
                    }

                    roundActive = false; //End Round
                }
            }
        }

        if(!roundActive && !roundEnd) //If no more enemies need to be spawned, and all enemies are dead, end the round
        {
            if (liveEnemies.Count == 0)
            {
                roundEnd = true;
            }
        }


    }

    void RoundDataCollect() //Function goes to Round Game objects, taking which enemies to spawn on which beats
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
