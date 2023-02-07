using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    public Canvas Inventory;
    public Canvas placeSellDonate;
    //true if the inventory canvas is open, false if otherwise
    bool inventoryOpen;

    private void Start()
    {
        //inventory canvas is closed when the game begins
        inventoryOpen = false;
    }

    //run when the inventory button is clicked 
    public void buttonClicked()
    {
        //close the place / sell / donate canvas in case it is open 
        placeSellDonate.gameObject.SetActive(false);
        //if the inventory is not open
        if (inventoryOpen == false)
        {
            //open the inventory
            Inventory.gameObject.SetActive(true);
            inventoryOpen = true;
        }
        //if the inventory is open
        else if (inventoryOpen == true)
        {
            //close the inventory
            Inventory.gameObject.SetActive(false);
            inventoryOpen = false;
        }
    }
}
