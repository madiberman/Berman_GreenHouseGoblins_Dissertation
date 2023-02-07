using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItems : MonoBehaviour
{
    public GM gameManager;
    public Canvas placeSellDonate;
    public Canvas donationMessage;
    //between 0 and 5 for the space number in the inventory
    public int spaceNumber;
    public Inventory inventory;

    //run when an inventory item is clicked
    public void clickCheckInventory()
    {
        //checks if the inventory includes the space number so that empty inventory spaces won't trigger a response
        //&& if the inventory space number is not waiting for a donation pick-up
        if (gameManager.inventory.Count > spaceNumber && !inventory.inventoryItemsBeingDonated.Contains(spaceNumber))
        {
            //allow the player to choose what to do with the inventory item
            placeSellDonate.gameObject.SetActive(true);
        }
        //if the item is being donated and waiting for donation pick-up
        else if(inventory.inventoryItemsBeingDonated.Contains(spaceNumber))
        {
            //display and error
            donationMessage.gameObject.SetActive(true);
        }
    }

    //run when the inventory space is clicked
    public void setCurrentInventoryItem()
    {
        //if the space number is holding an item
        if (gameManager.inventory.Count > spaceNumber)
        {
            //set all information in the game manager about the item that has been clicked
            gameManager.currentInventoryItem = gameManager.inventory[spaceNumber];
            gameManager.currentInventorySlot = spaceNumber;
            gameManager.currentInventoryClassItem = this;
        }
    }
}
