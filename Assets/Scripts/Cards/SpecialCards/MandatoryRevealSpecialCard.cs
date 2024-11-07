using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Special Card", menuName = "Cards/SpecialCards/MandatoryReveal")]
public class MandatoryRevealSpecialCard : SpecialCard
{
    protected override void OnEnable()
    {
        base.OnEnable();
        cardName = "Прямой вопрос";
    }


    Transform cardsToChoseList;
    GameManager gameManager;



    public override void Use()
    {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        FindPlayersToChoseList();


        for (int i = 0; i < cardsToChoseList.childCount; i++)
        {
            bool checker = false;
            foreach(PlayerData player in gameManager.CurrentPlayers)
            {
                if (!player.OpenCard[(CardType)i])
                {
                    checker = true;
                    break;
                }
            }
            if (checker) cardsToChoseList.GetChild(i).gameObject.SetActive(true);
            else cardsToChoseList.GetChild(i).gameObject.SetActive(false);
            Button curButton = cardsToChoseList.GetChild(i).GetComponent<Button>();
            curButton.onClick.RemoveAllListeners();
            int cardTypeIndex = i;
            curButton.onClick.AddListener(() => ChoseCardType((CardType) cardTypeIndex));
        }


        cardsToChoseList.parent.gameObject.SetActive(true);
    }


    public void ChoseCardType(CardType mandatoryCardType)
    {
        cardsToChoseList.parent.gameObject.SetActive(false);

        gameManager.MandatoryCard = mandatoryCardType;
        base.Use();
    }


    private void FindPlayersToChoseList()
    {
        cardsToChoseList = GameObject.Find("Canvas").transform;
        cardsToChoseList = cardsToChoseList.Find("PauseWindow");
        cardsToChoseList = cardsToChoseList.Find("Window");
        cardsToChoseList = cardsToChoseList.Find("CardChoice");
        cardsToChoseList = cardsToChoseList.Find("CardTypeChoice");
        cardsToChoseList = cardsToChoseList.Find("CardToChose");
    }

    public override bool CheckPossibilityToUse()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().CurrentRound == 1)
        {
            return false;
        }
        else
            return base.CheckPossibilityToUse();
    }
}
