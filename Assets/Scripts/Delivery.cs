using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Delivery : MonoBehaviour
{
    public GM gameManager;

    public Sprite prefabCarrots;
    public Sprite prefabStrawberries;
    public Sprite prefabAubergine;
    public Sprite prefabEggs;
    public Sprite prefabMeat;
    public Sprite prefabPumpkin;
    public Sprite prefabNoodles;
    public Sprite prefabShrimp;

    public Sprite prefabLaundry;
    public Sprite prefabDishwasher;
    public Sprite prefabSolarpanels;

    public Sprite prefabClock;
    public Sprite prefabPainting;
    public Sprite prefabPoster;
    public Sprite prefabMap;
    public Sprite prefabLamp1;
    public Sprite prefabLamp2;
    public Sprite prefabLamp3;
    public Sprite prefabLamp4;
    public Sprite prefabChair1;
    public Sprite prefabChair2;
    public Sprite prefabChair3;
    public Sprite prefabJukebox;
    public Sprite prefabShelves1;
    public Sprite prefabShelves2;
    public Sprite prefabShelves3;
    public Sprite prefabBookCollection;
    public Sprite prefabFish;
    public Sprite prefabShelfDecor;
    public Sprite prefabBooks;
    public Sprite prefabDeskPlant;
    public Sprite prefabTallPlant;
    public Sprite prefabFlowers;
    public Sprite prefabSucculent;
    public Sprite prefabVase;

    public Canvas deliveryAlert;
    public GameObject deliveryProduct;

    private void Update ()
    {
        //set delivery canvas to active
        deliveryAlert.gameObject.SetActive(true);

        //get the image on the delivery canvas
        Image oldImage = deliveryProduct.GetComponent<Image>();

        //if the delivery is from allmart, pull the string of current delivery from the allmartBeingDelivered list
        if (gameManager.allmartDelivery == true)
        {
            //set the image on the delivery canvas according to the string in allmartBeingDelivered[0]

            //food
            if (gameManager.allmartBeingDelivered[0] == "Strawberries")
            {
                oldImage.sprite = prefabStrawberries;
            }
            if (gameManager.allmartBeingDelivered[0] == "Noodles")
            {
                oldImage.sprite = prefabNoodles;
            }
            if (gameManager.allmartBeingDelivered[0] == "Carrots")
            {
                oldImage.sprite = prefabCarrots;
            }
            if (gameManager.allmartBeingDelivered[0] == "Aubergine")
            {
                oldImage.sprite = prefabAubergine;
            }
            if (gameManager.allmartBeingDelivered[0] == "Eggs")
            {
                oldImage.sprite = prefabEggs;
            }
            if (gameManager.allmartBeingDelivered[0] == "Meat")
            {
                oldImage.sprite = prefabMeat;
            }
            if (gameManager.allmartBeingDelivered[0] == "Pumpkin")
            {
                oldImage.sprite = prefabPumpkin;
            }
            if (gameManager.allmartBeingDelivered[0] == "Shrimp")
            {
                oldImage.sprite = prefabShrimp;
            }

            //Appliances

            if (gameManager.allmartBeingDelivered[0] == "Laundry")
            {
                oldImage.sprite = prefabLaundry;
                oldImage.color = new Color32(218, 255, 191, 255);
            }
            if (gameManager.allmartBeingDelivered[0] == "Dishwasher")
            {
                oldImage.sprite = prefabDishwasher;
                oldImage.color = new Color32(207, 255, 198, 255);
            }
            if (gameManager.allmartBeingDelivered[0] == "Solarpanels")
            {
                oldImage.sprite = prefabSolarpanels;
            }

            //furniture

            if (gameManager.allmartBeingDelivered[0] == "Clock")
            {
                oldImage.sprite = prefabClock;
            }
            if (gameManager.allmartBeingDelivered[0] == "Painting")
            {
                oldImage.sprite = prefabPainting;
            }
            if (gameManager.allmartBeingDelivered[0] == "Poster")
            {
                oldImage.sprite = prefabPoster;
            }
            if (gameManager.allmartBeingDelivered[0] == "Map")
            {
                oldImage.sprite = prefabMap;
            }
            if (gameManager.allmartBeingDelivered[0] == "Lamp1")
            {
                oldImage.sprite = prefabLamp1;
            }
            if (gameManager.allmartBeingDelivered[0] == "Lamp2")
            {
                oldImage.sprite = prefabLamp2;
            }
            if (gameManager.allmartBeingDelivered[0] == "Lamp3")
            {
                oldImage.sprite = prefabLamp3;
            }
            if (gameManager.allmartBeingDelivered[0] == "Lamp4")
            {
                oldImage.sprite = prefabLamp4;
            }
            if (gameManager.allmartBeingDelivered[0] == "Chair1")
            {
                oldImage.sprite = prefabChair1;
            }
            if (gameManager.allmartBeingDelivered[0] == "Chair2")
            {
                oldImage.sprite = prefabChair2;
            }
            if (gameManager.allmartBeingDelivered[0] == "Chair3")
            {
                oldImage.sprite = prefabChair3;
            }
            if (gameManager.allmartBeingDelivered[0] == "Jukebox")
            {
                oldImage.sprite = prefabJukebox;
            }
            if (gameManager.allmartBeingDelivered[0] == "Shelves1")
            {
                oldImage.sprite = prefabShelves1;
            }
            if (gameManager.allmartBeingDelivered[0] == "Shelves2")
            {
                oldImage.sprite = prefabShelves2;
            }
            if (gameManager.allmartBeingDelivered[0] == "Shelves3")
            {
                oldImage.sprite = prefabShelves3;
            }
            if (gameManager.allmartBeingDelivered[0] == "BookCollection")
            {
                oldImage.sprite = prefabBookCollection;
            }
            if (gameManager.allmartBeingDelivered[0] == "Fish")
            {
                oldImage.sprite = prefabFish;
            }
            if (gameManager.allmartBeingDelivered[0] == "ShelfDecor")
            {
                oldImage.sprite = prefabShelfDecor;
            }
            if (gameManager.allmartBeingDelivered[0] == "DeskPlant")
            {
                oldImage.sprite = prefabDeskPlant;
            }
            if (gameManager.allmartBeingDelivered[0] == "Books")
            {
                oldImage.sprite = prefabBooks;
            }
            if (gameManager.allmartBeingDelivered[0] == "TallPlant")
            {
                oldImage.sprite = prefabTallPlant;
            }
            if (gameManager.allmartBeingDelivered[0] == "Vase")
            {
                oldImage.sprite = prefabVase;
            }
            if (gameManager.allmartBeingDelivered[0] == "Flowers")
            {
                oldImage.sprite = prefabFlowers;
            }
            if (gameManager.allmartBeingDelivered[0] == "Succulent")
            {
                oldImage.sprite = prefabSucculent;
            }
        }
        //if the delivery is from Ma&Pop, pull the string of current delivery from the localFoodBeingDelivered list
        else if (gameManager.allmartDelivery == false)
        {
            //set the image on the delivery canvas according to the string in localFoodBeingDelivered[0]
            //food
            if (gameManager.localFoodBeingDelivered[0] == "Strawberries")
            {
                oldImage.sprite = prefabStrawberries;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Noodles")
            {
                oldImage.sprite = prefabNoodles;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Carrots")
            {
                oldImage.sprite = prefabCarrots;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Aubergine")
            {
                oldImage.sprite = prefabAubergine;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Eggs")
            {
                oldImage.sprite = prefabEggs;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Meat")
            {
                oldImage.sprite = prefabMeat;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Pumpkin")
            {
                oldImage.sprite = prefabPumpkin;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Shrimp")
            {
                oldImage.sprite = prefabShrimp;
            }

            //Appliances

            if (gameManager.localFoodBeingDelivered[0] == "Laundry")
            {
                oldImage.sprite = prefabLaundry;
                oldImage.color = new Color32(218, 255, 191, 255);
            }
            if (gameManager.localFoodBeingDelivered[0] == "Dishwasher")
            {
                oldImage.sprite = prefabDishwasher;
                oldImage.color = new Color32(207, 255, 198, 255);
            }
            if (gameManager.localFoodBeingDelivered[0] == "Solarpanels")
            {
                oldImage.sprite = prefabSolarpanels;
            }

            //furniture

            if (gameManager.localFoodBeingDelivered[0] == "Clock")
            {
                oldImage.sprite = prefabClock;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Painting")
            {
                oldImage.sprite = prefabPainting;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Poster")
            {
                oldImage.sprite = prefabPoster;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Map")
            {
                oldImage.sprite = prefabMap;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Lamp1")
            {
                oldImage.sprite = prefabLamp1;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Lamp2")
            {
                oldImage.sprite = prefabLamp2;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Lamp3")
            {
                oldImage.sprite = prefabLamp3;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Lamp4")
            {
                oldImage.sprite = prefabLamp4;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Chair1")
            {
                oldImage.sprite = prefabChair1;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Chair2")
            {
                oldImage.sprite = prefabChair2;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Chair3")
            {
                oldImage.sprite = prefabChair3;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Jukebox")
            {
                oldImage.sprite = prefabJukebox;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Shelves1")
            {
                oldImage.sprite = prefabShelves1;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Shelves2")
            {
                oldImage.sprite = prefabShelves2;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Shelves3")
            {
                oldImage.sprite = prefabShelves3;
            }
            if (gameManager.localFoodBeingDelivered[0] == "BookCollection")
            {
                oldImage.sprite = prefabBookCollection;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Fish")
            {
                oldImage.sprite = prefabFish;
            }
            if (gameManager.localFoodBeingDelivered[0] == "ShelfDecor")
            {
                oldImage.sprite = prefabShelfDecor;
            }
            if (gameManager.localFoodBeingDelivered[0] == "DeskPlant")
            {
                oldImage.sprite = prefabDeskPlant;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Books")
            {
                oldImage.sprite = prefabBooks;
            }
            if (gameManager.localFoodBeingDelivered[0] == "TallPlant")
            {
                oldImage.sprite = prefabTallPlant;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Vase")
            {
                oldImage.sprite = prefabVase;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Flowers")
            {
                oldImage.sprite = prefabFlowers;
            }
            if (gameManager.localFoodBeingDelivered[0] == "Succulent")
            {
                oldImage.sprite = prefabSucculent;
            }
        }

    }
}