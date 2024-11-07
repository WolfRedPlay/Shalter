using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject inputWindow;


    [SerializeField] ConditionsWindow conditionsWindow;
    [SerializeField] GameObject infoList;
    [SerializeField] GameObject cardsListForSpecialCards;
    [SerializeField] CardData catastropheCard;

    [SerializeField] Animator gatesAnimator;


    [SerializeField] PauseWindow pauseWindow;
    [SerializeField] List<Timer> timers;


    [SerializeField] ScreensTransition transition;

    public void CloseGates()
    {
        if (inputWindow.activeInHierarchy) inputWindow.SetActive(false);
    }

    public void OpenInputWindow()
    {
        gatesAnimator.SetBool("GameStarted", true);
        inputWindow.SetActive(true);
    }

    public void OpenStartWindow()
    {
        gatesAnimator.SetBool("GameStarted", false);
    }

    public void OpenConditionsWindow()
    {
        conditionsWindow.gameObject.SetActive(true);
        conditionsWindow.OpenWindow();
    }
    
    public void CloseConditionsWindow()
    {
        conditionsWindow.CloseWindow();
    }

    public void SetNewBunkerInfo(Catastrophe newCatastrophe, BunkerCard[] newBunkerCards, int monthesInBunker)
    {
        catastropheCard.SetCardData(newCatastrophe);

        List<TMP_Text> bunkerCardsText = infoList.GetComponentsInChildren<TMP_Text>().ToList();
        List<CardData> bunkerCardsTextForSpecialCards = cardsListForSpecialCards.GetComponentsInChildren<CardData>().ToList();

        for(int i = 0; i < newBunkerCards.Length; i++)
        {
            bunkerCardsText[i].text = newBunkerCards[i].Name;
            bunkerCardsTextForSpecialCards[i].SetCardData(newBunkerCards[i]);
        }
        bunkerCardsText[bunkerCardsText.Count - 1].text = "¬рем€ в бункере: ";
        int years = monthesInBunker / 12;
        if (years != 0)
        {
            if (years == 1) bunkerCardsText[bunkerCardsText.Count - 1].text += years.ToString() + " год ";
            else bunkerCardsText[bunkerCardsText.Count - 1].text += years.ToString() + " года";
        }

        int monthes = monthesInBunker % 12;
        if (monthes != 0)
        {
            if (monthes == 4)
                bunkerCardsText[bunkerCardsText.Count - 1].text += monthes.ToString() + " мес€ца";
            else
                bunkerCardsText[bunkerCardsText.Count - 1].text += monthes.ToString() + " мес€цев";
        }


    }


    public void OpenPauseWindow()
    {
        pauseWindow.gameObject.SetActive(true);
        pauseWindow.OpenWindow();
        foreach (Timer timer in timers)
        {
            timer.PauseTimer();
        }
    }

     public void ClosePauseWindow()
     {
         pauseWindow.CloseWindow();
         foreach (Timer timer in timers)
         {
             timer.ResumeTimer();
         }
    }
    


    public void ChangeBunkerCard(BunkerCard newBunkerCard, int oldCardIndex)
    {
        List<TMP_Text> bunkerCardsText = infoList.GetComponentsInChildren<TMP_Text>().ToList();
        List<CardData> bunkerCardsTextForSpecialCards = cardsListForSpecialCards.GetComponentsInChildren<CardData>().ToList();

        bunkerCardsText[oldCardIndex].text = newBunkerCard.Name;
        bunkerCardsTextForSpecialCards[oldCardIndex].SetCardData(newBunkerCard);
    }
}
