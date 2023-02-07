using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Emails : MonoBehaviour
{
    public GameObject emailText;
    public GameObject emailSubject;
    public bool unreadEmail = false;
    public GM gameManager;

    void Start()
    {
        //get currently displayed email from PlayerPrefs
        if (PlayerPrefs.HasKey("currentEmail"))
        {
            gameManager.email = PlayerPrefs.GetInt("currentEmail");
        }
    }
    public void Update()
    {
        //email 1 = recycling 1
        if (gameManager.email == 1)
        {
            emailSubject.GetComponent<TextMeshProUGUI>().text = "Subj: Recycling";
            emailText.GetComponent<TextMeshProUGUI>().text =
                "Hey again, it seems like you figured out the recycling, well done. " +
                "Be careful not to throw any of that into the trash can. " +
                "You may also have to clean up some dirty dishes and clothes lying around... but I'm sure you'll figure that one out without a pdf. " +
                "Now that you've started to get the hang of living here, I think you can expect " +
                "a guest or two to start coming to hang around. Don't be afraid, they may even bring you a little something " +
                "as a thanks for your hospitality.";
        }

        //email 2 = first time playing
        if (gameManager.email == 2)
        {
            emailSubject.GetComponent<TextMeshProUGUI>().text = "Subj: You must be new here...";
            emailText.GetComponent<TextMeshProUGUI>().text =
                "Hey there, I'm the previous tenant of your new apartment. " +
                "I've heard from some friends that you're settling in nicely. " +
                "I left this old computer here so I can let you know about the house rules. " +
                "The most I can tell you right now is be sure to check out the recycling document I've left you, " +
                "and try not to stray too far from it. You may get some visitors leaving some trash around. " +
                "If you work on separating your recycling, you may just get to meet a few.";
        }

        //email 3 = solar panel purchase
        if (gameManager.email == 3)
        {
            emailSubject.GetComponent<TextMeshProUGUI>().text = "Subj: Solar Panels";
            emailText.GetComponent<TextMeshProUGUI>().text = "Hey there, it's me again. " +
                "I heard through the vine that you got solar panels! Nice one. I'm sure our little friends will appreciate that. " +
                "I always wanted to but could never save up the money. And even if I had, I'm sure Ma & Pop would be out of stock! " +
                "You're gonna make some good money back on that investment, and you may get to meet someone I never got the chance to...";
        }

        //email 4 = first purchase from M&P
        if (gameManager.email == 4)
        {
            emailSubject.GetComponent<TextMeshProUGUI>().text = "Subj: Ma & Pop";
            emailText.GetComponent<TextMeshProUGUI>().text = "Hiya! Do you get any other emails or is it all from me? " +
                "I see you discovered Ma & Pop. That was my favourite little shop to buy from. It may take a while to get to you, " +
                "and the stock might be extremely limited, " +
                "but it really is worth it. They're good people who really care about taking care of the planet. You and I " +
                "both know how important that is around here ;)";
        }

        //email 5 = laundry purchase
        if (gameManager.email == 5)
        {
            emailSubject.GetComponent<TextMeshProUGUI>().text = "Subj: Eco laundry";
            emailText.GetComponent<TextMeshProUGUI>().text = "Hey there friend. Can I call you friend? " +
                "That eco laundry machine you just got is definitely gonna save you some money on your water bills. " +
                "You got it from Ma & Pop right? " +
                "Hope so, or the little fellas won't be too pleased.";
        }

        //email 6 = dishwasher purchase
        if (gameManager.email == 6)
        {
            emailSubject.GetComponent<TextMeshProUGUI>().text = "Subj: Eco dishwasher";
            emailText.GetComponent<TextMeshProUGUI>().text = "Greetings. I'm in the mood to be formal today. " +
                "I am of the understanding that you just procured an eco dishwasher? " +
                "I am please to hear this news. I do hope you aquired it from the great Ma & Pop. " +
                "If not, I would be quite disappointed. As would our mutual friends.\n\n" +
                "Regards,\n" +
                "Previous tenant";
        }

        //email 7 = trashed an inventory item for the first time
        if (gameManager.email == 6)
        {
            emailSubject.GetComponent<TextMeshProUGUI>().text = "Subj: Oh by the way...";
            emailText.GetComponent<TextMeshProUGUI>().text = "Hey there, hope you're managing your space well. " +
                "Just so you know, the inventory you've got only has 6 slots, so be careful not to fill them up carelessly. " +
                "If you need to get rid of something, the donation squad comes around once a day, " +
                "and it's much better for the planet and other people than sending new, unused items to the landfill.\n\n" +
                "Just something to consider.";
        }

        //email 8 = donated an inventory item for the first time
        if (gameManager.email == 6)
        {
            emailSubject.GetComponent<TextMeshProUGUI>().text = "Subj: Donation celebration";
            emailText.GetComponent<TextMeshProUGUI>().text = "I heard you're getting a visit from the donation squad tomorrow! " +
                "I know waiting for a pick-up can be a bit tedious, especially if Ma&Pop have something you want to buy right now " +
                "(I've been there), but utilizing donation is so much better for the planet and your little friends will be happy with you. " +
                "Even if Ma&Pop don't have what you want in stock tomorrow, it'll be back soon. At least that piece of furniture " +
                "won't be in the landfill for ~10,000 years! It'll go to someone who needs it.";
        }
    }
}
