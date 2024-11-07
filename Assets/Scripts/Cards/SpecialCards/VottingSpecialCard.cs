using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Special Card", menuName = "Cards/SpecialCards/Votting")]
public class VottingSpecialCard : SpecialCard
{
    enum Type
    {
        VOICES_AGAINST,
        DOUBLE_VOICE,
        ZERO_VOICE,
        NEW_VOTTING,
        FRIEND
    }


    [SerializeField] Type type;
    [SerializeField] GameObject playerButtonPrefab;
    Transform playersToChoseList;
    GameManager gameManager;
    


    public override void Use()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        switch (type)
        {
            case Type.DOUBLE_VOICE:
                owner.VoiceMultiplier = 2;
                base.Use();
                break;

            case Type.NEW_VOTTING:
                foreach (PlayerData player in gameManager.CurrentPlayers)
                {
                    if (player.ChoosenPlayer != null)
                    {
                        player.RemovePlayerFromPossibleTargets(player.ChoosenPlayer.GetComponent<PlayerCard>().Data);
                    }

                    player.Card.Voices = 0;
                }
                gameManager.StartVoting();
                base.Use();
                break;

            default:
                OpenPlayerChoseWindow();
                break;
        }
    }


    private void OpenPlayerChoseWindow()
    {
        FindPlayersToChoseList();
        playersToChoseList.parent.gameObject.SetActive(true);

        foreach (PlayerData player in gameManager.CurrentPlayers)
        {
            GameObject newButton = Instantiate(playerButtonPrefab, playersToChoseList);
            newButton.GetComponent<PlayerButton>().PlayerData = player;
            newButton.GetComponent<Button>().onClick.AddListener(() => ChosePlayer(newButton.GetComponent<PlayerButton>().PlayerData));
        }
    }

    public void ChosePlayer(PlayerData choosenPlayer)
    {
        switch (type)
        {
            case Type.VOICES_AGAINST:
                owner.VoiceMultiplier = 0;
                choosenPlayer.SelfVoicesMultiplier = 2;
                break;


            case Type.ZERO_VOICE:
                choosenPlayer.VoiceMultiplier = 0;
                break;


            case Type.FRIEND:
                choosenPlayer.ExceptedPlayer = owner;
                break;
        }

        playersToChoseList.parent.gameObject.SetActive(false);
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
}
