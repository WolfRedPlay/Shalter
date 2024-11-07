using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Bunker")]
public class BunkerCard : Card
{
    private void OnEnable()
    {
        cardType = CardType.BUNKER;
    }

    [SerializeField] string description;
    public string Description
    {
        get { return description; }
    }
}
