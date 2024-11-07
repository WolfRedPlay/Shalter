using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultWindow : MonoBehaviour
{
    [SerializeField] Transform playersInBunker;
    [SerializeField] Transform playersNotInBunker;

    [SerializeField] GameObject playerPrefab;

    [SerializeField] GameObject playerListsWindow;

    [SerializeField] StartMenu startMenu;

    [SerializeField] Image familyModeForInBunker;
    [SerializeField] Image familyModeForOutBunker;



    public void ShowResultWindow(List<PlayerData> inBunker, List<PlayerData> notInBunker, bool familyMode)
    {
        this.gameObject.SetActive(true);
        playerListsWindow.SetActive(true);

        foreach (var player in inBunker)
        {
            GameObject newPlayerInBunker = Instantiate(playerPrefab, playersInBunker);
            PlayerCard newPlayerCard = newPlayerInBunker.GetComponent<PlayerCard>();
            newPlayerCard.Data = player;
            newPlayerCard.Data.RevealAllCards();
            
        }

        foreach (var player in notInBunker)
        {
            GameObject newPlayerInBunker = Instantiate(playerPrefab, playersNotInBunker);
            PlayerCard newPlayerCard = newPlayerInBunker.GetComponent<PlayerCard>();
            newPlayerCard.Data = player;
            newPlayerCard.Data.RevealAllCards();
        }

        if (familyMode)
        {
            familyModeForInBunker.gameObject.SetActive(true);
            familyModeForOutBunker.gameObject.SetActive(true);

            if (CheckForFertilePair(inBunker)) familyModeForInBunker.color = Color.green;
            else familyModeForInBunker.color = Color.red;

            if (CheckForFertilePair(notInBunker)) familyModeForOutBunker.color = Color.green;
            else familyModeForOutBunker.color = Color.red;

        }

    }

    private bool CheckForFertilePair(List<PlayerData> playersToCheck)
    {
        bool maleChecker = false;
        bool femaleChecker = false;

        foreach (PlayerData player in playersToCheck)
        {


            if (player.PlayerGender == Gender.MALE)
            {
                if (maleChecker) continue;
                else if (player.Fertile)
                    maleChecker = true;
            }

            if (player.PlayerGender == Gender.FEMALE)
            {
                if (femaleChecker) continue;
                else if (player.Fertile)
                    femaleChecker = true;
            }
        }

        if (femaleChecker && maleChecker) return true;
        else return false;
    }



    public void RestartGame()
    {
        startMenu.GatesClose();
    }
}
