using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public GM gameManager;
    //slider that tracks the player's hunger
    public Slider hungerBar;
    float timeWhenNextDecrease;
    float currentTime;
    float T = 0;
    float U = 15;

    public void Start()
    {
        //set hunger values based on PlayerPrefs
        if (PlayerPrefs.HasKey("hunger"))
        {
            hungerBar.value = PlayerPrefs.GetFloat("hunger");
        }
        else
        {
            hungerBar.value = 100f;
            PlayerPrefs.SetFloat("hunger", hungerBar.value);
        }

        //time when hunger will next be reduced is the time of game start plus 3 minutes
        timeWhenNextDecrease = Time.time + 180;
    }

    public void Update()
    {
        //on update if the game manager is not currently updating its hunger value
        if (gameManager.hungerUpdated == false)
        {
            //adjust the hunger value based on updates in the game manager
            hungerBar.value -= gameManager.hunger;
            PlayerPrefs.SetFloat("hunger", hungerBar.value);
            gameManager.hungerUpdated = true;
        }

        //the the time since opening the app is greater than the next hunger decrease time 
        if (Time.time >= timeWhenNextDecrease)
        {
            //remove 20 points from the hunger
            hungerBar.value -= 20;
            PlayerPrefs.SetFloat("hunger", hungerBar.value);
            //change the time of next decrease to the current time since opening the app plus 180 seconds
            timeWhenNextDecrease = Time.time + 180f;
        }

        //if the hunger bar is empty
        if (hungerBar.value == 0)
        {
            gameManager.hungerEmpty = true;
        }
        else
        {
            gameManager.hungerEmpty = false;
        }

        //if the hunger bar is empty
        if (hungerBar.value == 0)
        {
            //play the hunger sound every 15 seconds
            if (T < Time.time)
            {
                gameManager.soundManager.playHungry();
                T = Time.time + U;
            }
        }
    }

    public void setHunger(float hunger)
    {
        //sent the value of the hunger bar slider to the parameter hunger
        hungerBar.value = hunger;
        PlayerPrefs.SetFloat("hunger", hungerBar.value);
    }
}
