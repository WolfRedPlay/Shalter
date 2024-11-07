using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Job")]
public class JobCard : CharacteristicsCard
{
    int experience = 0;
    public int Exp => experience;


    private void OnEnable()
    {
        cardType = CardType.JOB;
    }


    public void SetRandomExperience(int age)
    {
        int maxExperience = age - 18;

        if (maxExperience <= 0)
        {
            experience = 0;
        }
        else
        {
            experience = Random.Range(0, maxExperience + 1);
        }

    }
}
