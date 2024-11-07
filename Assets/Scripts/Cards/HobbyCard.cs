using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Hobby")]
public class HobbyCard : CharacteristicsCard
{
    int experience = 0;
    public int Exp => experience;

    private void OnEnable()
    {
        cardType = CardType.HOBBY;
    }

    public void SetRandomExperience(int age)
    {
        int maxExperience = age;

        if (maxExperience <= 0)
        {
            experience = 0;
        }
        else
        {
            experience = Random.Range(0, maxExperience);
        }

    }
}
