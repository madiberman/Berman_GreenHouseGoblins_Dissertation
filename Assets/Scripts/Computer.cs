using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Computer : MonoBehaviour, IPointerClickHandler
{
    public Canvas computerCanvas;
    public Canvas topLeft;
    public Canvas topRight;
    public Canvas xButtonCanvas;
    public GM gameManager;

    //when the computer is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        //close all other canvases that are currently open
        computerCanvas.gameObject.SetActive(true);
        topLeft.gameObject.SetActive(false);
        topRight.gameObject.SetActive(false);
        xButtonCanvas.gameObject.SetActive(true);
        //play the computer sound
        gameManager.soundManager.playComputer();
    }

}
