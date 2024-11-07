using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Special Card", menuName = "Cards/SpecialCards/CahngeBunkerCard")]
public class ChangeBunkerCardSpecialCard : SpecialCard
{
    protected override void OnEnable()
    {
        base.OnEnable();
        cardName = "Включил свет";
    }

    GameManager gameManager;
    Transform bunkerCardsList;




    public override void Use()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        FindPlayersToChoseList();
        bunkerCardsList.parent.gameObject.SetActive(true);
        foreach (Transform bunkerCard in bunkerCardsList)
        {
            Button cardButton = bunkerCard.GetOrAddComponent<Button>();
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(() => ChangeBunkerCardOnRandom(bunkerCard.GetSiblingIndex()));

        }


    }


    public void ChangeBunkerCardOnRandom(int bunkerCardIndex)
    {
        Menu menu = GameObject.Find("Canvas").GetComponent<Menu>();

        BunkerCard newBunkerCard = (BunkerCard) gameManager.GetRandomCard(CardType.BUNKER);
        menu.ChangeBunkerCard(newBunkerCard, bunkerCardIndex);
        bunkerCardsList.parent.gameObject.SetActive(false);
        base.Use();
    }


    private void FindPlayersToChoseList()
    {
        bunkerCardsList = GameObject.Find("Canvas").transform;
        bunkerCardsList = bunkerCardsList.Find("PauseWindow");
        bunkerCardsList = bunkerCardsList.Find("Window");
        bunkerCardsList = bunkerCardsList.Find("CardChoice");
        bunkerCardsList = bunkerCardsList.Find("BunkerCards");
        bunkerCardsList = bunkerCardsList.Find("CardsList");
    }

}
