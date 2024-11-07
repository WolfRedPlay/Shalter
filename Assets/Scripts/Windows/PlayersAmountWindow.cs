using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayersAmountWindow : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameManager gameManager;

    [SerializeField] Toggle checkBox;


    [SerializeField] StartMenu startMenu;

    [SerializeField] ScreensTransition transition;


    bool familyMode = false;
    int playersAmount = 0;

    public void StartGame()
    {
        //startMenu.StartTransition();
        playersAmount = int.Parse(inputField.text);
        gameManager.FamilyMode = familyMode;
        Transition();
    }

    public void ChangeWindows()
    {
        gameObject.SetActive(false);
        gameManager.StartGame(playersAmount);
    }

    public void Transition()
    {
        transition.SetTransitionMiddleAction(ChangeWindows);
        transition.StartTransition();
    }

    public void MiddleTransition()
    {
        int playersAmount = int.Parse(inputField.text);
        gameObject.SetActive(false);
        gameManager.FamilyMode = familyMode;
        gameManager.StartGame(playersAmount);
    }

    public void IncreaseAmountOfPlayers()
    {
        int amount = int.Parse(inputField.text);
        amount++;
        inputField.text = amount.ToString();
    }

    public void DecreaseAmountOfPlayers()
    {
        int amount = int.Parse(inputField.text);
        amount--;
        inputField.text = amount.ToString();
    }

    public void CheckPlayersAmount()
    {
        int amount = int.Parse(inputField.text);
        if (amount >= 15)
        {
            amount = 15;

        }
        if (amount <= 4)
        {
            amount = 4;

        }

        inputField.text = amount.ToString();
    }


    public void ChangeFamilyMode()
    {
        familyMode = !checkBox.isOn;
    }
}
