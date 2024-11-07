using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardData : MonoBehaviour
{
    Card card;

    [SerializeField] TMP_Text typeText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] Image icon;


    public void SetCardData(Card newCardData)
    {
        card = newCardData;
        nameText.text = card.Name;
        icon.gameObject.SetActive(false);
        switch (card.CardType)
        {
            case CardType.CATASTROPHE:
                typeText.text = "����������";
                Catastrophe newCatastrophe = (Catastrophe)card;
                descriptionText.text = newCatastrophe.Description;
                break;

            case CardType.BUNKER:
                typeText.text = "������";
                BunkerCard newBunker = (BunkerCard)card;
                descriptionText.text = newBunker.Description;
                break;

            case CardType.HEALTH:
                typeText.text = "��������";
                HealthCard newHealth = (HealthCard)card;
                icon.gameObject.SetActive(true);
                icon.sprite = newHealth.CardIcon;
                if (newHealth.HasStage)
                {
                    nameText.text += $" ({newHealth.Stage})";
                }
                break;
                
            
            case CardType.JOB:
                typeText.text = "������";
                JobCard newJob = (JobCard)card;
                icon.gameObject.SetActive(true);
                icon.sprite = newJob.CardIcon;
                
                nameText.text += $" ({newJob.Exp})";
                break;
                            
            case CardType.HOBBY:
                typeText.text = "�����";
                HobbyCard newHobby = (HobbyCard)card;
                icon.gameObject.SetActive(true);
                icon.sprite = newHobby.CardIcon;

                nameText.text += $" ({newHobby.Exp})";
                break;
                            
            case CardType.BAGGAGE:
                typeText.text = "�����";
                BaggageCard newBagggage = (BaggageCard)card;
                icon.gameObject.SetActive(true);
                icon.sprite = newBagggage.CardIcon;
                break;
                            
            case CardType.FACT:
                typeText.text = "����";
                FactCard newFact = (FactCard)card;
                icon.gameObject.SetActive(true);
                icon.sprite = newFact.CardIcon;
                break;
                            
            case CardType.BIOLOGY:
                typeText.text = "��������";
                BiologyCard newBiology = (BiologyCard)card;
                icon.gameObject.SetActive(true);
                icon.sprite = newBiology.CardIcon;
                break;
            
            case CardType.SPECIAL:
                typeText.text = "������ �������";
                SpecialCard newSpecial = (SpecialCard)card;
                descriptionText.text = newSpecial.description;
                
                break;

        }
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetDescription(string description)
    {
        descriptionText.text = description;
        descriptionText.enableWordWrapping = false;
    }

    public void SetType(string type)
    {
        typeText.text = type;
    }
}
