using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Cards/Catastrophe")]
public class Catastrophe : Card
{
    private void OnEnable()
    {
        cardType = CardType.CATASTROPHE;
    }

    [SerializeField][TextArea] string description;
    public string Description
    {
        get { return description; }
    }
}
