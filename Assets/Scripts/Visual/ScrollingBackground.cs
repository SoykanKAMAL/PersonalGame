using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public Material material;
    public Vector2 scrollSpeed;
    void Start()
    {
        material.mainTextureOffset = new Vector2(-1f,-1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(material.mainTextureOffset.x > 1)
        {
            material.mainTextureOffset = new Vector2(-1f, -1f);
        }
        material.mainTextureOffset += scrollSpeed * Time.deltaTime;
    }
}
