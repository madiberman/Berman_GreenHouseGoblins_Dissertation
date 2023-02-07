using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinBehaviour : MonoBehaviour
{
    public GM gameManager;
    public bool isRecycling;
    public bool isLaundry;
    public bool isDishwasher;
    public bool isTrash;
    public SustainabilityBar sustainabilityBar;

    //in the event that a collider2D (in this case a trash piece) collides with the object this script is attached to
    void OnTriggerEnter2D(Collider2D col)
    {
        //if the object isTrash or isRecycling
        if (isTrash || isRecycling)
        {
            //play trash sound
            gameManager.soundManager.playTrash();
        }
        //if the object isDishwasher
        else if (isDishwasher)
        {
            //play dish sound
            gameManager.soundManager.playDishes();
        }
        //if the object isLaundry
        else if (isLaundry)
        {
            //play laundry sound
            gameManager.soundManager.playLaundry();
        }
        //if the Collider2D name includes "Trash"
        if (col.gameObject.name.Contains("Trash"))
        {
            //if the object isTrash
            if (isTrash)
            {
                gameManager.IncreaseTrashCount();
                //destroy Collider2D
                Destroy(col.gameObject);
            }
            //if the object isRecycling
            if (isRecycling)
            {
                //remove 1 sustainability point
                sustainabilityBar.addSustainability(-1);
                gameManager.IncreaseTrashCount();
                //destroy Collider2D
                Destroy(col.gameObject);
            }
        }
        //if the Collider2D name includes "Recycling"
        if (col.gameObject.name.Contains("Recycling"))
        {
            //if the object isRecycling
            if (isRecycling)
            {
                gameManager.IncreaseRecyclingCount();
                //destroy Collider2D
                Destroy(col.gameObject);
                //gain 1 sustainability point
                sustainabilityBar.addSustainability(1);
            }
            //if the object isTrash
            if (isTrash)
            {
                gameManager.IncreaseTrashCount();
                //destroy Collider2D
                Destroy(col.gameObject);
                //remove 1 sustainability point
                sustainabilityBar.addSustainability(-1);
            }
        }
        //if the Collider2D name includes "Dish"
        if (col.gameObject.name.Contains("Dish"))
        {
            //if the object isDishwasher
            if (isDishwasher)
            {
                //destroy Collider2D
                Destroy(col.gameObject);
                gameManager.dishNumber -= 1;
            }
        }
        //if the Collider2D name includes "Laundry"
        if (col.gameObject.name.Contains("Laundry"))
        {
            //if the object isLaundry
            if (isLaundry)
            {
                //destroy Collider2D
                Destroy(col.gameObject);
            }
        }
    }
}