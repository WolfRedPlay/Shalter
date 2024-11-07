using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RevealCardMenu : MonoBehaviour
{
    //Cards Buttons
    [SerializeField] Button jobButton;
    [SerializeField] Button bioButton;
    [SerializeField] Button healthButton;
    [SerializeField] Button hobbyButton;
    [SerializeField] Button baggageButton;
    [SerializeField] Button factButton;
    [SerializeField] Timer timer;
    [SerializeField] GameManager gameManager;


    //Windows
    [SerializeField] GameObject choiceWindow;
    [SerializeField] GameObject speechWindow;
    [SerializeField] GameObject currentPlayerWindow;


    [SerializeField] TMP_Text currentPlayerName;
    [SerializeField] TMP_Text cardsToRevealAmountText;

    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardList;

    PlayerData currentPlayer;

    List<CardType> chosenCards = new List<CardType>();

    int cardsToRevel = 0;

    CardType mandatoryCard;


    [SerializeField] ScreensTransition transition;


    public void StartTurn(PlayerData player, int cardsToReveal, CardType mandatoryCard)
    {
        this.gameObject.SetActive(true);
        chosenCards.Clear();
        currentPlayer = player;
        currentPlayerName.text = currentPlayer.PlayerName;
        cardsToRevealAmountText.text = cardsToReveal.ToString();
        RefreshButtons();
        this.mandatoryCard = mandatoryCard;

        jobButton.gameObject.SetActive(false);
        bioButton.gameObject.SetActive(false);
        healthButton.gameObject.SetActive(false);
        hobbyButton.gameObject.SetActive(false);
        baggageButton.gameObject.SetActive(false);
        factButton.gameObject.SetActive(false);

        if (mandatoryCard == CardType.NONE || currentPlayer.OpenCard[mandatoryCard])
        {
            if (!currentPlayer.OpenCard[CardType.JOB]) jobButton.gameObject.SetActive(true);
            if (!currentPlayer.OpenCard[CardType.BIOLOGY]) bioButton.gameObject.SetActive(true);
            if (!currentPlayer.OpenCard[CardType.HEALTH]) healthButton.gameObject.SetActive(true);
            if (!currentPlayer.OpenCard[CardType.HOBBY]) hobbyButton.gameObject.SetActive(true);
            if (!currentPlayer.OpenCard[CardType.BAGGAGE]) baggageButton.gameObject.SetActive(true);
            if (!currentPlayer.OpenCard[CardType.FACT]) factButton.gameObject.SetActive(true);
            this.mandatoryCard = CardType.NONE;
        }
        else
        {
            switch (mandatoryCard)
            {
                case CardType.HEALTH:
                    healthButton.gameObject.SetActive(true);
                    break;

                case CardType.HOBBY:
                    hobbyButton.gameObject.SetActive(true);
                    break;

                case CardType.BIOLOGY:
                    bioButton.gameObject.SetActive(true);
                    break;

                case CardType.BAGGAGE:
                    baggageButton.gameObject.SetActive(true);
                    break;

                case CardType.FACT:
                    factButton.gameObject.SetActive(true);
                    break;

                case CardType.JOB:
                    jobButton.gameObject.SetActive(true);
                    break;
            }
            if (cardsToReveal == 2)
            {
                if (!currentPlayer.OpenCard[CardType.JOB]) jobButton.gameObject.SetActive(true);
                if (!currentPlayer.OpenCard[CardType.BIOLOGY]) bioButton.gameObject.SetActive(true);
                if (!currentPlayer.OpenCard[CardType.HEALTH]) healthButton.gameObject.SetActive(true);
                if (!currentPlayer.OpenCard[CardType.HOBBY]) hobbyButton.gameObject.SetActive(true);
                if (!currentPlayer.OpenCard[CardType.BAGGAGE]) baggageButton.gameObject.SetActive(true);
                if (!currentPlayer.OpenCard[CardType.FACT]) factButton.gameObject.SetActive(true);
            }
        }


        speechWindow.SetActive(false);
        choiceWindow.SetActive(true);
        for (int i = 0; i < cardList.childCount; i++)
        {
            Destroy(cardList.GetChild(i).gameObject);
        }


        this.cardsToRevel = cardsToReveal;


    }

    public void ShowCurrentPlayerWindow()
    {
        currentPlayerWindow.GetComponent<CurrentPlayerWindow>().SetPlayerName(currentPlayer.PlayerName);
        currentPlayerWindow.SetActive(true);
        currentPlayerWindow.GetComponent<CurrentPlayerWindow>().StartBlendOut(Times.CurrentPlayerTime);
    }


    public void RevealCard(int typeInt)
    {
        CardType type = (CardType)typeInt;
        currentPlayer.RevealCard(type);

        switch (type)
        {
            case CardType.BIOLOGY: 
                bioButton.interactable = false;
                chosenCards.Add(CardType.BIOLOGY);
                break;

            case CardType.HEALTH: 
                healthButton.interactable = false;
                chosenCards.Add(CardType.HEALTH);
                break;

            case CardType.JOB: 
                jobButton.interactable = false;
                chosenCards.Add(CardType.JOB);
                break;

            case CardType.HOBBY: 
                hobbyButton.interactable = false;
                chosenCards.Add(CardType.HOBBY);
                break;

            case CardType.FACT: 
                factButton.interactable = false;
                chosenCards.Add(CardType.FACT);
                break;

            case CardType.BAGGAGE: 
                baggageButton.interactable = false;
                chosenCards.Add(CardType.BAGGAGE);
                break;
        }


        if (chosenCards.Count == cardsToRevel)
        {
            if (mandatoryCard != CardType.NONE)
            {
                bool checker = false;
                foreach (CardType card in chosenCards) 
                {
                    if (card == mandatoryCard) checker = true;
                }
                if (!checker)
                {
                    chosenCards.Clear();
                    RefreshButtons();
                    return;
                }
            }
            foreach (CardType cardType in chosenCards)
            {
                GameObject newCard = Instantiate(cardPrefab, cardList);
                newCard.GetComponent<CardData>().SetCardData(currentPlayer.HandCard[cardType]);
            }

            transition.SetTransitionMiddleAction(ChangeWindowToSpeech);
            transition.SetTransitionFinalAction(SetTimer);
            transition.StartTransition();
        }

    }

    public void ChangeWindowToSpeech()
    {
        choiceWindow.SetActive(false);
        speechWindow.SetActive(true);
    }

    public void SetTimer()
    {
        timer.StartTimer(Times.SpeechTime, gameManager.FinishSpeech);
    }

    private void RefreshButtons()
    {
        bioButton.interactable = true;
        jobButton.interactable = true;
        healthButton.interactable = true;
        hobbyButton.interactable = true;
        factButton.interactable = true;
        baggageButton.interactable = true;
    }
    
}
