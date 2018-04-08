using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    float deltaTime = 0;
    float msec = 0;
    float fps = 0;

    void Update ()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        msec = deltaTime * 1000.0f;
        fps = Mathf.Round(1.0f / deltaTime);
    }

    public float GetMilliseconds()
    {
        return msec;
    }

    public float GetFPS()
    {
        return fps;
    }
}
