using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Fridge : MonoBehaviour, IPointerClickHandler
{
    public Canvas FridgeInterior;
    public Canvas inventory;
    public GM gameManager;
    public GameObject[] spaces;

    //images for each of the food types
    public Sprite prefabCarrots;
    public Sprite prefabStrawberries;
    public Sprite prefabAubergine;
    public Sprite prefabEggs;
    public Sprite prefabMeat;
    public Sprite prefabPumpkin;
    public Sprite prefabNoodles;
    public Sprite prefabShrimp;

    //when the fridge is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        //open the fridge canvas
        FridgeInterior.gameObject.SetActive(true);
        //close the invnentory canvas in case it is open
        inventory.gameObject.SetActive(false);
    }

    public void Start()
    {
        //populate the fridge spaces with the food in fridge strings that are saved to PlayerPrefs
        for (int i = 0; i < 6; i++)
        {
            if (PlayerPrefs.HasKey("foodSpace_" + i))
            {
                gameManager.foodInFridge.Add(PlayerPrefs.GetString("foodSpace_" + i));
            }
        }

    }

    void Update()
    {
        //make all images transparent
        for (int j = 0; j < 6; j++)
        {
            Image oldImage = spaces[j].GetComponent<Image>();
            oldImage.color = new Color32(255, 255, 255, 0);
        }

        if (gameManager.foodInFridge.Count > 0)
        {
            //for each string in the foodInFridge list
            for (int i = 0; i < gameManager.foodInFridge.Count; i++)
            {
                //get the image of the space
                Image oldImage = spaces[i].GetComponent<Image>();
                //make the image opaque
                oldImage.color = new Color32(255, 255, 255, 255);
                //set the associated fridge space to the image of the foodInFridge string
                if (gameManager.foodInFridge[i] == "Strawberries")
                    {
                        oldImage.sprite = prefabStrawberries;
                    }
                    if (gameManager.foodInFridge[i] == "Noodles")
                    {
                        oldImage.sprite = prefabNoodles;
                    }
                    if (gameManager.foodInFridge[i] == "Carrots")
                    {
                        oldImage.sprite = prefabCarrots;
                    }
                    if (gameManager.foodInFridge[i] == "Aubergine")
                    {
                        oldImage.sprite = prefabAubergine;
                    }
                    if (gameManager.foodInFridge[i] == "Eggs")
                    {
                        oldImage.sprite = prefabEggs;
                    }
                    if (gameManager.foodInFridge[i] == "Meat")
                    {
                        oldImage.sprite = prefabMeat;
                    }
                    if (gameManager.foodInFridge[i] == "Pumpkin")
                    {
                        oldImage.sprite = prefabPumpkin;
                    }
                    if (gameManager.foodInFridge[i] == "Shrimp")
                    {
                        oldImage.sprite = prefabShrimp;
                    }
            }
        }
    }
}
