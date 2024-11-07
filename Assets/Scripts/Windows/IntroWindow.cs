using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroWindow : MonoBehaviour
{
    [SerializeField] GameObject catastropheWindow;
    [SerializeField] GameObject bunkerWindow;

    [SerializeField] CardData catastropheCard;

    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform bunkerCards;

    [SerializeField] Menu menu;
    [SerializeField] GameManager gameManager;

    [SerializeField] ScreensTransition transition;

    public void StartIntro(Catastrophe newCatastrophe, BunkerCard[] newBunkerCards)
    {
        bunkerWindow.SetActive(false);
        catastropheCard.SetCardData(newCatastrophe);
        for (int i = 0; i < newBunkerCards.Length; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, bunkerCards);
            newCard.GetComponent<CardData>().SetCardData(newBunkerCards[i]);
        }
        int check = Random.Range(1, 7);
        int monthesInBunker = check * 3;
        if (check == 1) monthesInBunker = 4;
        if (check == 5) monthesInBunker = 18;
        if (check == 6) monthesInBunker = 24;

        GameObject newTimeCard = Instantiate(cardPrefab, bunkerCards);
        CardData newTimeCardData = newTimeCard.GetComponent<CardData>();
        newTimeCardData.SetName("¬рем€ в бункере");
        newTimeCardData.SetType("Ѕункер");
        string desciptionForTimeCard = "";
        int years = monthesInBunker / 12;
        if (years != 0)
        {
            if (years == 1) desciptionForTimeCard += years.ToString() + " год ";
            else desciptionForTimeCard += years.ToString() + " года";
        }

        int monthes = monthesInBunker % 12;
        if (monthes != 0)
        {
            if (monthes == 4)
                desciptionForTimeCard += monthes.ToString() + " мес€ца";
            else
                desciptionForTimeCard += monthes.ToString() + " мес€цев";
        }

        newTimeCardData.SetDescription(desciptionForTimeCard);

        menu.SetNewBunkerInfo(newCatastrophe, newBunkerCards, monthesInBunker);
        catastropheWindow.SetActive(true);
        this.gameObject.SetActive(true);
    }


    public void ProccedToBunkerCards()
    {
        transition.SetTransitionMiddleAction(ChangeWindowToBunkerCards);
        transition.StartTransition();
    }

    public void ChangeWindowToBunkerCards()
    {
        catastropheWindow.SetActive(false);
        bunkerWindow.SetActive(true);
    }



    public void FinishIntro()
    {
        transition.SetTransitionMiddleAction(ChangeWindowToReveal);
        transition.StartTransition();

    }

    public void ChangeWindowToReveal()
    {
        this.gameObject.SetActive(false);
        gameManager.StartRevealing();
    }
}
