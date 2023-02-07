using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//attached to each Goblin that appears in the apartment
public class Unlockables : MonoBehaviour
{
    //keyname referenced by UnlockButton
    public string keyName;
    //refers to the Unity image attached to the GameObject of each Goblin
    Image oldImage;
    //unlocked status referenced by UnlockButton
    public bool unlocked;
    public Canvas errorCanvas;
    public TextMeshProUGUI errorMessage;
    //reference to the unlockables screen image of each unlock
    public GameObject reference;

    public void Start()
    {
        //load unlock status from PlayerPrefs
        if (PlayerPrefs.GetInt(keyName) == 1)
        {
            //unlock the Goblin the script is attached to 
            this.Unlock();
        }
    }

    public void Unlock()
    {
        //set PlayerPref that is called at Start()
        PlayerPrefs.SetInt(keyName, 1);

        //unlocked status referenced by UnlockButton
        unlocked = true;

        //set the image of the Goblin to the image of the unlockable screen image when unlocked
        oldImage = reference.GetComponent<Image>();
        oldImage.sprite = this.GetComponent<SpriteRenderer>().sprite;
        
    }
}
