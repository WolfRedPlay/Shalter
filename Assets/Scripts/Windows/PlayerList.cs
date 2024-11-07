using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour
{


    [SerializeField] Timer timer;

    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject playerPrefab;

    [SerializeField] GameObject askWindow;
    [SerializeField] CurrentPlayerWindow currentPlayerWindow;
    [SerializeField] VottingResultWindow vottingResultWindow;

    [SerializeField] GameObject playerList;
    [SerializeField] GameObject currntExcuserZone;

    [SerializeField] TMP_Text currentPlayerNickname;
    [SerializeField] TMP_Text playersAmountToKickText;
    PlayerData currentVoter;

    List<PlayerData> voters = new List<PlayerData>();
    List<GameObject> playerCards = new List<GameObject>();
    List<GameObject> playersWithVoices = new List<GameObject>();
    List<GameObject> excusers = new List<GameObject>();


    GameObject excuserCard;

    string vottingResultText;
    [SerializeField] ScreensTransition transition;

    public void SetPlayersAmountToKickText(int amount)
    {
        string playersToKick = amount.ToString();

        playersAmountToKickText.text = playersToKick;

    }

    public int VotersAmount()
    {
        return voters.Count;
    }

    public int ExcusersAmount()
    {
        return excusers.Count;
    }

    private void Start()
    {
        //playerCards.Clear();
        currentPlayerNickname.gameObject.SetActive(false);
        askWindow.SetActive(false);
    }


    public void StartConversation()
    {
        foreach (GameObject card in playerCards)
        {
            card.GetComponent<Button>().enabled = false;
        }
        playersAmountToKickText.gameObject.SetActive(true);
        currentPlayerNickname.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        timer.StartTimer(Times.ConversationTime, gameManager.FinishConversation);
    }




    public void StartVoting(List<PlayerData> playersToVote, int round)
    {
        foreach (GameObject card in playerCards)
        {
            card.GetComponent<PlayerCard>().Data.ChangeVoice = true;
            card.GetComponent<PlayerCard>().Data.ChoosenPlayer = null;
            card.GetComponent<PlayerCard>().Voices = 0;


        }
        playersWithVoices.Clear();
        currentPlayerNickname.gameObject.SetActive(true);
        voters.Clear();
        foreach (PlayerData player in playersToVote)
        {
            voters.Add(player);
        }
        gameManager.SetUpTurns(round - 1, voters.Count);
        playersAmountToKickText.gameObject.SetActive(true);
        currentPlayerNickname.gameObject.SetActive(true);
    }
    
    public void StartVoting(List<PlayerData> playersToVote)
    {

        currentPlayerNickname.gameObject.SetActive(true);
        voters.Clear();
        foreach (PlayerData player in playersToVote)
        {
            voters.Add(player);
        }
        gameManager.SetUpTurns(0, voters.Count);
        playersAmountToKickText.gameObject.SetActive(true);
        currentPlayerNickname.gameObject.SetActive(true);
    }


    public void StartVote(int indexVoter)
    {
        foreach (GameObject card in playerCards)
        {
            card.GetComponent<Button>().enabled = false;
        }
        currentVoter = voters[indexVoter];
        currentPlayerNickname.text = currentVoter.PlayerName;

        foreach (PlayerData target in currentVoter.PossibleTargets)
        {
            GameObject targetCard = playerCards.Find(x => x.GetComponent<PlayerCard>().Data.ID == target.ID);
            if (targetCard != null)
            {
                targetCard.GetComponent<Button>().enabled = true;
            }
        }

        currentPlayerWindow.SetAction(StartVoteTimer);
        currentPlayerWindow.SetPlayerName(currentVoter.PlayerName);
        currentPlayerWindow.StartBlendOut(Times.CurrentPlayerTime);
        
    }

    public void StartVoteTimer()
    {
        timer.StartTimer(Times.VoteTime, gameManager.FinishVote);
    }


    public void VoteForPlayer()
    {
        if (currentVoter.ChoosenPlayer != null)
            if (playersWithVoices.Find(x => x.GetComponent<PlayerCard>().Data.ID == currentVoter.ChoosenPlayer.GetComponent<PlayerCard>().Data.ID) == null)
                playersWithVoices.Add(currentVoter.ChoosenPlayer);
    }

    public void ChosePlayer(GameObject playerCard)
    {
        currentVoter.ChoosenPlayer = playerCard;
    }


    public int AnayzeVoting(int playersNeedToKick, out int amountOfExcusers)
    {
        foreach (GameObject card in playerCards)
        {
            card.GetComponent<Button>().enabled = false;
        }
        if (playersWithVoices.Count == 0)
        {
            amountOfExcusers = 0;
            return 0;
        }
        else if (playersWithVoices.Count < playersNeedToKick)
        {
            amountOfExcusers = 0;
            return -1;
        }
        else
        {
            int maxAmount = 1;
            playersWithVoices = playersWithVoices.OrderBy(x => x.GetComponent<PlayerCard>().Voices).ToList();
            int maxVoices = playersWithVoices[playersWithVoices.Count - 1].GetComponent<PlayerCard>().Voices;
            bool excusesAvaliable = gameManager.IsChangingVoiceAvaliable();
            for (int i = playersWithVoices.Count - 2; i >= 0; i--)
            {
                if (playersWithVoices[i].GetComponent<PlayerCard>().Voices == maxVoices) maxAmount++;
                else break;
            }

            if (maxAmount == playersNeedToKick)
            {
                amountOfExcusers = 0;
                return 1;
            }
            else if (maxAmount < playersNeedToKick)
            {
                int beforeMax = 0;
                int amountBeforeMax = 0;
                for (int i = playersWithVoices.Count - 2; i >= 0; i--)
                {
                    if (playersWithVoices[i].GetComponent<PlayerCard>().Voices < maxVoices)
                    {
                        beforeMax = playersWithVoices[i].GetComponent<PlayerCard>().Voices;
                        break;
                    }
                }
                for (int i = playersWithVoices.Count - 2; i >= 0; i--)
                {
                    if (playersWithVoices[i].GetComponent<PlayerCard>().Voices == beforeMax)
                    {
                        amountBeforeMax++;
                    }
                }

                if (amountBeforeMax + maxAmount > playersNeedToKick)
                {
                    if (excusesAvaliable)
                    {
                        amountOfExcusers = amountBeforeMax + maxAmount;
                        return 2;
                    }
                    else
                    {
                        amountOfExcusers = amountBeforeMax + maxAmount;
                        return 1;
                    }
                }
                else
                {
                    amountOfExcusers = 0;
                    return 1;
                }

            }
            else
            {
                if (excusesAvaliable)
                {
                    amountOfExcusers = maxAmount;
                    return 2;
                }
                else
                {
                    amountOfExcusers = 0;
                    return 1;
                }
            }
        }
        
    }

    public void ShowVottingResult(string text, Action newAction)
    {
        vottingResultWindow.ClearPlayerList();
        vottingResultWindow.SetText(text);
        vottingResultWindow.SetAction(newAction);
        vottingResultWindow.StartBlendOut(Times.ResultWindowTime);
    }
    
    public void ShowVottingResult(string text, Action newAction, List<GameObject> newPlayersCards)
    {
        vottingResultWindow.ClearPlayerList();
        vottingResultWindow.SetText(text);
        vottingResultWindow.SetAction(newAction);
        vottingResultWindow.AddPlayersCards(newPlayersCards);
        vottingResultWindow.StartBlendOut(Times.ResultWindowTime);
    }




    public void StartExuses(int amountOfExcusers)
    {
        excusers.Clear();
        for (int i = 1; i <= amountOfExcusers; i++)
        {
            excusers.Add(playersWithVoices[playersWithVoices.Count - i]);
        }
        gameManager.SetUpTurns(0, excusers.Count);
        vottingResultText = "По итогу голосования " + excusers.Count.ToString() + " набрали одинаковое количество голосов. У них будет время чтобы оправдаться, после этого будет проведенно повтороне голосование.";
        transition.SetTransitionFinalAction(ShowVottingResultWindowForExcuses);
        transition.StartTransition();
    }

    public void ShowVottingResultWindowForExcuses()
    {
        ShowVottingResult(vottingResultText, gameManager.StartExcuse, excusers);
    }

    public void StartExcuse(int excuserIndex)
    {
        if (excuserCard != null) Destroy(excuserCard);
        excuserCard = Instantiate(excusers[excuserIndex], currntExcuserZone.transform);
        excuserCard.GetComponent<Button>().enabled = true;
        excuserCard.GetComponent<Button>().interactable = false;
        currentPlayerNickname.text = excusers[excuserIndex].GetComponent<PlayerCard>().Name;
        currentPlayerWindow.SetAction(StartExcuseTimer);
        currentPlayerWindow.SetPlayerName(excusers[excuserIndex].GetComponent<PlayerCard>().Name);
        currentPlayerWindow.StartBlendOut(Times.CurrentPlayerTime);
  
    }

    public void StartExcuseTimer()
    {
        timer.StartTimer(Times.ExcuseTime, gameManager.FinishExcuse);
    }


    public bool CheckForChangingVoices()
    {
        bool check = false;
        foreach (GameObject player in playerCards)
        {
            if (player.GetComponent<PlayerCard>().Data.ChangeVoice)
            {
                check = true;
                break;
            }
        }
        return check;
    }

    public void AskForChangeVoices()
    {
        Destroy(excuserCard);
        askWindow.SetActive(true);
        askWindow.GetComponent<AskChangeVoices>().OpenAskWindow();
    }




    public List<GameObject> FindPlayersToKick(int playersToKickAmount)
    {
        List<GameObject> playerstoKick = new List<GameObject>();
        for (int i = 1; i <= playersToKickAmount; i++)
        {
            playerstoKick.Add(playersWithVoices[playersWithVoices.Count - i]);
        }

        return playerstoKick;
    }

    public PlayerData AddPlayerCard()
    {
        GameObject newPlayerCard = Instantiate(playerPrefab, playerList.transform);
        newPlayerCard.GetComponent<Button>().onClick.AddListener(() => ChosePlayer(newPlayerCard.gameObject));
        //if (playerCards.Count == 0)
        //    newPlayerCard.transform.SetSiblingIndex(0);
        //else 
        //    newPlayerCard.transform.SetSiblingIndex(playerCards[playerCards.Count - 1].transform.GetSiblingIndex() + 1);
        playerCards.Add(newPlayerCard);
        newPlayerCard.GetComponent<PlayerCard>().CleanPlayerText();
        return newPlayerCard.GetComponent<PlayerCard>().Data;
    }

    public void RemovePlayerCard(PlayerData playerDataToRemove)
    {
        GameObject playerToRemove = playerCards.Find(x => x.GetComponent<PlayerCard>().Data.ID == playerDataToRemove.ID);
        if (playerToRemove != null)
        {
            playerCards.Remove(playerToRemove);
            Destroy(playerToRemove);
        }
    }



    public void SyncPlayersCardsPositions(List<PlayerData> orderedPlayers)
    {
        int index = 0;
        foreach (PlayerData player in orderedPlayers)
        {

            int cardIndex = playerCards.FindIndex(x => x.GetComponent<PlayerCard>().Data.ID == player.ID);

            playerCards[cardIndex].transform.SetSiblingIndex(index);

            index++;

        }
    }
}
