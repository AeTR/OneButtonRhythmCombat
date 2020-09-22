using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FencerSwordController : MonoBehaviour
{
    public static FencerSwordController me;

    public bool space;
    public int spaceTimer;

    public Quaternion ogRot;

    public int dropTimer;
    public int resetTimer;

    public SpriteRenderer sr;

    public BoxCollider2D bc;
    public Transform playerBody;

    [Header("Sprites")]
    public Sprite rest;
    public Sprite stab;
    public Sprite down;
    public Sprite up;
    public Sprite guard;

    public List<GameObject> killList;
    public float[] distances;
    public float smallestDis;

    
    void Awake()
    {
        me = this;
        ogRot = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.K))
        {
            space = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.K))
        {
            space = false;
            ExecuteSpace();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(resetTimer < 0)
        {
            resetTimer++;           
        }
        else
        {
            transform.rotation = ogRot;
            sr.sprite = rest;
            bc.enabled = false;
        }

        if(space)
        {
            spaceTimer++;
        }

        if(spaceTimer >= dropTimer)
        {
            sr.sprite = down;
        }


        //if (killList.Count > 1)
        //{
        //    bc.enabled = false; 
        //
        //    for (int i = 0; i < killList.Count - 1; i++)
        //    {
        //        distances[i] = Vector2.Distance(playerBody.position, killList[i].gameObject.transform.position);
        //        smallestDis = Mathf.Min(smallestDis, distances[i]);
        //    }
        //
        //    for (int i = 0; i < killList.Count; i++)
        //    {
        //        if (smallestDis == Vector2.Distance(playerBody.position, killList[i].gameObject.transform.position))
        //        {
        //            transform.LookAt(new Vector2(transform.position.x, killList[i].gameObject.transform.position.y));
        //            if (killList[i].gameObject.name == "Dog(Clone)")
        //            {
        //                killList[i].gameObject.GetComponent<Dog>().Kill();
        //            }
        //        }
        //    }
        //
        //    smallestDis = Mathf.Infinity;
        //    killList.Clear();
        //}

    }

    void ExecuteSpace()
    {
        if(spaceTimer < dropTimer)
        {
            sr.sprite = stab;
            bc.enabled = true;
            bc.tag = "Kill";
        }
        else
        {
            sr.sprite = up;
            bc.enabled = true;
            bc.tag = "Break";
        }

        spaceTimer = 0;
        resetTimer = -3;
    }

    public void Guard()
    {
        sr.sprite = guard;
        resetTimer = -8;
    }
    
}
