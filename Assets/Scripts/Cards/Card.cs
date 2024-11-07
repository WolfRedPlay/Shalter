using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card: ScriptableObject
{
    [SerializeField] protected string cardName;
    public string Name
    {
        get { return cardName; }
    }


    [SerializeField] protected CardType cardType;
    public CardType CardType {  get { return cardType; } }
}
