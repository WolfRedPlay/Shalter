using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Special Card", menuName = "Cards/SpecialCards/CollectAll")]
public class CollectAllSpecialCard : SpecialCard
{
    protected override void OnEnable()
    {
        base.OnEnable();
        cardName = "Давай начистоту";
        isUsed = false;
    }

    [SerializeField] CardType cardToCollect;

    public override void Use()
    {
        
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        List<Card> collectedCards = new List<Card>();
        foreach (PlayerData player in gameManager.CurrentPlayers)
        {
            collectedCards.Add(player.HandCard[cardToCollect]);
        }

        collectedCards.Shuffle();

        for (int i = 0; i < collectedCards.Count; i++)
        {
            gameManager.CurrentPlayers[i].AddCard(collectedCards[i]);
        }

        base.Use();
    }

}
