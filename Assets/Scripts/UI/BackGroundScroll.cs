using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundScroll : MonoBehaviour
{
    [SerializeField] private RawImage[] backgroundImage;

    private Vector2 scrollSpeed = new Vector2(5f,0f);


    // Update is called once per frame
    void Update()
    {

        Rect uv = backgroundImage[0].uvRect;

        uv.x += scrollSpeed.x * Time.deltaTime;
        uv.y += scrollSpeed.y * Time.deltaTime;

        uv.x = uv.x % 1; // UV 좌표를 0-1 범위로 제한
        uv.y = uv.y % 1; // UV 좌표를 0-1 범위로 제한
        
        backgroundImage[0].uvRect = uv;
        Debug.Log("Background Scrolling: " + uv.x + ", " + uv.y);
    }
}
