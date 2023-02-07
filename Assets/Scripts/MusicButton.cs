using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour, IPointerClickHandler
{
    public GM gameManager;
    //slash image that appears over the music button 
    public GameObject slash;

    void Update()
    {
        //if the backgroundMusic int is 0, the music is off, and the slash is transparent
        if (gameManager.backgroundMusic == 0)
        {
            slash.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        }
        //if the backgroundMusic int is 1, the music is on, and the slash is opaque
        else
        {
            slash.GetComponent<Image>().color = new Color32(255, 255, 255, 186);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if music is on
        if(gameManager.backgroundMusic == 1)
        {
            gameManager.backgroundMusic = 0;
            //PlayerPrefs set so that when the game is reopened, the player's music preference is saved
            PlayerPrefs.SetInt("backgroundMusic", 0);
            //turn off music
            gameManager.backgroundMusicManager.gameObject.SetActive(false);
        }
        //if music is off
        else if (gameManager.backgroundMusic == 0)
        {
            gameManager.backgroundMusic = 1;
            //PlayerPrefs set so that when the game is reopened, the player's music preference is saved
            PlayerPrefs.SetInt("backgroundMusic", 1);
            //turn on music
            gameManager.backgroundMusicManager.gameObject.SetActive(true);
        }
    }
}
