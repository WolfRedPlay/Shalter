using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] TMP_Text nickname;
    [SerializeField] TMP_Text age;
    [SerializeField] Image gender;
    [SerializeField] TMP_Text job;
    [SerializeField] TMP_Text health;
    [SerializeField] TMP_Text hobby;
    [SerializeField] TMP_Text baggage;
    [SerializeField] TMP_Text extraBaggage;
    [SerializeField] TMP_Text fact;

    [SerializeField] GameObject voicesIcon;
    [SerializeField] TMP_Text voices;

    PlayerData playerData;

    public PlayerData Data { 
        
        set 
        { 
            playerData = value;
            playerData.Card = this;
            Name = value.PlayerName;
        }
        get { return playerData; } 
    }


    public string Name
    {
        set
        {
            nickname.text = value;
        }

        get { return playerData.PlayerName; }

    }


    public int Voices
    {
        set
        {
            playerData.Voices = value;
            if (value == 0)
            {
                voicesIcon.SetActive(false);
                voices.text = "";
            }
            else
            {
                voicesIcon.SetActive(true);
                voices.text = (value).ToString();
            }
        }

        get
        {
            return playerData.Voices;
        }
    }

    public void CleanPlayerText()
    {
        nickname.text = "";
        age.text = "";
        gender.sprite = null;
        job.text = "";
        health.text = "";
        hobby.text = "";
        baggage.text = "";
        fact.text = "";
        playerData = new PlayerData();
        playerData.Card = this;
        playerData.SetID(Random.Range(0, 100000000));
    }

    public void AddExtraBaggage(BaggageCard extraBaggageCard)
    {
        extraBaggage.gameObject.SetActive(true);
        extraBaggage.text = extraBaggageCard.Name;
    }

    public void RevealCard(CardType cardType)
    {
        string stageTxt = "";
        switch (cardType)
        {
            case CardType.BIOLOGY:
                BiologyCard bioCard = (BiologyCard)playerData.HandCard[cardType];
                age.text = $"{bioCard.Age}";
                gender.color = Color.white;
                gender.sprite = bioCard.GenderIcon;
                break;

            case CardType.JOB:
                JobCard jobCard = (JobCard)playerData.HandCard[cardType];
                stageTxt = $" ({jobCard.Exp})";
                job.text = "- " + jobCard.Name + stageTxt;
                break;
            case CardType.HEALTH:
                HealthCard healthCard = (HealthCard)playerData.HandCard[cardType];
                if (healthCard.HasStage)
                {
                    stageTxt = $" ({healthCard.Stage})";
                }
                else stageTxt = string.Empty;

                health.text = "- " + healthCard.Name + stageTxt;
                break;
            case CardType.HOBBY:
                HobbyCard hobbyCard = (HobbyCard)playerData.HandCard[cardType];
                stageTxt = $" ({hobbyCard.Exp})";
                hobby.text = "- " + hobbyCard.Name + stageTxt;
                break;
            case CardType.BAGGAGE:
                BaggageCard baggageCard = (BaggageCard)playerData.HandCard[cardType];
                baggage.text = "- " + baggageCard.Name;
                break;
            case CardType.FACT:
                FactCard factCard = (FactCard)playerData.HandCard[cardType];
                fact.text = factCard.Name;
                break;
        }
    }


    public void ResizeTexts(float multiplier)
    {
        float maxFontSize = 24 * multiplier;
        job.fontSizeMax = maxFontSize;
        health.fontSizeMax = maxFontSize;
        hobby.fontSizeMax = maxFontSize;
        baggage.fontSizeMax = maxFontSize;
        extraBaggage.fontSizeMax = maxFontSize;
    }
}
