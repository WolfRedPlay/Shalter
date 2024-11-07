using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Special Card", menuName = "Cards/SpecialCards/SwapLeftRight")]
public class SwapLeftRightSpecialCard : SpecialCard
{
    protected override void OnEnable()
    {
        base.OnEnable();
        cardName = "Обмен карт";

    }

    [SerializeField] CardType cardToSwap;
    Transform playersToChoseList;
    
    PlayerData leftPlayer;
    PlayerData rightPlayer;
    GameManager gameManager;
    public override void Use()
    {
        FindPlayersToChoseList();

        playersToChoseList.parent.gameObject.SetActive(true);

        GameObject newPlayerCard;
        Button newPlayerCardButton;

        if (leftPlayer != null)
        {
            newPlayerCard = Instantiate(leftPlayer.Card.gameObject, playersToChoseList);
            newPlayerCard.GetComponent<RectTransform>().sizeDelta = new Vector2(256, 0);
            newPlayerCardButton = newPlayerCard.GetComponent<Button>();
            newPlayerCardButton.enabled = true;
            newPlayerCardButton.onClick.RemoveAllListeners();
            newPlayerCardButton.onClick.AddListener(() => ChosePlayerToSwap(leftPlayer));
        }
            
        if (rightPlayer != null)
        {
            newPlayerCard = Instantiate(rightPlayer.Card.gameObject, playersToChoseList);
            newPlayerCard.GetComponent<RectTransform>().sizeDelta = new Vector2(256, 0);
            newPlayerCardButton = newPlayerCard.GetComponent<Button>();
            newPlayerCardButton.enabled = true;
            newPlayerCardButton.onClick.RemoveAllListeners();
            newPlayerCardButton.onClick.AddListener(() => ChosePlayerToSwap(rightPlayer));
        }


    }

    public void ChosePlayerToSwap(PlayerData playerToSwap)
    {
        playersToChoseList.parent.gameObject.SetActive(false);
        owner.SwapCard(playerToSwap, cardToSwap);
        foreach(Transform player in playersToChoseList)
        {
            Destroy(player.gameObject);
        }

        gameManager.RewritePlayersCards();
        base.Use();
    }
    

    private void FindPlayersToChoseList()
    {
        playersToChoseList = GameObject.Find("Canvas").transform;
        playersToChoseList = playersToChoseList.Find("PauseWindow");
        playersToChoseList = playersToChoseList.Find("Window");
        playersToChoseList = playersToChoseList.Find("CardChoice");
        playersToChoseList = playersToChoseList.Find("LeftRightPlayerChoice");
        playersToChoseList = playersToChoseList.Find("PlayerToChose");

        foreach (Transform playerButton in playersToChoseList)
        {
            Destroy(playerButton.gameObject);
        }
    }


    public override bool CheckPossibilityToUse()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        int cardOwnerIndex = gameManager.CurrentPlayers.FindIndex(x => x.ID == owner.ID);

        if (!gameManager.CurrentPlayers[cardOwnerIndex].OpenCard[cardToSwap]) return false;

        int indexTocheck = cardOwnerIndex - 1;
        if (indexTocheck < 0) indexTocheck = gameManager.CurrentPlayers.Count - 1;
        PlayerData playerToCheck = gameManager.CurrentPlayers[indexTocheck];
        if (playerToCheck.OpenCard[cardToSwap])
        {
            leftPlayer = playerToCheck;
        }

        indexTocheck = (cardOwnerIndex + 1) % gameManager.CurrentPlayers.Count;
        playerToCheck = gameManager.CurrentPlayers[indexTocheck];
        if (playerToCheck.OpenCard[cardToSwap])
        {
            rightPlayer = playerToCheck;
        }


        if (leftPlayer == null && rightPlayer == null)       
            return false;       
        else
            return base.CheckPossibilityToUse();
    }
}
