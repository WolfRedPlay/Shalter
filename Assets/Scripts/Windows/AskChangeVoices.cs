using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AskChangeVoices : MonoBehaviour
{
    [SerializeField] PlayerList playerList;

    [SerializeField] GameManager gameManager;

    [SerializeField] GameObject askWindow;
    [SerializeField] GameObject whoWindow;
    [SerializeField] GameObject listPlayersButtons;

    [SerializeField] GameObject playerButtonPrefab;

    List<PlayerData> changingPlayers = new List<PlayerData>();


    private void Start()
    {
        whoWindow.SetActive(false);
    }


    public void OpenAskWindow()
    {
        whoWindow.SetActive(false);
        askWindow.SetActive(true);
    }

    public void OpenWhoWindow()
    {
        askWindow.SetActive(false);
        whoWindow.SetActive(true);
        CreateButtons(gameManager.CurrentPlayers);
    }

    public void FinishAsking()
    {
        gameObject.SetActive(false);
        gameManager.KickPlayers();
    }

    public void CreateButtons(List<PlayerData> currentPlayers)
    {
        changingPlayers.Clear();
        foreach(Transform child in listPlayersButtons.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (PlayerData player in currentPlayers)
        {
            if (player.ChangeVoice)
            {
                GameObject newButton = Instantiate(playerButtonPrefab, listPlayersButtons.transform);
                newButton.GetComponent<PlayerButton>().PlayerData = player;
                newButton.GetComponent<PlayerButton>().DeselectPlayer();
                newButton.GetComponent<Button>().onClick.AddListener(() => PlayerToChange(newButton.GetComponent<PlayerButton>()));
            }

        }
    }


    public void PlayerToChange(PlayerButton playerButton)
    {
        if (changingPlayers.Find(x => x.ID == playerButton.PlayerData.ID) != null){
            changingPlayers.Remove(playerButton.PlayerData);
            playerButton.DeselectPlayer();
        }
        else
        {
            changingPlayers.Add(playerButton.PlayerData);
            playerButton.SelectPlayer();
        }
    }

    public void FinishChoice()
    {
        foreach (PlayerData player in changingPlayers)
        {
            player.ChangeVoice = false;
        }
        playerList.StartVoting(changingPlayers);
        playerList.StartVote(0);
        gameObject.SetActive(false);

    }


}
