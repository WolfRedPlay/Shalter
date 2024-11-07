using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Biology")]
public class BiologyCard : CharacteristicsCard
{
    [SerializeField] private Sprite genderIcon;
    public Sprite GenderIcon { get { return genderIcon; } }


    [SerializeField] Gender gender;
    public Gender CardGender { get { return gender; } }


    [SerializeField] private int age;
    public int Age { get { return age; } }


    [SerializeField] bool fertile;
    public bool Fertile { get { return fertile; } }


    private void OnEnable()
    {
        cardType = CardType.BIOLOGY;
    }
}
