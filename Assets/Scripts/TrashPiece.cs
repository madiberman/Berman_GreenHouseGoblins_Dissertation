using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashPiece : MonoBehaviour
{
    GM gameManager;

    //change the trash piece to the position of the mouse while the mouse is being dragged
    void OnMouseDrag()
    {
        transform.position = GetMousePos();
    }

    //returns the position of the mouse on screen
    Vector3 GetMousePos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    //when mouse is clicked, take the screen position of the mouse
    void OnMouseDown()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
