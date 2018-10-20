using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tv : MonoBehaviour
{

    public Light spotlight;
    private Color currentColor;
    private Color startColor;
    private Color endColor;
    private Color[] colors;
    private int colorsSize = 4;

    private float scale = 1;
    private float smoothness = .1f, duration = .3f;
    // Use this for initialization
    void Start()
    {
        currentColor = Color.red;
        startColor = Color.red;
        endColor = Color.magenta;

        colors = new Color[4];
        colors[0] = Color.red;
        colors[1] = Color.magenta;
        colors[2] = Color.blue;
        colors[3] = Color.green;

        StartCoroutine("LerpColor");
    }

    // Update is called once per frame
    void Update()
    {
        spotlight.color = currentColor;
        float sample = scale * Mathf.PerlinNoise(Time.time * scale, 0.0F);
        spotlight.spotAngle = spotlight.spotAngle + sample;
    }

    IEnumerator LerpColor()
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        int index = 0;
        while (true){
            progress = 0;
            while (progress < 1)
            {
                currentColor = Color.Lerp(startColor, endColor, progress);
                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
            startColor = colors[index];
            index = (index + 1) % colorsSize;
            endColor = colors[index];
        }
    }
}