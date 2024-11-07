using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Special Card", menuName = "Cards/SpecialCards/Treatment")]
public class TreatmentSpecialCard : SpecialCard
{
    protected override void OnEnable()
    {
        base.OnEnable();
        cardName = "Отличные таблетки";
    }

    [SerializeField] HealthCard idealHealth;
    [SerializeField] GameObject playerButtonPrefab;
    Transform playersToChoseList;
    GameManager gameManager;


    public override void Use()
    {


        FindPlayersToChoseList();


        foreach (PlayerData player in gameManager.CurrentPlayers)
        {
            if (player.OpenCard[CardType.HEALTH])
            {
                GameObject newButton = Instantiate(playerButtonPrefab, playersToChoseList);
                newButton.GetComponent<PlayerButton>().PlayerData = player;
                newButton.GetComponent<Button>().onClick.AddListener(() => ChosePlayer(newButton.GetComponent<PlayerButton>().PlayerData));
            }
        }


        playersToChoseList.parent.gameObject.SetActive(true);
    }

    public void ChosePlayer(PlayerData choosenPlayer)
    {
        choosenPlayer.AddCard(idealHealth);
        gameManager.RewritePlayersCards();
        base.Use();
    }


    private void FindPlayersToChoseList()
    {
        playersToChoseList = GameObject.Find("Canvas").transform;
        playersToChoseList = playersToChoseList.Find("PauseWindow");
        playersToChoseList = playersToChoseList.Find("Window");
        playersToChoseList = playersToChoseList.Find("CardChoice");
        playersToChoseList = playersToChoseList.Find("PlayerChoice");
        playersToChoseList = playersToChoseList.Find("PlayerToChose");

        foreach (Transform playerButton in playersToChoseList)
        {
            Destroy(playerButton.gameObject);
        }
    }

    public override bool CheckPossibilityToUse()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        bool checker = false;
        foreach(PlayerData player in gameManager.CurrentPlayers)
        {
            if (player.OpenCard[CardType.HEALTH])
            {
                checker = true;
                break;
            }
        }
        if (!checker) return false;
        else
            return base.CheckPossibilityToUse();
    }
}
