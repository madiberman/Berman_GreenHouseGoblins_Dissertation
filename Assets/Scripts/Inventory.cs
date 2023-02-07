using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Canvas inventoryCanvas;
    public Canvas errorCanvas;
    public GM gameManager;
    //each inventory space is saved into this array in Unity
    public GameObject[] inventorySpaces;
    //all inventory spaces being donated
    public List<int> inventoryItemsBeingDonated;
    //all times that inventory spaces waiting for donation pick-up will be donated
    public List<string> listOfDonationTimes;
    //donation time to be added to listOfDonationTimes
    string donationTimeS;
    public TextMeshProUGUI errorMessage;
    //if the player has already donated an item = 1, else = 0 (for email purposes)
    int donatedPreviously;
    //if the player has already trashed an item = 1, else = 0 (for email purposes)
    int trashedPreviously;

    bool shelves1Owned = false;
    bool shelves2Owned = false;
    bool shelves3Owned = false;

    //the sprites for each type of item that can be held in the inventory
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

    //the transparent gameobject of each item that, when the item == "placed", will become opaque
    public GameObject Clock;
    public GameObject Painting;
    public GameObject Poster;
    public GameObject Map;
    public GameObject Lamp1;
    public GameObject Lamp2;
    public GameObject Lamp3;
    public GameObject Lamp4;
    public GameObject Chair1;
    public GameObject Chair2;
    public GameObject Chair3;
    public GameObject Jukebox;
    public GameObject Shelves1;
    public GameObject Shelves2;
    public GameObject Shelves3;
    public GameObject BookCollection;
    public GameObject Fish;
    public GameObject ShelfDecor;
    public GameObject Books;
    public GameObject DeskPlant;
    public GameObject TallPlant;
    public GameObject Flowers;
    public GameObject Succulent;
    public GameObject Vase;

    private void Start()
    {
        //retrieve PlayerPrefs
        if (PlayerPrefs.HasKey("donatedPreviously"))
        {
            if (PlayerPrefs.GetInt("donatedPreviously") == 1)
            {
                donatedPreviously = 1;
            }
        }
        else
        {
            donatedPreviously = 0;
        }

        if (PlayerPrefs.HasKey("trashedPreviously"))
        {
            if (PlayerPrefs.GetInt("trashedPreviously") == 1)
            {
                trashedPreviously = 1;
            }
        }
        else
        {
            trashedPreviously = 0;
        }

        if (PlayerPrefs.HasKey("shelves1Owned"))
        {
            if (PlayerPrefs.GetInt("shelves1Owned") == 1)
            {
                shelves1Owned = true;
            }
        }
        if (PlayerPrefs.HasKey("shelves2Owned"))
        {
            if (PlayerPrefs.GetInt("shelves2Owned") == 1)
            {
                shelves2Owned = true;
            }
        }
        if (PlayerPrefs.HasKey("shelves3Owned"))
        {
            if (PlayerPrefs.GetInt("shelves3Owned") == 1)
            {
                shelves3Owned = true;
            }
        }

        //retrieve the inventory items that are waiting to be donated and the times they will be deleted from the inventory
        for (int i = 0; i < gameManager.inventorySpace; i++)
        {
            if (PlayerPrefs.HasKey("inventoryItemsBeingDonated_" + i))
            {
                inventoryItemsBeingDonated.Add(PlayerPrefs.GetInt("inventoryItemsBeingDonated_" + i));
                //image of the item being donated set to 50% transparancy
                Image oldImage = inventorySpaces[PlayerPrefs.GetInt("inventoryItemsBeingDonated_" + i)].GetComponent<Image>();
                oldImage.color = new Color32(255, 255, 255, 150);
            }
            if (PlayerPrefs.HasKey("donationTimers_" + listOfDonationTimes.Count))
            {
                listOfDonationTimes.Add(PlayerPrefs.GetString("donationTimers_" + i));
            }
        }
    }

    //run when player chooses to trash an inventory item
    public void trashInventoryItem()
    {
        //if the player has never trashed an item before
        if (!PlayerPrefs.HasKey("trashedPreviously"))
        {
            trashedPreviously = 1;
            PlayerPrefs.SetInt("trashedPreviously", 1);
            //set the email to 7
            gameManager.email = 7;
            //email sound plys
            gameManager.soundManager.playEmail();
        }
        //take away 3 sustainability points from the player
        gameManager.sustainabilityBar.addSustainability(-3);
        gameManager.inventorySpace += 1;
        //remove the item from the inventory list
        gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);

        for (int i = 0; i < inventoryItemsBeingDonated.Count; i++)
        {
            //if the item being donated is at a higher inventory slot than the item being donated
            if (gameManager.currentInventorySlot < inventoryItemsBeingDonated[i])
            {
                //move inventory slot moved down one to account for deleted items
                inventoryItemsBeingDonated[i] -= 1;
            }
        }

        //adjust list of inventory items in PlayerPrefs by deleting all and recreating
        for (int j = 0; j < inventorySpaces.Length; j++)
        {
            PlayerPrefs.DeleteKey("inventory_" + j);
        }
        for (int i = 0; i < gameManager.inventory.Count; i++)
        {
            PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
        }
    }

    //run when donation time[0] is less than or equal to current time
    public void deleteDonatedInventoryItem()
    {
        //increase available inventory spaces
        gameManager.inventorySpace += 1;
        //remove the donated item from the inventory
        gameManager.inventory.RemoveAt(inventoryItemsBeingDonated[0]);

        //remove the item slot number from the inventoryItemsBeingDonated list
        inventoryItemsBeingDonated.RemoveAt(0);
        //remove the time of the donation from the listOfDonationTimes list
        listOfDonationTimes.RemoveAt(0);

        //delete all items being donated and donation times from PlayerPrefs
        for (int i = 0; i < gameManager.inventorySpace; i++)
        {
            if (PlayerPrefs.HasKey("inventoryItemsBeingDonated_" + i))
            {
                PlayerPrefs.DeleteKey("inventoryItemsBeingDonated_" + i);
            }
            if (PlayerPrefs.HasKey("donationTimers_" + i))
            {
                PlayerPrefs.DeleteKey("donationTimers_" + i);
            }
        }

        for (int i = 0; i < inventoryItemsBeingDonated.Count; i++)
        {
            //if the item being donated is at a higher inventory slot than the item being donated
            if (gameManager.currentInventorySlot < inventoryItemsBeingDonated[i])
            {
                //move inventory slot moved down one to account for deleted items
                inventoryItemsBeingDonated[i] -= 1;
            }

            //add back all itmes waiting for donation and donation times to PlayerPrefs
            PlayerPrefs.SetInt("inventoryItemsBeingDonated_" + i, inventoryItemsBeingDonated[i]);
            PlayerPrefs.SetString("donationTimers_" + i, listOfDonationTimes[i]);
        }

        //adjust list of inventory items in PlayerPrefs by deleting all and recreating
        for (int j = 0; j < inventorySpaces.Length; j++)
        {
            PlayerPrefs.DeleteKey("inventory_" + j);
        }
        for (int i = 0; i < gameManager.inventory.Count; i++)
        {
            PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
        }

    }

    //run when a player chooses to donate an item in their inventory
    public void donateInventoryItem()
    {
        //if the player has never donated an item before
        if (!PlayerPrefs.HasKey("donatedPreviously"))
        {
            trashedPreviously = 1;
            PlayerPrefs.SetInt("donatedPreviously", 1);
            //set email to 8
            gameManager.email = 8;
            //play the email sound
            gameManager.soundManager.playEmail();
        }

        //give 3 sustainability points to the player
        gameManager.sustainabilityBar.addSustainability(3);
        //add the item to the inventoryItemsBeingDonated list
        inventoryItemsBeingDonated.Add(gameManager.currentInventorySlot);

        //add item being donated to PlayerPrefs list
        PlayerPrefs.SetInt("inventoryItemsBeingDonated_" + (inventoryItemsBeingDonated.Count - 1), gameManager.currentInventorySlot);
        //add donation time to donationTimeS list
        donationTimeS = DateTime.Now.AddSeconds(86400).ToString();
        //add donation time to PlayerPrefs list
        PlayerPrefs.SetString("donationTimers_" + listOfDonationTimes.Count, donationTimeS.ToString());
        //add donation time to PlayerPrefs list
        listOfDonationTimes.Add(donationTimeS);
    }


    //run when the player chooses to place an item from their inventory in their home
    public void placeInventoryItem()
    {
        //add item being placed to PlayerPrefs list so that they can be placed each time the game is opened
        PlayerPrefs.SetString("placedInventoryItems_" + gameManager.placedInventoryItems.Count.ToString(), gameManager.currentInventoryItem);
        //add to the game manager placedInventoryItems list
        gameManager.placedInventoryItems.Add(gameManager.currentInventoryItem);

        //for each possible item being placed, check the string of the item the player is trying to place
        if (gameManager.currentInventoryItem == "Clock")
        {
            //make the image of the invisible game object of whatever is being place fully opaque
            SpriteRenderer oldImage = Clock.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);

            //if the gameManager is not currently placing items from PlayerPrefs during Start()
            if (gameManager.placingItems == false)
            {
                //remove the placed item from the inventory
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);

                //add inventory space available
                gameManager.inventorySpace += 1;

                //adjust the inventory items PlayerPrefs list by deleting all "inventory_" keys and recreating the list
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }

        if (gameManager.currentInventoryItem == "Painting")
        {
            SpriteRenderer oldImage = Painting.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Poster")
        {
            SpriteRenderer oldImage = Poster.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Map")
        {
            SpriteRenderer oldImage = Map.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Lamp1")
        {
            SpriteRenderer oldImage = Lamp1.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Lamp2")
        {
            SpriteRenderer oldImage = Lamp2.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Lamp3")
        {
            SpriteRenderer oldImage = Lamp3.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Lamp4")
        {
            SpriteRenderer oldImage = Lamp4.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Chair1")
        {
            SpriteRenderer oldImage = Chair1.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Chair2")
        {
            SpriteRenderer oldImage = Chair2.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Chair3")
        {
            SpriteRenderer oldImage = Chair3.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Jukebox")
        {
            SpriteRenderer oldImage = Jukebox.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Shelves1")
        {
            SpriteRenderer oldImage = Shelves1.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            //for all types of shelves, set the bool of shelves_Owned to true
            //this bool is used in determining whether certain other items can be placed, as some need shelves to be placed
            shelves1Owned = true;
            PlayerPrefs.SetInt("shelves1Owned", 1);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Shelves2")
        {
            SpriteRenderer oldImage = Shelves2.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            shelves2Owned = true;
            PlayerPrefs.SetInt("shelves2Owned", 1);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Shelves3")
        {
            SpriteRenderer oldImage = Shelves3.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            shelves3Owned = true;
            PlayerPrefs.SetInt("shelves3Owned", 1);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }

        if (gameManager.currentInventoryItem == "BookCollection" && shelves3Owned)
        {
            SpriteRenderer oldImage = BookCollection.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        else if (gameManager.currentInventoryItem == "BookCollection")
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "You have no shelves to put this on yet!";
        }

        if (gameManager.currentInventoryItem == "Fish" && shelves2Owned)
        {
            SpriteRenderer oldImage = Fish.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        else if (gameManager.currentInventoryItem == "Fish")
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "You have no shelves to put this on yet!";
        }

        if (gameManager.currentInventoryItem == "ShelfDecor" && shelves2Owned)
        {
            SpriteRenderer oldImage = ShelfDecor.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        else if (gameManager.currentInventoryItem == "ShelfDecor")
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "You have no shelves to put this on yet!";
        }

        if (gameManager.currentInventoryItem == "Books" && shelves1Owned)
        {
            SpriteRenderer oldImage = Books.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        else if (gameManager.currentInventoryItem == "Books")
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "You have no shelves to put this on yet!";
        }

        if (gameManager.currentInventoryItem == "DeskPlant" && shelves1Owned)
        {
            SpriteRenderer oldImage = DeskPlant.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        else if (gameManager.currentInventoryItem == "DeskPlant")
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "You have no shelves to put this on yet!";
        }

        if (gameManager.currentInventoryItem == "TallPlant")
        {
            SpriteRenderer oldImage = TallPlant.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Flowers" && shelves2Owned)
        {
            SpriteRenderer oldImage = Flowers.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        else if (gameManager.currentInventoryItem == "Flowers")
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "You have no shelves to put this on yet!";
        }

        if (gameManager.currentInventoryItem == "Succulent")
        {
            SpriteRenderer oldImage = Succulent.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
            if (gameManager.placingItems == false)
            {
                gameManager.inventory.RemoveAt(gameManager.currentInventorySlot);
                gameManager.inventorySpace += 1;
                for (int j = 0; j < inventorySpaces.Length; j++)
                {
                    PlayerPrefs.DeleteKey("inventory_" + j);
                }
                for (int i = 0; i < gameManager.inventory.Count; i++)
                {
                    PlayerPrefs.SetString("inventory_" + i, gameManager.inventory[i]);
                }
            }
        }
        if (gameManager.currentInventoryItem == "Vase")
        {
            SpriteRenderer oldImage = Vase.GetComponent<SpriteRenderer>();
            oldImage.color = new Color32(255, 255, 255, 255);
        }
    }

    void Update()
    {
        //on every frame check if the next donation time has occurred 
        if (listOfDonationTimes != null && listOfDonationTimes.Count > 0 && DateTime.Compare(DateTime.Now, DateTime.Parse(listOfDonationTimes[0])) >= 0)
        {
            //if it has, run the deleteDonatedInventoryItem() method
            deleteDonatedInventoryItem();
        }

        //set the image of each inventory slot to transparent
        for (int j = gameManager.inventory.Count; j < 8; j++)
        {
            Image oldImage = inventorySpaces[j].GetComponent<Image>();
            oldImage.color = new Color32(255, 255, 255, 0);
        }

        if (gameManager.inventory.Count > 0)
        {
            //reset all inventory images based on the gameManager's inventory list in case of any updates
            for (int i = 0; i < gameManager.inventory.Count; i++)
            {
                Image oldImage = inventorySpaces[i].GetComponent<Image>();

                //if (inventorySpaces[i].GetComponent<InventoryItems>().beingDonated == 1)
                if (inventoryItemsBeingDonated.Contains(i))
                {
                    oldImage.color = new Color32(255, 255, 255, 150);
                }
                else
                {
                    oldImage.color = new Color32(255, 255, 255, 255);
                }

                if (gameManager.inventory[i] == "Clock")
                {
                    oldImage.sprite = prefabClock;
                }
                if (gameManager.inventory[i] == "Poster")
                {
                    oldImage.sprite = prefabPoster;

                }
                if (gameManager.inventory[i] == "Painting")
                {
                    oldImage.sprite = prefabPainting;

                }
                if (gameManager.inventory[i] == "Map")
                {
                    oldImage.sprite = prefabMap;

                }
                if (gameManager.inventory[i] == "Lamp1")
                {
                    oldImage.sprite = prefabLamp1;

                }
                if (gameManager.inventory[i] == "Lamp2")
                {
                    oldImage.sprite = prefabLamp2;

                }
                if (gameManager.inventory[i] == "Lamp3")
                {
                    oldImage.sprite = prefabLamp3;
                }
                if (gameManager.inventory[i] == "Lamp4")
                {
                    oldImage.sprite = prefabLamp4;
                }
                if (gameManager.inventory[i] == "Chair1")
                {
                    oldImage.sprite = prefabChair1;
                }
                if (gameManager.inventory[i] == "Chair2")
                {
                    oldImage.sprite = prefabChair2;

                }
                if (gameManager.inventory[i] == "Chair3")
                {
                    oldImage.sprite = prefabChair3;

                }
                if (gameManager.inventory[i] == "Jukebox")
                {
                    oldImage.sprite = prefabJukebox;

                }
                if (gameManager.inventory[i] == "Shelves1")
                {
                    oldImage.sprite = prefabShelves1;

                }
                if (gameManager.inventory[i] == "Shelves2")
                {
                    oldImage.sprite = prefabShelves2;

                }
                if (gameManager.inventory[i] == "Shelves3")
                {
                    oldImage.sprite = prefabShelves3;
                }
                if (gameManager.inventory[i] == "BookCollection")
                {
                    oldImage.sprite = prefabBookCollection;
                }
                if (gameManager.inventory[i] == "Fish")
                {
                    oldImage.sprite = prefabFish;
                }
                if (gameManager.inventory[i] == "ShelfDecor")
                {
                    oldImage.sprite = prefabShelfDecor;
                }
                if (gameManager.inventory[i] == "Books")
                {
                    oldImage.sprite = prefabBooks;

                }
                if (gameManager.inventory[i] == "DeskPlant")
                {
                    oldImage.sprite = prefabDeskPlant;

                }
                if (gameManager.inventory[i] == "TallPlant")
                {
                    oldImage.sprite = prefabTallPlant;

                }
                if (gameManager.inventory[i] == "Flowers")
                {
                    oldImage.sprite = prefabFlowers;

                }
                if (gameManager.inventory[i] == "Succulent")
                {
                    oldImage.sprite = prefabSucculent;
                }
                if (gameManager.inventory[i] == "Vase")
                {
                    oldImage.sprite = prefabVase;
                }
            }
        }
    }
}
