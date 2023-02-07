using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingEffect : MonoBehaviour
{
    //used for computer
    Renderer ren;
    //used for email button
    public Image image;
    bool flashBool;
    float nextFlashTime = 0;
    float flashInterval = 0.5f;
    public GM gameManager;
    public bool isComputer;
    bool computerFlash;
    public Canvas computerScreen;
    public bool isEmail;
    public bool isUnlockable;
    public bool isEmailFlashing;

    private void Awake()
    {
        //get the renderer of the object the script is attached to
        ren = GetComponent<Renderer>();
        computerFlash = true;
    }

    public void setComputerFlash(bool CFE)
    {
        computerFlash = CFE;
    }

    private void Update()
    {
        //if the object the script is attached to isComputer == true
        if (isComputer)
        {
            //if the game mamanger's flashGo bool has been set to true and the computer is not opened
            if (gameManager.flashGo == true && !computerScreen.isActiveAndEnabled)
            {
                //the computer will flash between red and white
                //if nextFlashTime has passed and flashBool is true
                if (flashBool == true && Time.time >= nextFlashTime)
                {
                    //flash red
                    ren.material.color = Color.red;
                    //set flashBool to false
                    flashBool = false;
                    //update nextFlashTime
                    nextFlashTime = Time.time + flashInterval;
                }
                //if nextFlashTime has passed and flashBool is false
                else if (flashBool == false && Time.time >= nextFlashTime)
                {
                    //flash white
                    ren.material.color = Color.white;
                    //set flashBool to true
                    flashBool = true;
                    //update nextFlashTime
                    nextFlashTime = Time.time + flashInterval;
                }
            }

            //if the computer screen is open
            if (computerScreen.isActiveAndEnabled)
            {
                //the computer should remain white and not flash
                ren.material.color = Color.white;
            }
        }

        //if the object isEmail == true and isEmailFlashing == true
        else if (gameManager.flashGo == true && isEmail && isEmailFlashing)
        {
                if (flashBool == true && Time.time >= nextFlashTime)
                {
                    image.color = Color.red;
                    flashBool = false;
                    nextFlashTime = Time.time + flashInterval;
                }
                else if (flashBool == false && Time.time >= nextFlashTime)
                {
                    image.color = Color.white;
                    flashBool = true;
                    nextFlashTime = Time.time + flashInterval;
                }
        }
        //if the object isUnlockable == true
        else if (isUnlockable && gameManager.unlockableFlashGo == true)
        {
            if (flashBool == true && Time.time >= nextFlashTime)
            {
                image.color = Color.red;
                flashBool = false;
                nextFlashTime = Time.time + flashInterval;
            }
            else if (flashBool == false && Time.time >= nextFlashTime)
            {
                image.color = Color.white;
                flashBool = true;
                nextFlashTime = Time.time + flashInterval;
            }
        }
    }

    //when the object is clicked
    public void onClickedButton()
    {
        if (isEmail)
        {
            //only turn off the flashGo once the email button is clicked meaning that the email has been read
            gameManager.flashGo = false;
        }
        if (isUnlockable)
        {
            gameManager.unlockableFlashGo = false;
        }

        //turn colors back to white once flashing is done
        if (isComputer)
        {
            ren.material.color = Color.white;
        }
        else
        {
            image.color = Color.white;
        }
    }
}
