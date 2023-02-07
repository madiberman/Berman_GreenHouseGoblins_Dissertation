using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SustainabilityBar : MonoBehaviour
{
    public GM gameManager;
    //refers to the slider bar that is used to track sustainability points
    public Slider sustainabilityBar;

    public void Start()
    {
        //get the PlayerPrefs value of the number of sustainability points
        if (PlayerPrefs.HasKey("sustainability"))
        {
            //updates the sustainability bar to match the saved value
            sustainabilityBar.value = PlayerPrefs.GetFloat("sustainability");
        }
        else
        {
            //if there is no PlayerPref value, it sets the bar value to 0
            sustainabilityBar.value = 0;
            PlayerPrefs.SetFloat("hunger", sustainabilityBar.value);
        }
    }

    //allows change of slider bar value total by adding the parameter susty
    public void addSustainability(int susty)
    {
        sustainabilityBar.value += susty;
        PlayerPrefs.SetFloat("sustainability", sustainabilityBar.value);
    }
}