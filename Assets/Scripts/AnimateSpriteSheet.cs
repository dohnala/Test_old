﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://wiki.unity3d.com/index.php?title=Animating_Tiled_texture

public class AnimateSpriteSheet : MonoBehaviour
{
    public int Columns = 5;
    public int Rows = 5;
    public float FramesPerSecond = 10f;
    public bool RunOnce = true;
    public bool DestroyAtAnimationEnd;

    public float RunTimeInSeconds
    {
        get
        {
            return ((1f / FramesPerSecond) * (Columns * Rows));
        }
    }

    private Material materialCopy = null;

    void Awake()
    {
        // Copy its material to itself in order to create an instance not connected to any other
        materialCopy = new Material(GetComponent<Renderer>().sharedMaterial);
        GetComponent<Renderer>().sharedMaterial = materialCopy;

        Vector2 size = new Vector2(1f / Columns, 1f / Rows);
        GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", size);
    }

    void OnEnable()
    {
        StartCoroutine(UpdateTiling());
    }

    private IEnumerator UpdateTiling()
    {
        float x = 0f;
        float y = 0f;
        Vector2 offset = Vector2.zero;

        while (true)
        {
            for (int i = Rows - 1; i >= 0; i--) // y
            {
                y = (float)i / Rows;

                for (int j = 0; j <= Columns - 1; j++) // x
                {
                    x = (float)j / Columns;

                    offset.Set(x, y);

                    GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
                    yield return new WaitForSeconds(1f / FramesPerSecond);
                }
            }

            if (RunOnce)
            {
                if (DestroyAtAnimationEnd)
                {
                    Destroy(gameObject);
                }
                yield break;
            }
        }
    }

}
