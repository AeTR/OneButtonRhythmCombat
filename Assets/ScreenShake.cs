using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake me;

    float ogPosx;
    float ogPosy;
    float ogPosz;
    public int shakeTimer;
  
    // Start is called before the first frame update
    void Awake()
    {
        me = this;
        ogPosx = transform.position.x;
        ogPosy = transform.position.y;
        ogPosz = transform.position.z;
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(shakeTimer > 0)
        {
            shakeTimer--;
            transform.position = Vector3.Lerp(transform.position, new Vector3(ogPosx, ogPosy, ogPosz) + (Random.insideUnitSphere * (shakeTimer / 5f)), 0.5f);
        }
        if(shakeTimer == 1)
        {
            transform.position = new Vector3(ogPosx, ogPosy,ogPosz);
        }
    }

    public void ScreenShakeFunc()
    {
        shakeTimer = 10;
    }
}
