using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Special Card", menuName = "Cards/SpecialCards/StealBaggage")]
public class StealBaggageSpecialCard : SpecialCard
{
    [SerializeField] BaggageCard emptyBaggage;
    [SerializeField] GameObject playerButtonPrefab;
    Transform playersToChoseList;
    GameManager gameManager;

    public override void Use()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        FindPlayersToChoseList();

        playersToChoseList.parent.gameObject.SetActive(true);

        foreach (PlayerData player in gameManager.CurrentPlayers)
        {

            GameObject newButton = Instantiate(playerButtonPrefab, playersToChoseList);
            newButton.GetComponent<PlayerButton>().PlayerData = player;
            newButton.GetComponent<Button>().onClick.AddListener(() => ChosePlayer(newButton.GetComponent<PlayerButton>().PlayerData));
            
        }
    }

    public void ChosePlayer(PlayerData choosenPlayer)
    {
        BaggageCard stolenCard = (BaggageCard) choosenPlayer.HandCard[CardType.BAGGAGE];

        owner.ExtraBaggage = stolenCard;

        choosenPlayer.AddCard(emptyBaggage);
        choosenPlayer.RevealCard(CardType.BAGGAGE);
        choosenPlayer.AddCard(gameManager.GetRandomCard(CardType.SPECIAL));

        playersToChoseList.parent.gameObject.SetActive(false);
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
}
