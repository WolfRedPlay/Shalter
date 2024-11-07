using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Fact")]
public class FactCard : CharacteristicsCard
{
    private void OnEnable()
    {
        cardType = CardType.FACT;
    }
}
