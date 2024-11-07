using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu]
public class Deck : ScriptableObject
{
    [SerializeField] List<Card> AllCards;
    List<Card> cards = new List<Card>();

    public Card GetRandomCard()
    {
        int cardIndex = Random.Range(0, cards.Count);

        Card cardToReturn = cards[cardIndex];
        cards.RemoveAt(cardIndex);
        return cardToReturn;
    }

    public void AddCArd(Card cardToAdd)
    {
        cards.Add(cardToAdd);
    }

    public void RefreshCards()
    {
        cards.Clear();
        cards.AddRange(AllCards);
    }
}
