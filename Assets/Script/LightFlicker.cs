using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Color blue;
    public Color green;
    Light light;
    float colorFloat;

    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        colorFloat = Random.Range(0f, 1f);
        light.color = Color.Lerp(light.color, blue + (green * colorFloat), 0.15f);
    }
}
