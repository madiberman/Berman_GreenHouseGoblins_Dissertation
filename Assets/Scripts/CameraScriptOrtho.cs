using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScriptOrtho : MonoBehaviour
{
    // sprite renderer attached to transparent GameObject in Unity, used to size the camera
    public SpriteRenderer tileMap;

    void Start()
    {
        //get the orthographic size of the x of the tileMap sprite renderer
        //multiply this by the screen's heigh divided by the width multiplied by 2
        float orthoSize = (float)tileMap.bounds.size.x * (float)Screen.height / (float)Screen.width * 0.5f;
        //set the orthographic size to the orthoSize calculated so that the camera alwasy shows the entire width of the apartment
        //helps render the apartment appropriately on different screen sizes
        Camera.main.orthographicSize = orthoSize;
    }

}
