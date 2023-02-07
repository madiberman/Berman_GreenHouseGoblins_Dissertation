using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//attached to each image on the unlockables screen
//used to display information about how a Goblin was unlocked by the player
public class UnlockButton : MonoBehaviour
{
    public Canvas errorCanvas;
    public TextMeshProUGUI errorMessage;
    //the unlockable Goblin that the image is in reference to
    public Unlockables reference;

    //attached to each unlockable image on the unlockables screen
    public void onClickedButton()
    {
        //check if the referenced Goblin has been unlocked
        if (reference.keyName == "unlockable1" &&  reference.unlocked == true)
        {
            //if it has been unlocked, display the reason it was unlocked on a pop-up window
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "Unlocked: Recycled 1 item";
        }
        else if (reference.keyName == "unlockable2" && reference.unlocked == true)
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "Unlocked: Recycled 20 items";
        }
        else if(reference.keyName == "unlockable3" && reference.unlocked == true)
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "Unlocked: Recycled 50 items";
        }
        else if(reference.keyName == "unlockable4" && reference.unlocked == true)
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "Unlocked: Recycled 100 items";
        }
        else if(reference.keyName == "unlockable5")
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "Unlocked: Recycled 500 items";
        }
        else if(reference.keyName == "unlockable6" && reference.unlocked == true)
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "Unlocked: Recycled 1000 items";
        }
        else if(reference.keyName == "unlockable7" && reference.unlocked == true)
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "Unlocked: 20 purchases from Ma & Pop";
        }
        else if(reference.keyName == "unlockable8" && reference.unlocked == true)
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "Unlocked: 100 purchases from Ma & Pop";
        }
        else if(reference.keyName == "unlockable9" && reference.unlocked == true)
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "Unlocked: 500 purchases from Ma & Pop";
        }
        else if(reference.keyName == "unlockable10" && reference.unlocked == true)
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "Unlocked: Purchased eco laundry machine from Ma & Pop";
        }
        else if(reference.keyName == "unlockable11" && reference.unlocked == true)
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "Unlocked: Purchased eco washing machine from Ma & Pop";
        }
        else if(reference.keyName == "unlockable12" && reference.unlocked == true)
        {
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "Unlocked: Purchased solar panels from Ma & Pop";
        }
        else
        {
            //if it has not been unlocked, display "????"
            errorCanvas.gameObject.SetActive(true);
            errorMessage.text = "????";
        }
    }
}
