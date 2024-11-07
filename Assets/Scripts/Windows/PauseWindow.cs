using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : MonoBehaviour
{
    [SerializeField] GameObject startButtonsWindow;
    [SerializeField] GameObject playerChoiceWindow;
    [SerializeField] GameObject cardChoiceWindow;

    GameObject buttonsWindow;

    [SerializeField] GameObject playerButtonPrefab;

    [SerializeField] GameManager gameManager;
    [SerializeField] Transform playerList;
    [SerializeField] Transform cardsWindow;
    [SerializeField] UsedSpecialCardWindow usedCardWindow;

    [SerializeField] Animator animator;
    //PlayerData usedPlayer;
    //int usedCardIndex;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        buttonsWindow = cardChoiceWindow.transform.GetChild(0).gameObject;
    }

    public void OpenWindow()
    {
        animator.SetBool("Open", true);
    }
    
    public void CloseWindow()
    {
        startButtonsWindow.SetActive(false);
        playerChoiceWindow.SetActive(false);
        cardChoiceWindow.SetActive(false);

        animator.SetBool("Open", false);
    }



    public void OpenStartButtonsWindow()
    {
        foreach (PlayerData player in gameManager.CurrentPlayers)
        {
            bool checker = false;

            foreach (SpecialCard card in player.SpecialCards)
            {
                if (card.CheckPossibilityToUse())
                {
                    checker = true;
                    break;
                }
            }
            if (checker)
            {
                GameObject newButton = Instantiate(playerButtonPrefab, playerList);
                newButton.GetComponent<PlayerButton>().PlayerData = player;
                newButton.GetComponent<Button>().onClick.AddListener(() => OpenCardChoiceWindow(newButton.transform.position, newButton.GetComponent<PlayerButton>().PlayerData));
            }
           
        }

        startButtonsWindow.SetActive(true);
        playerChoiceWindow.SetActive(false);
        cardChoiceWindow.SetActive(false);
    }

    public void OpenPlayerChoiceWindow()
    {
        startButtonsWindow.SetActive(false);
        playerChoiceWindow.SetActive(true);

    }

    public void ClosePlayerChoiceWindow()
    {
        playerChoiceWindow.SetActive(false);
        startButtonsWindow.SetActive(true);
    }

    public void OpenCardChoiceWindow(Vector3 newPosition, PlayerData playerData)
    {
        List<Button> buttons = cardsWindow.GetComponentsInChildren<Button>(true).ToList();

        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }


        List<int> avaliableCardsIndexes = new List<int>();

        for (int i = 0; i < playerData.SpecialCards.Count; i++)
        {
            if (playerData.SpecialCards[i].CheckPossibilityToUse()) avaliableCardsIndexes.Add(i);
        }

        if (avaliableCardsIndexes.Count == 0) return;

        foreach (int index in avaliableCardsIndexes)
        {
            cardsWindow.GetChild(index).gameObject.SetActive(true);
            int cardIndex = index;
            cardsWindow.GetChild(index).GetComponent<Button>().onClick.AddListener(() => ShowSpecialCard(playerData, cardIndex));
        }

        cardsWindow.parent.position = newPosition;

        cardChoiceWindow.SetActive(true);
        buttonsWindow.SetActive(true);
    }

    private void ShowSpecialCard(PlayerData playerData,int index)
    {
        usedCardWindow.SetSpecilaCard(playerData.SpecialCards[index]);
        usedCardWindow.SetHeader("Использованная карта");
        usedCardWindow.SetAction(() => UseSpecialCard(playerData, index));
        usedCardWindow.StartBlendOut(Times.UsedSpecilaCardTime);


    }

    //private void UseSpecialCard()
    //{
    //    usedPlayer.UseSpecialCard(usedCardIndex, OnCardUsed);
    //}
    
    private void UseSpecialCard(PlayerData usedPlayer, int index)
    {
        usedPlayer.UseSpecialCard(index, OnCardUsed);
    }

    private void OnCardUsed()
    {
        CloseCardChoiceWindow();
        ClosePlayerChoiceWindow();
    }


    public void CloseCardChoiceWindow()
    {
        cardChoiceWindow.SetActive(false);
    }

    public void ClosePauseWindow()
    {
        for (int i = 0; i < playerList.childCount; i++)
        {
            Destroy(playerList.GetChild(i).gameObject);
        }



        gameObject.SetActive(false);
    }
}
