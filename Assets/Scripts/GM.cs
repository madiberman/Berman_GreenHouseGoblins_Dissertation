using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking.NetworkSystem;
//using static UnityEditor.PlayerSettings;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;

public class GM : MonoBehaviour
{
    //flash
    public bool flashGo;
    public bool unlockableFlashGo;

    //spawner
    public Spawner spawnObject;

    //trash/recycling/dishes
    int trashCount;
    int recyclingCount;

    public int dishNumber;

    //unlockables
    public List<Unlockables> unlockables;
    public List<Unlockables> unlocked;

    //emails
    public int email;
    public int playedBefore;
    public int mpFirstPurchase;
    int currentEmail;

    //fridge
    public int emptyFridgeSpaces = 6;

    //delivery
    public bool allmartDelivery;
    public List<string> allmartBeingDelivered;
    public List<string> listOfDeliveryTimers;
    public List<string> localFoodBeingDelivered;
    public List<string> localTimers;
    public List<string> foodInFridge;
    public TextMeshProUGUI errorMessage;
    bool allmartOrder;
    int allmartOrdersTotal;
    int localOrdersTotal;

    //appliances
    public List<string> ownedAppliances;
    public GameObject Laundry;
    public GameObject Dishwasher;

    //furniture
    public Inventory inventoryClass;
    public List<string> inventory;
    public int inventorySpace = 8;
    public string currentInventoryItem;
    public InventoryItems currentInventoryClassItem;
    public int currentInventorySlot;
    public List<string> placedInventoryItems;

    //purchases
    public TextMeshProUGUI moneyText;
    public Canvas purchase;
    public Canvas errorInventory;
    public string currentPurchaseName;
    public float deliveryTime;
    DateTime timeOfDelivery;
    public Canvas deliveryAlert;

    //images
    public Sprite prefabLaundry;
    public Sprite prefabDishwasher;
    public Sprite prefabSolarpanels;

    //rewards
    public int money;
    public DateTime nextDailyReward;
    public Canvas dailyRewardCanvas;

    //time
    float T = 0;
    float U = 300;

    //spawns
    int spawnNumber;
    DateTime nextGoblinVisitSwitch;
    public List<int> currentGoblinVisitors;
    int currentNumberOfVisitors;
    public Canvas goblinGifts;
    public TextMeshProUGUI giftAmount;
    int giftInt;

    //hunger bar
    public float hunger;
    public bool hungerUpdated;
    public bool hungerEmpty;

    //sustainability bar
    public SustainabilityBar sustainabilityBar;

    //placing items at Start
    public bool placingItems;

    //sound
    public Audio soundManager;
    public AudioSource backgroundMusicManager;
    public int backgroundMusic;

    void Start()
    {
        unlockables[0].Unlock();
        unlockables[1].Unlock();
        unlockables[2].Unlock();
        unlockables[3].Unlock();
        unlockables[4].Unlock();
        unlockables[5].Unlock();

        playedBefore = 0;

        //deleted all PlayerPrefs if the player hasn't played before
        if(PlayerPrefs.HasKey("havePlayedBefore"))
        {
            if (PlayerPrefs.GetInt("havePlayedBefore") == 0)
            {
                PlayerPrefs.DeleteAll();
            }
        }
        else if (PlayerPrefs.HasKey("havePlayedBefore") == false)
        {
            PlayerPrefs.DeleteAll();
        }

        //make sure there is no currentInventoryItem
        currentInventoryItem = null;
        //hunger is not being updated by the GM
        hungerUpdated = false;

        //background music PlayerPrefs
        if (PlayerPrefs.HasKey("backgroundMusic"))
        {
            backgroundMusic = PlayerPrefs.GetInt("backgroundMusic");
            if(backgroundMusic == 1)
            {
                backgroundMusicManager.gameObject.SetActive(true);
            }
        }
        else
        {
            backgroundMusic = 1;
            backgroundMusicManager.gameObject.SetActive(true);
            PlayerPrefs.SetInt("backgroundMusic", 1);
        }

        //money PlayerPrefs
        if (PlayerPrefs.HasKey("money"))
        {
            money = PlayerPrefs.GetInt("money");
        }
        else
        {
            money = 20;
        }

        //fridge space PlayerPrefs
        if (PlayerPrefs.HasKey("emptyFridgeSpaces"))
        {
            emptyFridgeSpaces = PlayerPrefs.GetInt("emptyFridgeSpaces");
        }

        //current email int to be displayed PlayerPrefs
        if (PlayerPrefs.HasKey("currentEmail"))
        {
            email = PlayerPrefs.GetInt("currentEmail");
        }

        //total orders from Allmart PlayerPrefs
        if (PlayerPrefs.HasKey("allmartOrdersTotal"))
        {
            allmartOrdersTotal = PlayerPrefs.GetInt("allmartOrdersTotal");
        }

        //total orders from Ma&Pop PlayerPrefs
        if (PlayerPrefs.HasKey("localOrdersTotal"))
        {
            localOrdersTotal = PlayerPrefs.GetInt("localOrdersTotal");
        }

        //whether the player has played before loaded from PlayerPrefs
        if (PlayerPrefs.HasKey("havePlayedBefore") == false || (PlayerPrefs.HasKey("havePlayedBefore") && PlayerPrefs.GetInt("havePlayedBefore") == 0))
        {
            //if the player has never played before
            email = 2;
            soundManager.playEmail();
            //computer and email button flash becuase of new email
            flashGo = true;
            PlayerPrefs.SetInt("currentEmail", email);
            //spawn 10 random objects
            spawnObject.appReOpened(10);
            playedBefore = 1;
            PlayerPrefs.SetInt("havePlayedBefore", 1);
        }

        //whether the player has ordered from Ma&Pop previously PlayerPrefs
        if (PlayerPrefs.HasKey("mpFirstPurchase"))
        {
            mpFirstPurchase = 1;
            PlayerPrefs.SetInt("mpFirstPurchase", 1);
        }

        //PlayerPrefs for total trash and recycling sorted by the player
        if (PlayerPrefs.GetInt("trash") != 0)
        {
            trashCount = PlayerPrefs.GetInt("trash");
        }

        if (PlayerPrefs.GetInt("recycling") != 0)
        {
            recyclingCount = PlayerPrefs.GetInt("recycling");
        }

        //spawn trash/laundry/dishes/hunger bar based on time the player last closed the game
        if (PlayerPrefs.HasKey("time app closed"))
        {
            //if it has been between 3600 seconds and 25200 seconds since the app was closed
            if (DateTime.Compare(DateTime.Parse(PlayerPrefs.GetString("time app closed")).AddSeconds(3600), DateTime.Now) <= 0 && DateTime.Compare(DateTime.Parse(PlayerPrefs.GetString("time app closed")).AddSeconds(25200), DateTime.Now) > 0)
            {
                //spawn 0 - 2 pieces
                spawnNumber = UnityEngine.Random.Range(0, 3);
                spawnObject.appReOpened(spawnNumber);
                hunger = 20;
            }
            //if it has been between 25200 seconds and 43200 seconds since the app was closed
            else if (DateTime.Compare(DateTime.Parse(PlayerPrefs.GetString("time app closed")).AddSeconds(25200), DateTime.Now) <= 0 && DateTime.Compare(DateTime.Parse(PlayerPrefs.GetString("time app closed")).AddSeconds(43200), DateTime.Now) > 0)
            {
                //spawn 3 - 5 pieces
                spawnNumber = UnityEngine.Random.Range(3, 6);
                spawnObject.appReOpened(spawnNumber);
                hunger = 40;
            }
            //if it has been between 43200 seconds and 86400 seconds since the app was closed
            else if (DateTime.Compare(DateTime.Parse(PlayerPrefs.GetString("time app closed")).AddSeconds(43200), DateTime.Now) <= 0 && DateTime.Compare(DateTime.Parse(PlayerPrefs.GetString("time app closed")).AddSeconds(86400), DateTime.Now) > 0)
            {
                //spawn 6 - 7 pieces
                spawnNumber = UnityEngine.Random.Range(6, 8);
                spawnObject.appReOpened(spawnNumber);
                hunger = 60;
            }
            //if it has been more than 86400 since the app was closed
            else if (DateTime.Compare(DateTime.Parse(PlayerPrefs.GetString("time app closed")).AddSeconds(86400), DateTime.Now) <= 0)
            {
                //spawn 8 - 10 pieces
                spawnNumber = UnityEngine.Random.Range(8, 11);
                spawnObject.appReOpened(spawnNumber);
                hunger = 80;
            }
        }

        //placedItems & inventory list from PlayerPrefs
        for (int i = 0; i < inventorySpace; i++)
        {
            //game manager is currently placing items
            placingItems = true;

            if (PlayerPrefs.HasKey("placedInventoryItems_" + i))
            {
                currentInventoryItem = PlayerPrefs.GetString("placedInventoryItems_" + i);
                inventoryClass.placeInventoryItem();
                ownedAppliances.Add(currentInventoryItem);
                placedInventoryItems.Add(currentInventoryItem);
            }
            if (PlayerPrefs.HasKey("inventory_" + i))
            {
                inventory.Add(PlayerPrefs.GetString("inventory_" + i));
                ownedAppliances.Add(PlayerPrefs.GetString("inventory_" + i));
                inventorySpace -= 1;
            }
        }
        //game manager is not currently placing items
        placingItems = false;

        //owned appliances from PlayerPrefs
        if (PlayerPrefs.HasKey("laundry") && PlayerPrefs.GetInt("laundry") == 1)
        {
            ownedAppliances.Add("Laundry");
            //if eco-laundry is owned, updated its sprite
            Laundry.GetComponent<SpriteRenderer>().color = new Color32(218, 255, 191, 255);
        }
        if (PlayerPrefs.HasKey("dishwasher") && PlayerPrefs.GetInt("dishwasher") == 1)
        {
            ownedAppliances.Add("Dishwasher");
            //if eco-dish washer is owned, updated its sprite
            Dishwasher.GetComponent<SpriteRenderer>().color = new Color32(207, 255, 198, 255);
        }
        if (PlayerPrefs.HasKey("solarpanels") && PlayerPrefs.GetInt("solarpanels") == 1)
        {
            ownedAppliances.Add("Solarpanels");
        }

        //owned items list from PlayerPrefs
        for (int i = 0; i < 30; i++)
        {
            if (PlayerPrefs.HasKey("ownedItems_" + i))
            {
                ownedAppliances.Add(PlayerPrefs.GetString("ownedItems_" + i));
            }

        }

        //deliveries from Allmart and Ma&Pop and theur delivery times loaded from PlayerPrefs into the appropriate lists
        for (int m = 0; m < 35; m++)
        {
            if (PlayerPrefs.HasKey("listOfDeliveryTimers_" + m))
            {
                Debug.Log(PlayerPrefs.GetString("listOfDeliveryTimers_" + m));
                listOfDeliveryTimers.Add(PlayerPrefs.GetString("listOfDeliveryTimers_" + m));
            }
            if (PlayerPrefs.HasKey("localTimers_" + m))
            {
                localTimers.Add(PlayerPrefs.GetString("localTimers_" + m));
            }
            if (PlayerPrefs.HasKey("allmartBeingDelivered_" + m))
            {
                Debug.Log(PlayerPrefs.GetString("allmartBeingDelivered_" + m));
                allmartBeingDelivered.Add(PlayerPrefs.GetString("allmartBeingDelivered_" + m));
            }
            if (PlayerPrefs.HasKey("localFoodBeingDelivered_" + m))
            {
                localFoodBeingDelivered.Add(PlayerPrefs.GetString("localFoodBeingDelivered_" + m));
            }
        }

        //next time the player recieves daily rewards loaded from PlayerPrefs
        if (PlayerPrefs.HasKey("nextDailyReward"))
        {
            nextDailyReward = DateTime.Parse(PlayerPrefs.GetString("nextDailyReward"));
        }
        else
        {
            nextDailyReward = DateTime.Now;
            PlayerPrefs.SetString("nextDailyReward", nextDailyReward.ToString());
        }


        //create list of unlockables that have been unlocked
        foreach (Unlockables unl in unlockables)
        {
            if (unl.unlocked == true)
            {
                unlocked.Add(unl);
            }
        }

        //make all unlockables transparent
        foreach (Unlockables unl in unlockables)
        {
            unl.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
        }

        //get next Goblin visit switch from PlayerPrefs
        if (PlayerPrefs.HasKey("nextGoblinVisit"))
        {
            nextGoblinVisitSwitch = DateTime.Parse(PlayerPrefs.GetString("nextGoblinVisit"));
        }
        else
        {
            nextGoblinVisitSwitch = DateTime.Now;
            PlayerPrefs.SetString("nextGoblinVisit", nextGoblinVisitSwitch.ToString());
        }

        //if the next Goblin visit switch hasnt occured
        int result = DateTime.Compare(DateTime.Now, nextGoblinVisitSwitch);
        if (result < 0)
        {
            for (int i = 0; i < unlocked.Count; i++)
            {
                //load the Goblins that were already visiting the last time the app was closed
                if (PlayerPrefs.HasKey("currentGoblinVisitors_" + i))
                {
                    unlocked[PlayerPrefs.GetInt("currentGoblinVisitors_" + i)].GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                }
            }
        }

        //if the next Goblin visit switch has passed
        else if (result >= 0)
        {
            //reser Goblin visitors
            currentGoblinVisitors.Clear();
            //delete Goblin visitors from PlayerPrefs
            for (int i = 0; i < unlocked.Count; i++)
            {
                if (PlayerPrefs.HasKey("currentGoblinVisitors_" + i))
                {
                    PlayerPrefs.DeleteKey("currentGoblinVisitors_" + i);
                }
            }

            //set new nextGoblinVisitSwitch time
            nextGoblinVisitSwitch = DateTime.Now.AddSeconds(10800);
            PlayerPrefs.SetString("nextGoblinVisit", nextGoblinVisitSwitch.ToString());

            //choose next random set of unlockables to visit if there are any Goblins unlocked
            for (int i = 0; i < unlocked.Count; i++)
            {
                //choose random number of visitors
                int randomNumber = UnityEngine.Random.Range(0, unlocked.Count);
                SpriteRenderer newS = unlocked[randomNumber].GetComponent<SpriteRenderer>();
                //make new visitors opaque
                newS.color = new Color32(255, 255, 255, 255);
                currentGoblinVisitors.Add(randomNumber);
            }

            //reset Goblin visitor information in PlayerPrefs
            PlayerPrefs.SetInt("currentGoblinVisitors_count", currentGoblinVisitors.Count);
            for (int i = 0; i < currentGoblinVisitors.Count; i++)
            {
                PlayerPrefs.SetInt("currentGoblinVisitors_" + i, currentGoblinVisitors[i]);
            }

            //if there aer unlocked Goblins
            if (currentGoblinVisitors.Count > 0)
            {
                //activate the Goblin gifts canvas
                goblinGifts.gameObject.SetActive(true);
                //randomize the monetary gift from the Goblins and multiply it by the number of visitors
                giftInt = UnityEngine.Random.Range(5, 16) * currentGoblinVisitors.Count;
                giftAmount.text = "£" + giftInt.ToString();
                //add the gift amount to the player's total money
                money += giftInt;
                PlayerPrefs.SetInt("money", money);
            }
        }
    }

    private void Update()
    {
        //if the player hits the esc key, the game will quit
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        //money displayed on the screen is the amount of money the player has
        moneyText.text = money.ToString();
        PlayerPrefs.SetInt("money", money);

        //if the nextDailyReward time has passed
        if (DateTime.Compare(nextDailyReward, DateTime.Now) <= 0 && nextDailyReward != null && !goblinGifts.isActiveAndEnabled)
        {
            //give the player new daily rewards
            dailyRewardCanvas.gameObject.SetActive(true);
        }


        if (listOfDeliveryTimers.Count > 0)
        {
            //if the next Allmart delivery time has passed
            if (DateTime.Compare(DateTime.Now, DateTime.Parse(listOfDeliveryTimers[0])) >= 0)
            {
                //activate delivery
                deliveryAlert.gameObject.SetActive(true);
                allmartDelivery = true;
            }
        }

        if (localTimers.Count > 0)
        {
            //if the next Ma&Pop delivery time has passed
            if (DateTime.Compare(DateTime.Now, DateTime.Parse(localTimers[0])) >= 0)
            {
                //activate delivery
                deliveryAlert.gameObject.SetActive(true);
                allmartDelivery = false;
            }
        }

        //if the time is greater than T
        if (Time.time >= T)
        {
            //T = seconds since opening the app plus U
            T = Time.time + U;
            //set time app closed PlayerPref
            PlayerPrefs.SetString("time app closed", DateTime.Now.ToString());
        }
    }

    //run after a delivery is made
    public void removeFirst()
    {
        //if the delivery was from Allmart
        if (allmartDelivery == true)
        {
            //if the delivery was food
            if (allmartBeingDelivered[0] == "Strawberries" || allmartBeingDelivered[0] == "Carrots" || allmartBeingDelivered[0] == "Aubergine" || allmartBeingDelivered[0] == "Eggs" || allmartBeingDelivered[0] == "Meat" || allmartBeingDelivered[0] == "Pumpkin" || allmartBeingDelivered[0] == "Noodles" || allmartBeingDelivered[0] == "Shrimp")
            {
                //add the delivery to the fridge
                foodInFridge.Add(allmartBeingDelivered[0]);
                PlayerPrefs.SetString("foodSpace_" + (foodInFridge.Count - 1), allmartBeingDelivered[0]);
            }
            //if the delivery was an eco-laundry machine
            else if (allmartBeingDelivered[0] == "Laundry")
            {
                //updated the laundry sprite's color to be green
                SpriteRenderer oldImage = Laundry.GetComponent<SpriteRenderer>();
                oldImage.color = new Color32(218, 255, 191, 255);
            }
            //if the delivery was an eco-dish washer
            else if (allmartBeingDelivered[0] == "Dishwasher")
            {
                //updated the dish washer's sprite's color to be green
                SpriteRenderer oldImage = Dishwasher.GetComponent<SpriteRenderer>();
                oldImage.color = new Color32(207, 255, 198, 255);
            }
            //if the delivery was solar panels
            else if (allmartBeingDelivered[0] == "Solarpanels")
            {
                //send the player an email
                email = 3;
                soundManager.playEmail();
                flashGo = true;
                PlayerPrefs.SetInt("currentEmail", email);
            }
            //if the delivery was furniture
            else
            {
                //add the delivery to the player's inventory
                inventory.Add(allmartBeingDelivered[0]);
                PlayerPrefs.SetString("inventory_" + inventory.Count, allmartBeingDelivered[0]);
            }

            //remove the first delivery item from Allmart
            allmartBeingDelivered.RemoveAt(0);

            //reset PlayerPrefs list of allmart items being delivered by deleting all keys and remaking them using the allmartBeingDelivered list
            for (int i = 0; i < 6; i++)
            {
                if (PlayerPrefs.HasKey("allmartBeingDelivered_" + i))
                {
                    PlayerPrefs.DeleteKey("allmartBeingDelivered_" + i);
                }

                if (allmartBeingDelivered.Count > i)
                {
                    PlayerPrefs.SetString("allmartBeingDelivered_" + i, allmartBeingDelivered[i]);
                }
            }
            //remove the first delivery time from allmart's delivery times list
            listOfDeliveryTimers.RemoveAt(0);
            //reset PlayerPrefs list of allmart delivered times by deleting all keys and remaking them using the listOfDeliveryTimers list
            for (int i = 0; i < 6; i++)
            {
                if (PlayerPrefs.HasKey("listOfDeliveryTimers_" + i))
                {
                    PlayerPrefs.DeleteKey("listOfDeliveryTimers_" + i);
                }

                if (listOfDeliveryTimers.Count > i)
                {
                    PlayerPrefs.SetString("listOfDeliveryTimers_" + i, listOfDeliveryTimers[i]);
                }
            }
        }
        else
        {
            if (localFoodBeingDelivered[0] == "Strawberries" || localFoodBeingDelivered[0] == "Carrots" || localFoodBeingDelivered[0] == "Aubergine" || localFoodBeingDelivered[0] == "Eggs" || localFoodBeingDelivered[0] == "Meat" || localFoodBeingDelivered[0] == "Pumpkin" || localFoodBeingDelivered[0] == "Noodles" || localFoodBeingDelivered[0] == "Shrimp")
            {
                foodInFridge.Add(localFoodBeingDelivered[0]);
                PlayerPrefs.SetString("foodSpace_" + (foodInFridge.Count - 1), localFoodBeingDelivered[0]);
            }
            else if (localFoodBeingDelivered[0] == "Laundry")
            {
                SpriteRenderer oldImage = Laundry.GetComponent<SpriteRenderer>();
                oldImage.color = new Color32(218, 255, 191, 255);
            }
            else if (localFoodBeingDelivered[0] == "Dishwasher")
            {
                SpriteRenderer oldImage = Dishwasher.GetComponent<SpriteRenderer>();
                oldImage.color = new Color32(207, 255, 198, 255);
            }
            else if (localFoodBeingDelivered[0] == "Solarpanels")
            {
                email = 3;
                soundManager.playEmail();
                flashGo = true;
                PlayerPrefs.SetInt("currentEmail", email);
            }
            else
            {
                inventory.Add(localFoodBeingDelivered[0]);
                PlayerPrefs.SetString("inventory_" + inventory.Count, localFoodBeingDelivered[0]);
            }

            //remove the first delivery item from Ma&Pop
            localFoodBeingDelivered.RemoveAt(0);
            //reset PlayerPrefs list of Ma&Pop items being delivered by deleting all keys and remaking them using the localFoodBeingDelivered list
            for (int i = 0; i < 6; i++)
            {
                if (PlayerPrefs.HasKey("localFoodBeingDelivered_" + i))
                {
                    PlayerPrefs.DeleteKey("localFoodBeingDelivered_" + i);
                }

                if (localFoodBeingDelivered.Count > i)
                {
                    PlayerPrefs.SetString("localFoodBeingDelivered_" + i, localFoodBeingDelivered[i]);
                }
            }
            //remove the first delivery time from Ma&Pop's delivery times list
            localTimers.RemoveAt(0);
            //reset PlayerPrefs list of Ma&Pop items being delivered by deleting all keys and remaking them using the localTimers list
            for (int i = 0; i < 6; i++)
            {
                if (PlayerPrefs.HasKey("localTimers_" + i))
                {
                    PlayerPrefs.DeleteKey("localTimers_" + i);
                }

                if (localTimers.Count > i)
                {
                    PlayerPrefs.SetString("localTimers_" + i, localTimers[i]);
                }
            }
        }
    }

    //increase total trash count by 1
    public void IncreaseTrashCount()
    {
        trashCount++;
        PlayerPrefs.SetInt("trash", trashCount);
    }
    //increase total recycling count by 1
    public void IncreaseRecyclingCount()
    {
        recyclingCount++;
        PlayerPrefs.SetInt("recycling", recyclingCount);
        if (recyclingCount == 1)
        {
            //unlock an unlockable
            unlockables[0].Unlock();
            //add the unlockable to the list of unlockables
            unlocked.Add(unlockables[0]);
            unlockableFlashGo = true;
            //set email to 1
            email = 1;
            //play the email sound
            soundManager.playEmail();
            //computer and email button flash until the email is read
            flashGo = true;
            PlayerPrefs.SetInt("currentEmail", email);
        }
        if (recyclingCount == 25)
        {
            unlockables[1].Unlock();
            unlocked.Add(unlockables[1]);
            unlockableFlashGo = true;
        }
        if (recyclingCount == 50)
        {
            unlockables[2].Unlock();
            unlocked.Add(unlockables[2]);
            unlockableFlashGo = true;
        }
        if (recyclingCount == 100)
        {
            unlockables[3].Unlock();
            unlocked.Add(unlockables[3]);
            unlockableFlashGo = true;
        }
        if (recyclingCount == 500)
        {
            unlockables[4].Unlock();
            unlocked.Add(unlockables[4]);
            unlockableFlashGo = true;
        }
        if (recyclingCount == 1000)
        {
            unlockables[5].Unlock();
            unlocked.Add(unlockables[5]);
            unlockableFlashGo = true;
        }
    }

    //run when a purchase is made
    public void startTimer()
    {
        //the time of the delivery is the current date time plus the deliveryTime seconds set in the purchaseMadeAllmart() method
        timeOfDelivery = DateTime.Now.AddSeconds(deliveryTime);
        Debug.Log("time of Delivery: " + timeOfDelivery);

        //add the delivery item and the time of delivery to the appropriate lists based on if it was an Allmart or Ma&Pop order
        if (allmartOrder == true)
        {
            listOfDeliveryTimers.Add(timeOfDelivery.ToString());
            PlayerPrefs.SetString("listOfDeliveryTimers_" + (listOfDeliveryTimers.Count - 1), listOfDeliveryTimers[(listOfDeliveryTimers.Count - 1)]);
        }
        else
        {
            localTimers.Add(timeOfDelivery.ToString());
            PlayerPrefs.SetString("localTimers_" + (localTimers.Count - 1), localTimers[(localTimers.Count - 1)]);

        }
    }

    //run when any purchase is made, not just from Allmart contrary to the name
    public void purchaseMadeAllmart()
    {
        //if the order if from allmart
        if (allmartOrder == true)
        {
            //delivery time is 60 seconds
            deliveryTime = 60f;
            allmartOrdersTotal += 1;
            //remove two sustainability points from the player
            sustainabilityBar.addSustainability(-2);
            PlayerPrefs.SetInt("allmartOrdersTotal", allmartOrdersTotal);
        }
        //if the order if from Ma&Pop and it's the first time they have purchased from Ma&Pop
        else if (allmartOrder == false && mpFirstPurchase == 0)
        {
            //reward the player with 2 sustainability points
            sustainabilityBar.addSustainability(2);
            //send email 4
            email = 4;
            //play email sound
            soundManager.playEmail();
            //computer and email button flash until the email is read
            flashGo = true;
            PlayerPrefs.SetInt("currentEmail", email);
            mpFirstPurchase = 1;
            PlayerPrefs.SetInt("mpFirstPurchase", 1);
            //delivery time is 600 seconds
            deliveryTime = 600f;
            localOrdersTotal += 1;
            PlayerPrefs.SetInt("localOrdersTotal", localOrdersTotal);
        }
        //if the order is from Ma&Pop but this is not the player's first order from Ma&Pop
        else
        {
            sustainabilityBar.addSustainability(2);
            localOrdersTotal += 1;
            PlayerPrefs.SetInt("localOrdersTotal", localOrdersTotal);

            //if net total of local orders is 20
            if (localOrdersTotal - allmartOrdersTotal >= 20)
            {
                //unlock an unlockable
                unlockables[6].Unlock();
                //add the unlockable to the unlockables list
                unlocked.Add(unlockables[6]);
                unlockableFlashGo = true;
            }
            if (localOrdersTotal - allmartOrdersTotal >= 100)
            {
                unlockables[7].Unlock();
                unlocked.Add(unlockables[7]);
                unlockableFlashGo = true;
            }
            if (localOrdersTotal - allmartOrdersTotal >= 500)
            {
                unlockables[8].Unlock();
                unlocked.Add(unlockables[8]);
                unlockableFlashGo = true;
            }
        }

        //if the currentPurchaseName string matches a string in this if/else a and the player has enough money for the item
        if (currentPurchaseName == "Strawberries" && money >= 2)
        {
            //the money is taken from the Player
            money -= 2;
            PlayerPrefs.SetInt("money", money);
            //an available fridge space is removed
            emptyFridgeSpaces -= 1;
            PlayerPrefs.SetInt("emptyFridgeSpaces", emptyFridgeSpaces);
            //startTimer() is run
            this.startTimer();

            //depending on if the order is from Allart or Ma&Pop, add it to the appropriate list of items being delivered 
            if(allmartOrder == true)
            {
                allmartBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
            }
            else
            {
                Debug.Log("local");
                localFoodBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
            }
        }
        //if there is not enough money for the item
        else if (currentPurchaseName == "Strawberries" && money < 2)
        {
            //display an error to the player
            errorInventory.gameObject.SetActive(true);
            errorMessage.text = "You don't have enough money for this!";
        }
        else if (currentPurchaseName == "Noodles" && money >= 3)
        {
            money -= 3;
            PlayerPrefs.SetInt("money", money);
            emptyFridgeSpaces -= 1;
            PlayerPrefs.SetInt("emptyFridgeSpaces", emptyFridgeSpaces);
            this.startTimer();
            if (allmartOrder == true)
            {
                allmartBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
            }
            else
            {
                localFoodBeingDelivered.Add(currentPurchaseName);
                Debug.Log(localFoodBeingDelivered.Count);
                PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
            }
        }
        else if (currentPurchaseName == "Noodles" && money < 3)
        {
            errorInventory.gameObject.SetActive(true);
            errorMessage.text = "You don't have enough money for this!";
        }
        else if (currentPurchaseName == "Carrots" && money >= 2)
        {
            money -= 2;
            PlayerPrefs.SetInt("money", money);
            emptyFridgeSpaces -= 1;
            PlayerPrefs.SetInt("emptyFridgeSpaces", emptyFridgeSpaces);
            this.startTimer();
            if (allmartOrder == true)
            {
                allmartBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
            }
            else
            {
                localFoodBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
            }
        }
        else if (currentPurchaseName == "Carrots" && money < 2)
        {
            errorInventory.gameObject.SetActive(true);
            errorMessage.text = "You don't have enough money for this!";
        }
        else if (currentPurchaseName == "Aubergine" && money >= 3)
        {
            money -= 3;
            PlayerPrefs.SetInt("money", money);
            emptyFridgeSpaces -= 1;
            PlayerPrefs.SetInt("emptyFridgeSpaces", emptyFridgeSpaces);
            this.startTimer();
            allmartBeingDelivered.Add(currentPurchaseName);
            if (allmartOrder == true)
            {
                allmartBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
            }
            else
            {
                localFoodBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
            }
        }
        else if (currentPurchaseName == "Aubergine" && money < 3)
        {
            errorInventory.gameObject.SetActive(true);
            errorMessage.text = "You don't have enough money for this!";
        }
        else if (currentPurchaseName == "Eggs" && money >= 5)
        {
            money -= 5;
            PlayerPrefs.SetInt("money", money);
            emptyFridgeSpaces -= 1;
            PlayerPrefs.SetInt("emptyFridgeSpaces", emptyFridgeSpaces);
            this.startTimer();
            if (allmartOrder == true)
            {
                allmartBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
            }
            else
            {
                localFoodBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
            }
        }
        else if (currentPurchaseName == "Eggs" && money < 5)
        {
            errorInventory.gameObject.SetActive(true);
            errorMessage.text = "You don't have enough money for this!";
        }
        else if (currentPurchaseName == "Meat" && money >= 6)
        {
            money -= 6;
            PlayerPrefs.SetInt("money", money);
            emptyFridgeSpaces -= 1;
            PlayerPrefs.SetInt("emptyFridgeSpaces", emptyFridgeSpaces);
            this.startTimer();
            if (allmartOrder == true)
            {
                allmartBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
            }
            else
            {
                localFoodBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
            }
        }
        else if (currentPurchaseName == "Meat" && money < 6)
        {
            errorInventory.gameObject.SetActive(true);
            errorMessage.text = "You don't have enough money for this!";
        }
        else if (currentPurchaseName == "Pumpkin" && money >= 5)
        {
            money -= 5;
            PlayerPrefs.SetInt("money", money);
            emptyFridgeSpaces -= 1;
            PlayerPrefs.SetInt("emptyFridgeSpaces", emptyFridgeSpaces);
            this.startTimer();
            if (allmartOrder == true)
            {
                allmartBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
            }
            else
            {
                localFoodBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
            }
        }
        else if (currentPurchaseName == "Pumpkin" && money < 5)
        {
            errorInventory.gameObject.SetActive(true);
            errorMessage.text = "You don't have enough money for this!";
        }
        else if (currentPurchaseName == "Shrimp" && money >= 4)
        {
            money -= 4;
            PlayerPrefs.SetInt("money", money);
            emptyFridgeSpaces -= 1;
            PlayerPrefs.SetInt("emptyFridgeSpaces", emptyFridgeSpaces);
            this.startTimer();
            if (allmartOrder == true)
            {
                allmartBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
            }
            else
            {
                localFoodBeingDelivered.Add(currentPurchaseName);
                PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
            }
        }
        else if (currentPurchaseName == "Shrimp" && money < 4)
        {
            errorInventory.gameObject.SetActive(true);
            errorMessage.text = "You don't have enough money for this!";
        }

        //if the player is attemping to buy non-food items, they must not be hungry
        else if (hungerEmpty == false && !placedInventoryItems.Contains(currentPurchaseName) && !inventory.Contains(currentPurchaseName))
        {
            if (currentPurchaseName == "Laundry" && money >= 200)
            {
                money -= 200;
                sustainabilityBar.addSustainability(15);
                PlayerPrefs.SetInt("money", money);
                ownedAppliances.Add(currentPurchaseName);
                PlayerPrefs.SetString("ownedItems_" + (ownedAppliances.Count - 1), currentPurchaseName);
                this.startTimer();
                if (allmartOrder == true)
                {
                    allmartBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
                }
                else
                {
                    localFoodBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
                }
                PlayerPrefs.SetInt("laundry", 1);
                email = 5;
                soundManager.playEmail();
                flashGo = true;
                PlayerPrefs.SetInt("currentEmail", email);
            }
            else if (currentPurchaseName == "Laundry" && money < 200)
            {
                errorInventory.gameObject.SetActive(true);
                errorMessage.text = "You don't have enough money for this!";
            }
            else if (currentPurchaseName == "Dishwasher" && money >= 500)
            {
                sustainabilityBar.addSustainability(15);
                money -= 500;
                PlayerPrefs.SetInt("money", money);
                ownedAppliances.Add(currentPurchaseName);
                PlayerPrefs.SetString("ownedItems_" + (ownedAppliances.Count - 1), currentPurchaseName);
                this.startTimer();
                if (allmartOrder == true)
                {
                    allmartBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
                }
                else
                {
                    localFoodBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
                }
                PlayerPrefs.SetInt("dishwasher", 1);
                email = 6;
                soundManager.playEmail();
                flashGo = true;
                PlayerPrefs.SetInt("currentEmail", email);
            }
            else if (currentPurchaseName == "Dishwasher" && money < 100)
            {
                errorInventory.gameObject.SetActive(true);
                errorMessage.text = "You don't have enough money for this!";
            }
            else if (currentPurchaseName == "Solarpanels" && money >= 1000)
            {
                sustainabilityBar.addSustainability(30);
                money -= 1000;
                PlayerPrefs.SetInt("money", money);
                ownedAppliances.Add(currentPurchaseName);
                PlayerPrefs.SetString("ownedItems_" + (ownedAppliances.Count - 1), currentPurchaseName);
                this.startTimer();
                if (allmartOrder == true)
                {
                    allmartBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
                }
                else
                {
                    localFoodBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
                }
                PlayerPrefs.SetInt("solarpanels", 1);
            }
            else if (currentPurchaseName == "Solarpanels" && money < 1000)
            {
                errorInventory.gameObject.SetActive(true);
                errorMessage.text = "You don't have enough money for this!";
            }
            else if (money >= 10 && (currentPurchaseName == "DeskPlant" || currentPurchaseName == "Vase"))
            {
                money -= 10;
                PlayerPrefs.SetInt("money", money);
                ownedAppliances.Add(currentPurchaseName);
                PlayerPrefs.SetString("ownedItems_" + (ownedAppliances.Count - 1), currentPurchaseName);
                inventorySpace -= 1;
                this.startTimer();
                if (allmartOrder == true)
                {
                    allmartBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
                }
                else
                {
                    localFoodBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
                }
            }
            else if (money < 10 && (currentPurchaseName == "DeskPlant" || currentPurchaseName == "Vase"))
            {
                errorInventory.gameObject.SetActive(true);
                errorMessage.text = "You don't have enough money for this!";
            }
            else if (money >= 15 && (currentPurchaseName == "Clock" || currentPurchaseName == "Books" || currentPurchaseName == "Flowers" || currentPurchaseName == "Succulent" || currentPurchaseName == "TallPlant"))
            {
                money -= 15;
                PlayerPrefs.SetInt("money", money);
                ownedAppliances.Add(currentPurchaseName);
                PlayerPrefs.SetString("ownedItems_" + (ownedAppliances.Count - 1), currentPurchaseName);
                inventorySpace -= 1;
                this.startTimer();
                if (allmartOrder == true)
                {
                    allmartBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
                }
                else
                {
                    localFoodBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
                }
            }
            else if (money < 15 && (currentPurchaseName == "Clock" || currentPurchaseName == "Books" || currentPurchaseName == "Flowers" || currentPurchaseName == "Succulent" || currentPurchaseName == "TallPlant"))
            {
                errorInventory.gameObject.SetActive(true);
                errorMessage.text = "You don't have enough money for this!";
            }
            else if (money >= 20 && (currentPurchaseName == "Painting" || currentPurchaseName == "Poster" || currentPurchaseName == "Map" || currentPurchaseName == "Fish"))
            {
                money -= 20;
                PlayerPrefs.SetInt("money", money);
                ownedAppliances.Add(currentPurchaseName);
                PlayerPrefs.SetString("ownedItems_" + (ownedAppliances.Count - 1), currentPurchaseName);
                inventorySpace -= 1;
                this.startTimer();
                if (allmartOrder == true)
                {
                    allmartBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
                }
                else
                {
                    localFoodBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
                }
            }
            else if (money < 20 && (currentPurchaseName == "Painting" || currentPurchaseName == "Poster" || currentPurchaseName == "Map" || currentPurchaseName == "Fish"))
            {
                errorInventory.gameObject.SetActive(true);
                errorMessage.text = "You don't have enough money for this!";
            }
            else if (money >= 25 && (currentPurchaseName == "Lamp2" || currentPurchaseName == "Lamp3" || currentPurchaseName == "Lamp4" || currentPurchaseName == "ShelfDecor"))
            {
                money -= 25;
                PlayerPrefs.SetInt("money", money);
                ownedAppliances.Add(currentPurchaseName);
                PlayerPrefs.SetString("ownedItems_" + (ownedAppliances.Count - 1), currentPurchaseName);
                inventorySpace -= 1;
                this.startTimer();
                if (allmartOrder == true)
                {
                    allmartBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
                }
                else
                {
                    localFoodBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
                }
            }
            else if (money < 25 && (currentPurchaseName == "Lamp2" || currentPurchaseName == "Lamp3" || currentPurchaseName == "Lamp4" || currentPurchaseName == "ShelfDecor"))
            {
                errorInventory.gameObject.SetActive(true);
                errorMessage.text = "You don't have enough money for this!";
            }
            else if (money >= 30 && currentPurchaseName == "Lamp1")
            {
                money -= 30;
                PlayerPrefs.SetInt("money", money);
                ownedAppliances.Add(currentPurchaseName);
                PlayerPrefs.SetString("ownedItems_" + (ownedAppliances.Count - 1), currentPurchaseName);
                inventorySpace -= 1;
                this.startTimer();
                if (allmartOrder == true)
                {
                    allmartBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
                }
                else
                {
                    localFoodBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
                }
            }
            else if (money < 30 && currentPurchaseName == "Lamp1")
            {
                errorInventory.gameObject.SetActive(true);
                errorMessage.text = "You don't have enough money for this!";
            }
            else if (money >= 50 && (currentPurchaseName == "Shelves1" || currentPurchaseName == "BookCollection"))
            {
                money -= 50;
                PlayerPrefs.SetInt("money", money);
                ownedAppliances.Add(currentPurchaseName);
                PlayerPrefs.SetString("ownedItems_" + (ownedAppliances.Count - 1), currentPurchaseName);
                inventorySpace -= 1;
                this.startTimer();
                if (allmartOrder == true)
                {
                    allmartBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
                }
                else
                {
                    localFoodBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
                }
            }
            else if (money < 50 && (currentPurchaseName == "Shelves1" || currentPurchaseName == "BookCollection"))
            {
                errorInventory.gameObject.SetActive(true);
                errorMessage.text = "You don't have enough money for this!";
            }
            else if (money >= 75 && currentPurchaseName == "Shelves2")
            {
                money -= 75;
                PlayerPrefs.SetInt("money", money);
                ownedAppliances.Add(currentPurchaseName);
                PlayerPrefs.SetString("ownedItems_" + (ownedAppliances.Count - 1), currentPurchaseName);
                inventorySpace -= 1;
                this.startTimer();
                if (allmartOrder == true)
                {
                    allmartBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
                }
                else
                {
                    localFoodBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
                }
            }
            else if (money < 75 && currentPurchaseName == "Shelves2")
            {
                errorInventory.gameObject.SetActive(true);
                errorMessage.text = "You don't have enough money for this!";
            }
            else if (money >= 100 && (currentPurchaseName == "Chair1" || currentPurchaseName == "Chair2" || currentPurchaseName == "Chair3" || currentPurchaseName == "Shelves3"))
            {
                money -= 100;
                PlayerPrefs.SetInt("money", money);
                ownedAppliances.Add(currentPurchaseName);
                PlayerPrefs.SetString("ownedItems_" + (ownedAppliances.Count - 1), currentPurchaseName);
                inventorySpace -= 1;
                this.startTimer();
                if (allmartOrder == true)
                {
                    allmartBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
                }
                else
                {
                    localFoodBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
                }
            }
            else if (money < 100 && (currentPurchaseName == "Chair1" || currentPurchaseName == "Chair2" || currentPurchaseName == "Chair3" || currentPurchaseName == "Shelves3"))
            {
                errorInventory.gameObject.SetActive(true);
                errorMessage.text = "You don't have enough money for this!";
            }
            else if (money >= 150 && currentPurchaseName == "Jukebox")
            {
                money -= 150;
                PlayerPrefs.SetInt("money", money);
                ownedAppliances.Add(currentPurchaseName);
                PlayerPrefs.SetString("ownedItems_" + (ownedAppliances.Count - 1), currentPurchaseName);
                inventorySpace -= 1;
                this.startTimer();
                if (allmartOrder == true)
                {
                    allmartBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("allmartBeingDelivered_" + (allmartBeingDelivered.Count - 1), currentPurchaseName);
                }
                else
                {
                    localFoodBeingDelivered.Add(currentPurchaseName);
                    PlayerPrefs.SetString("localFoodBeingDelivered_" + (localFoodBeingDelivered.Count - 1), currentPurchaseName);
                }
            }
            else if (money < 150 && currentPurchaseName == "Jukebox")
            {
                errorInventory.gameObject.SetActive(true);
                errorMessage.text = "You don't have enough money for this!";
            }

            //unlock an unlockable if the player is purchasing an eco-appliance
            if (currentPurchaseName == "Laundry" && money >= 200 && allmartOrder == false)
            {
                unlockables[9].Unlock();
                unlocked.Add(unlockables[9]);
                unlockableFlashGo = true;
            }
            if (currentPurchaseName == "Dishwasher" && money >= 500 && allmartOrder == false)
            {
                unlockables[10].Unlock();
                unlocked.Add(unlockables[10]);
                unlockableFlashGo = true;
            }
            if (currentPurchaseName == "Solarpanels" && money >= 1000 && allmartOrder == false)
            {
                unlockables[11].Unlock();
                unlocked.Add(unlockables[11]);
                unlockableFlashGo = true;
            }
        }
        //if the player is hungry, they cannot purchase non-food iteams
        else if (hungerEmpty == true && !placedInventoryItems.Contains(currentPurchaseName) && !inventory.Contains(currentPurchaseName))
        {
            //an error message is displayed
            errorInventory.gameObject.SetActive(true);
            errorMessage.text = "You are very hungry, maybe spend your money on food...";
        }
        //if the player already owns a furniture iteam they cannot purchase is again
        else if (placedInventoryItems.Contains(currentPurchaseName) || inventory.Contains(currentPurchaseName))
        {
            //an error message is displayed
            errorInventory.gameObject.SetActive(true);
            errorMessage.text = "You already own this!";
        }
    }
    
    public void purchaseOrError()
    {
        //if there are more than 0 fridge spaces available 
        if (emptyFridgeSpaces > 0)
        {
            //open the purchase canvas
            purchase.gameObject.SetActive(true);
        }
        //if there are 0 fridge spaces available
        else
        {
            //show the player an error message
            errorInventory.gameObject.SetActive(true);
            errorMessage.text = "Oops!\nYour fridge is full.";
        }
    }

    public void setAllmartBool(bool AB)
    {
        allmartOrder = AB;
    }

    public void purchaseAppliance(string CPN)
    {
        currentPurchaseName = CPN;

        //if the list of owned appliances already contains the currentPurchaseName, the player cannot purchase it
        if (ownedAppliances.Contains(currentPurchaseName) == true)
        {
            //show the player an error message
            errorInventory.gameObject.SetActive(true);
            errorMessage.text = "Oops!\nYou already own this.";
        }
        //if the player does not already own the currentPurchaseName
        else
        {
            //open the purchase canvas
            purchase.gameObject.SetActive(true);
        }
    }

    public void purchaseFurniture()
    {
        //if there are more than 0 inventory spaces available 
        if (inventorySpace > 0)
        {
            // open the purchase canvas
            purchase.gameObject.SetActive(true);
        }
        //if there are 0 fridge spaces available
        else
        {
            //show the player an error message
            errorInventory.gameObject.SetActive(true);
            errorMessage.text = "Oops!\nYour inventory is full.";
        }
            
    }

    public void changeCurrentPurchaseName(string CPN)
    {
        currentPurchaseName = CPN;
    }

}