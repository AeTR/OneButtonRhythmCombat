using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treads : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite[] sprites;
    public int sprLimit;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(sprLimit < GameManager.me.timer)
        {
            if(sr.sprite = sprites[0])
            {
                sr.sprite = sprites[1];
            }
            else if (sr.sprite = sprites[1])
            {
                sr.sprite = sprites[0];
            }
            sprLimit = GameManager.me.timer + 15;
        }
    }
}
