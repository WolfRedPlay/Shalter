using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Special Card", menuName = "Cards/SpecialCards/ChangeOnRandom")]
public class ChangeOnRandomSpecialCard : SpecialCard
{
    [SerializeField] CardType cardToCollect;
    [SerializeField] GameObject playerButtonPrefab;
    Transform playersToChoseList;
    GameManager gameManager;


    public override void Use()
    {

        
        FindPlayersToChoseList();


        foreach (PlayerData player in gameManager.CurrentPlayers)
        {
            if (player.OpenCard[cardToCollect])
            {
                GameObject newButton = Instantiate(playerButtonPrefab, playersToChoseList);
                newButton.GetComponent<PlayerButton>().PlayerData = player;
                newButton.GetComponent<Button>().onClick.AddListener(() => ChosePlayer(newButton.GetComponent<PlayerButton>().PlayerData));
            }
        }


        playersToChoseList.parent.gameObject.SetActive(true);
    }

    public void ChosePlayer(PlayerData choosenPlayer)
    {
        playersToChoseList.parent.gameObject.SetActive(false);
        Card takenCard = choosenPlayer.HandCard[cardToCollect];
        choosenPlayer.AddCard(gameManager.GetRandomCard(cardToCollect));
        gameManager.AddCardToDeck(takenCard);

        foreach (Transform player in playersToChoseList)
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
        playersToChoseList = playersToChoseList.Find("PlayerChoice");
        playersToChoseList = playersToChoseList.Find("PlayerToChose");

        foreach (Transform playerButton in playersToChoseList)
        {
            Destroy(playerButton.gameObject);
        }
    }

    public override bool CheckPossibilityToUse()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        bool checker = false;
        foreach (PlayerData player in gameManager.CurrentPlayers)
        {
            if (player.OpenCard[cardToCollect])
            {
                checker = true;
                break;
            }
        }
        if (!checker) return false;
        else
            return base.CheckPossibilityToUse();
    }
}
