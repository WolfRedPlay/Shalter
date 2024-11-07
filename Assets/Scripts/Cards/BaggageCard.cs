using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Baggage")]
public class BaggageCard : CharacteristicsCard
{
    private void OnEnable()
    {
        cardType = CardType.BAGGAGE;
    }
}
