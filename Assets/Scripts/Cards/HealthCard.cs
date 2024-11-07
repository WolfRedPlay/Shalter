using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Health")]
public class HealthCard : CharacteristicsCard
{

    [SerializeField] bool fertile;
    [SerializeField] bool isStageAppliable;

    public bool HasStage => isStageAppliable;
    
    
    int stage = -1;
    public int Stage => stage;

    public bool Fertile { get { return fertile; } }
    private void OnEnable()
    {
        cardType = CardType.HEALTH;
    }


    public void SetRandomStage()
    {
        if (isStageAppliable)
        {
            int dice1 = Random.Range(1, 7);
            int dice2 = Random.Range(1, 7);
            int dice3 = Random.Range(1, 7);

            int sum = dice1 + dice2 + dice3;

            if (sum <= 7)
            {
                stage = 3;
            }
            else if (sum <= 13)
            {
                stage = 2;
            }
            else
            {
                stage = 1;
            }
        }
        
    }

}
