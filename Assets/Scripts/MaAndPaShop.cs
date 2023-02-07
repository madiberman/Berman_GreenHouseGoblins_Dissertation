using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaAndPaShop : MonoBehaviour
{
    //objects in Unity whose positions are used to diplay each available item in the shop
    public GameObject prefabFood;
    public GameObject prefabAppliance;
    public GameObject prefabFurniture;
    public List<GameObject> foodList;
    public List<GameObject> applianceList;
    public List<GameObject> furnitureList;
    public Canvas localShop;

    //instantiated prefab buttons in the shop that change each day 
    GameObject foodNew;
    GameObject applianceNew;
    GameObject furnitureNew;
    public GameObject gameCanvas;

    //last time the items in the shop rotated
    DateTime oldDay;
    //current time/date
    DateTime newDay;

    public void Start()
    {
        //setting current items available in the shop based on PlayerPrefs
        if (PlayerPrefs.HasKey("old day"))
        {
            oldDay = DateTime.Parse(PlayerPrefs.GetString("old day"));
            foodNew = Instantiate(foodList[PlayerPrefs.GetInt("randomFood")], prefabFood.transform.position, prefabFood.transform.rotation, gameCanvas.transform);
            foodNew.transform.SetParent(localShop.transform);
            applianceNew = Instantiate(applianceList[PlayerPrefs.GetInt("randomAppliance")], prefabAppliance.transform.position, prefabAppliance.transform.rotation, gameCanvas.transform);
            applianceNew.transform.SetParent(localShop.transform);
            furnitureNew = Instantiate(furnitureList[PlayerPrefs.GetInt("randomFurniture")], prefabFurniture.transform.position, prefabFurniture.transform.rotation, gameCanvas.transform);
            furnitureNew.transform.SetParent(localShop.transform);
        }
        if (PlayerPrefs.HasKey("new day"))
        {
            newDay = DateTime.Parse(PlayerPrefs.GetString("new day"));
        }

        //setting the current items available if this is the first time the player is playing the game
        if (oldDay == null)
        {
            oldDay = DateTime.Now;
            PlayerPrefs.SetString("old day", oldDay.ToString());
            newDay = DateTime.Now;
            PlayerPrefs.SetString("new day", newDay.ToString());

            foodNew = Instantiate(foodList[0], prefabFood.transform.position, prefabFood.transform.rotation, gameCanvas.transform);

            applianceNew = Instantiate(applianceList[1], prefabAppliance.transform.position, prefabAppliance.transform.rotation, gameCanvas.transform);

            furnitureNew = Instantiate(furnitureList[22], prefabFurniture.transform.position, prefabFurniture.transform.rotation, gameCanvas.transform);
        }
    }

    public void Update()
    {
        //updating the current date and time
        newDay = DateTime.Now;
        PlayerPrefs.SetString("new day", newDay.ToString());

        //if the current date and time is equal to or over 24 hours since the previous item change in the shop
        if (DateTime.Compare(newDay, oldDay.AddSeconds(86400)) >= 0)
        {
            //resetting the time that the last item change occurred
            oldDay = DateTime.Now;
            PlayerPrefs.SetString("old day", oldDay.ToString());

            //NEW FOOD
            //choosing a random int for the food button prefab
            int randomFood = UnityEngine.Random.Range(0, 8);
            //destroying the previous food button prefab that was in the shop
            Destroy(foodNew);
            //creating a new object in the prefabFood's position and using the randomFood int to pull from the foodList of prefabs in Unity
            foodNew = Instantiate(foodList[randomFood], prefabFood.transform.position, prefabFood.transform.rotation, gameCanvas.transform);
            PlayerPrefs.SetInt("randomFood", randomFood);

            //NEW APPLIANCE
            //choosing a random int for the appliance button prefab
            int randomAppliance = UnityEngine.Random.Range(0, 3);
            //destroying the previous appliance button prefab that was in the shop
            Destroy(applianceNew);
            //creating a new object in the prefabAppliance's position and using the randomAppliance int to pull from the applianceList of prefabs in Unity
            applianceNew = Instantiate(applianceList[randomAppliance], prefabAppliance.transform.position, prefabAppliance.transform.rotation, gameCanvas.transform);
            PlayerPrefs.SetInt("randomAppliance", randomAppliance);

            //NEW FURNITURE
            //choosing a random int for the furniture button prefab
            int randomFurniture = UnityEngine.Random.Range(0, 24);
            //destroying the previous furniture button prefab that was in the shop
            Destroy(furnitureNew);
            //creating a new object in the prefabFurniture's position and using the randomFurniture int to pull from the furnitureList of prefabs in Unity
            furnitureNew = Instantiate(furnitureList[randomFurniture], prefabFurniture.transform.position, prefabFurniture.transform.rotation, gameCanvas.transform);
            PlayerPrefs.SetInt("randomFurniture", randomFurniture);
        }
    }
}
