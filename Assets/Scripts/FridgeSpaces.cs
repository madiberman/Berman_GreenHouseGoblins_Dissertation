using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class FridgeSpaces : MonoBehaviour
{
    public Canvas errorCanvas;
    public Fridge fridge;
    public GM gameManager;
    public TextMeshProUGUI errorMessage;
    public HungerBar hunger;
    public int spaceNumber;

    //when the mouse is clicked
    public void OnMouseDown()
    {
        //check if the space number of the fridge space contains an item
        if (gameManager.foodInFridge.Count > spaceNumber)
        {
            //if the hunger bar is not already full
            if (hunger.hungerBar.value < 100)
            {
                //add 20 points to hunger slider
                hunger.setHunger(hunger.hungerBar.value + 20);
                PlayerPrefs.SetFloat("hunger", hunger.hungerBar.value);
                //remove the food that was "eaten"
                gameManager.foodInFridge.RemoveAt(spaceNumber);
                //add an available fridge space
                gameManager.emptyFridgeSpaces += 1;
                PlayerPrefs.SetInt("emptyFridgeSpaces", gameManager.emptyFridgeSpaces);
            }
            //if the hunger bar is full
            else
            {
                //display an error message
                errorCanvas.gameObject.SetActive(true);
                errorMessage.text = "You're not hungry!";
            }
        }
    }
}
