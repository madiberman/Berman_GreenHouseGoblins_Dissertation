using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DailyReward : MonoBehaviour
{
    public GM gameManager;
    int dailyIncome = 20 ;
    public TextMeshProUGUI waterBill;
    public TextMeshProUGUI solarGains;
    public TextMeshProUGUI solarGainsValue;
    public TextMeshProUGUI total;
    int solarIncomeToday;
    int waterBillValue;
    int dailyIncomeWithSolar;
    int waterBillToday;
    int dailyIncomeWithLaundryOrDishwasher;
    int dailyIncomeWithLaundryAndDishwasher;
    int dailyIncomeWithLaundryOrDishwasherSOLAR;
    int dailyIncomeWithLaundryAndDishwasherSOLAR;
    int dailyIncomeNothing;

    private void Start()
    {
        //set the amount of solar gains for the day between 10 and 19, even if the player hasn't purchased solar panels
        solarIncomeToday = UnityEngine.Random.Range(10, 20);

        //set all income amounts depending on which appliances are owned
        //no appiances
        dailyIncomeNothing = 20 - 10;
        //only solar panels (no dish washer or laundry)
        dailyIncomeWithSolar = dailyIncome + solarIncomeToday - 10;
        //laundry OR dish washer (no solar panels)
        dailyIncomeWithLaundryOrDishwasher = dailyIncome - 6;
        //laundry AND dish washer (no solar panels)
        dailyIncomeWithLaundryAndDishwasher = dailyIncome - 3;
        //Solar AND (laundry OR dish washer)
        dailyIncomeWithLaundryOrDishwasherSOLAR = dailyIncomeWithLaundryOrDishwasher + solarIncomeToday;
        //Solar AND laundry AND dish washer
        dailyIncomeWithLaundryAndDishwasherSOLAR = dailyIncomeWithLaundryAndDishwasher + solarIncomeToday;

    }

    private void Update()
    {
        //set daily income and water bill text based on which appliances are owned
        if (gameManager.ownedAppliances.Contains("Laundry") && !gameManager.ownedAppliances.Contains("Dishwasher") && !gameManager.ownedAppliances.Contains("Solarpanels"))
        {
            dailyIncome = dailyIncomeWithLaundryAndDishwasher;
            waterBill.text = "6";
        }
        else if (gameManager.ownedAppliances.Contains("Dishwasher") && !gameManager.ownedAppliances.Contains("Laundry") && !gameManager.ownedAppliances.Contains("Solarpanels"))
        {
            dailyIncome = dailyIncomeWithLaundryOrDishwasher;
            waterBill.text = "6";
        }
        else if (gameManager.ownedAppliances.Contains("Dishwasher") && gameManager.ownedAppliances.Contains("Laundry") && !gameManager.ownedAppliances.Contains("Solarpanels"))
        {
           dailyIncome = dailyIncomeWithLaundryOrDishwasher;
           waterBill.text = "3";
        }
        else if (gameManager.ownedAppliances.Contains("Dishwasher") && gameManager.ownedAppliances.Contains("Laundry") && gameManager.ownedAppliances.Contains("Solarpanels"))
        {
            solarGains.text = "Solar\nGains:";
            solarGainsValue.text = solarIncomeToday.ToString();
            dailyIncome = dailyIncomeWithLaundryOrDishwasherSOLAR;
            waterBill.text = "3";
        }
        else if (!gameManager.ownedAppliances.Contains("Dishwasher") && gameManager.ownedAppliances.Contains("Laundry") && gameManager.ownedAppliances.Contains("Solarpanels"))
        {
            solarGains.text = "Solar\nGains:";
            solarGainsValue.text = solarIncomeToday.ToString();
            dailyIncome = dailyIncomeWithLaundryOrDishwasherSOLAR;
            waterBill.text = "6";
        }
        else if (gameManager.ownedAppliances.Contains("Dishwasher") && !gameManager.ownedAppliances.Contains("Laundry") && gameManager.ownedAppliances.Contains("Solarpanels"))
        {
            solarGains.text = "Solar\nGains:";
            solarGainsValue.text = solarIncomeToday.ToString();
            dailyIncome = dailyIncomeWithLaundryOrDishwasherSOLAR;
            waterBill.text = "6";
        }
        else if (gameManager.ownedAppliances.Contains("Solarpanels") && !gameManager.ownedAppliances.Contains("Laundry") && !gameManager.ownedAppliances.Contains("Dishwasher"))
        {
            solarGains.text = "Solar\nGains:";
            solarGainsValue.text = solarIncomeToday.ToString();
            dailyIncome = dailyIncomeWithSolar;
            total.text = dailyIncome.ToString();
            waterBill.text = "10";
        }
        else
        {
            dailyIncome = dailyIncomeNothing;
            waterBill.text = "10";
        }

    }

    //when the button of the daily reward canvas is closed
    public void closeButton()
    {
        //add the income to the player's money
        gameManager.money += dailyIncome;
        PlayerPrefs.SetInt("money", gameManager.money);

        //set the time of the next daily reward to 24 hours from the current time
        gameManager.nextDailyReward = DateTime.Now.AddSeconds(86400);
        PlayerPrefs.SetString("nextDailyReward", gameManager.nextDailyReward.ToString());

    }
}
