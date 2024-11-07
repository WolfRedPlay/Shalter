using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData
{
    Dictionary<CardType, Card> handCard = new Dictionary<CardType, Card>();
    Dictionary<CardType, bool> openCard = new Dictionary<CardType, bool>();
    List<SpecialCard> specialCards = new List<SpecialCard>();

    BaggageCard extraBaggageCard;
    public BaggageCard ExtraBaggage
    {
        set
        {
            extraBaggageCard = value;
            card.AddExtraBaggage(value);
        }
    }


    public Dictionary<CardType, Card> HandCard 
    {
        get { return handCard; } 
    }
    public Dictionary<CardType, bool> OpenCard
    {
        get { return openCard; }
    }
    public List<SpecialCard> SpecialCards
    {
        get { return specialCards; }
    }


    string playerName = "";
    public string PlayerName
    {
        set
        {
            playerName = value;
            card.Name = value;
        }

        get { return playerName; }

    }


    PlayerCard card;
    public PlayerCard Card { 
        set { card = value; } 
        get { return card; }
    }

    List<PlayerData> playersWhoCanBeVoted = new List<PlayerData>();
    public List<PlayerData> PossibleTargets
    {
        get { return playersWhoCanBeVoted; }
    }

    PlayerData exceptedPlayer;
    public PlayerData ExceptedPlayer
    {
        set { exceptedPlayer = value; }
        get { return exceptedPlayer; }
    }


    int id = 0;
    public int ID
    {
        get { return id; }
    }

    Gender gender;
    int age = 0;


    public Gender PlayerGender
    {
        get
        {
            return gender;
        }
    }


    int yourVoiceMultiplier = 1;

    public int VoiceMultiplier
    {
        set 
        {
            if (choosenPlayer != null) 
                if (value > yourVoiceMultiplier)
                {
                    choosenPlayer.GetComponent<PlayerCard>().Voices++;
                }
                else
                {
                    choosenPlayer.GetComponent<PlayerCard>().Voices--;
                }

            yourVoiceMultiplier = value;
        }

        get
        {
            return yourVoiceMultiplier;
        }
    }


    int multiplierForYourVoices = 1;
    public int SelfVoicesMultiplier
    {
        set
        {
            multiplierForYourVoices = value;
            card.Voices = card.Voices * multiplierForYourVoices;
        }

        get
        {
            return multiplierForYourVoices;
        }
    }


    int voicesAmount = 0;
    public int Voices
    {
        set { 
            voicesAmount = value;
        }

        get { return voicesAmount; }
    }


    GameObject choosenPlayer;
    public GameObject ChoosenPlayer
    {
        set
        {
            if (value == null) choosenPlayer = value;
            else if (choosenPlayer == null)
            {
                value.GetComponent<PlayerCard>().Voices += yourVoiceMultiplier * value.GetComponent<PlayerCard>().Data.SelfVoicesMultiplier;
                choosenPlayer = value;
            }
            else if (value.GetComponent<PlayerCard>().Data.ID == choosenPlayer.GetComponent<PlayerCard>().Data.ID)
            {
                choosenPlayer.GetComponent<PlayerCard>().Voices -= yourVoiceMultiplier * choosenPlayer.GetComponent<PlayerCard>().Data.SelfVoicesMultiplier;
                choosenPlayer = null;
            } else
            {
                choosenPlayer.GetComponent<PlayerCard>().Voices -= yourVoiceMultiplier * choosenPlayer.GetComponent<PlayerCard>().Data.SelfVoicesMultiplier;
                value.GetComponent<PlayerCard>().Voices += yourVoiceMultiplier * value.GetComponent<PlayerCard>().Data.SelfVoicesMultiplier;
                choosenPlayer = value;
            }
        }

        get
        {
            return choosenPlayer;
        }
    }


    bool isAbleToChangeVoice = true;
    public bool ChangeVoice
    {
        set
        {
            isAbleToChangeVoice = value;
        }

        get { return isAbleToChangeVoice;}

    }


    event Action OnKickAction;


    bool isAbleToChangePossibleTargets = true;

    bool fertile = true;
    
    public bool Fertile 
    {
        get {return fertile;}
    }

    public void UseSpecialCard(int index, Action OnUsed)
    {
        specialCards[index].OnUsed += OnUsed;
        specialCards[index].Use();
    }



    public void SetNewPossiblePlayersForVote(List<PlayerData> newPLayersList)
    {
        if (isAbleToChangePossibleTargets)
        {
            playersWhoCanBeVoted.Clear();
            foreach (PlayerData player in newPLayersList)
            {
                if (exceptedPlayer != null)
                    if (player.ID == exceptedPlayer.ID) continue;
                playersWhoCanBeVoted.Add(player);
            }
        }
        else
        {
            isAbleToChangePossibleTargets = true;
        }
        
    }

    public void RemovePlayerFromPossibleTargets(PlayerData playerToRemove)
    {

        int playerIndex = playersWhoCanBeVoted.FindIndex(x => x.ID == playerToRemove.ID);
        playersWhoCanBeVoted.RemoveAt(playerIndex);
    }

    public void SetOnePlayerForPossibleTargets(PlayerData targetPlayer)
    {
        playersWhoCanBeVoted.Clear();
        playersWhoCanBeVoted.Add(targetPlayer);
        isAbleToChangePossibleTargets = false;
    }



    //testing
    public void RevealAllCards()
    {
     foreach (var card in handCard.Values)
        {
            RevealCard(card.CardType);
        }
    }


    public void CleanHand()
    {
        handCard.Clear();
        openCard.Clear();
    }
    
    public void AddCard(Card cardToAdd)
    {
        if (cardToAdd.CardType == CardType.SPECIAL)
        {
            SpecialCard specialCardToAdd = (SpecialCard)cardToAdd;
            specialCardToAdd.Owner = this;
            specialCards.Add(specialCardToAdd);
        }
        else
        {
            if (handCard.ContainsKey(cardToAdd.CardType))
            {
                handCard.Remove(cardToAdd.CardType);
            }

            handCard.Add(cardToAdd.CardType, cardToAdd);

            switch (cardToAdd.CardType)
            {
                case CardType.BIOLOGY:
                    gender = (cardToAdd as BiologyCard).CardGender;
                    age = (cardToAdd as BiologyCard).Age;
                break;

                case CardType.JOB:
                    (cardToAdd as JobCard).SetRandomExperience(age);
                break;

                case CardType.HEALTH:
                    (cardToAdd as HealthCard).SetRandomStage();
                break;

                case CardType.HOBBY:
                    (cardToAdd as HobbyCard).SetRandomExperience(age);
                break;
            }

            if ((cardToAdd.CardType == CardType.HEALTH || cardToAdd.CardType == CardType.BIOLOGY))
            {
                if (!fertile)
                {
                    if ((handCard.ContainsKey(CardType.HEALTH) && handCard.ContainsKey(CardType.BIOLOGY)) &&
                        ((handCard[CardType.HEALTH] as HealthCard).Fertile && (handCard[CardType.BIOLOGY] as BiologyCard).Fertile))
                        fertile = true;
                }
                else
                {
                    if (cardToAdd.CardType == CardType.BIOLOGY && !(cardToAdd as BiologyCard).Fertile)
                    {
                        gender = (cardToAdd as BiologyCard).CardGender;
                        fertile = false;
                    }
                    if (cardToAdd.CardType == CardType.HEALTH && !(cardToAdd as HealthCard).Fertile)
                    {
                        fertile = false;
                    }
                }
            }
            
            
            if (!openCard.ContainsKey(cardToAdd.CardType))
            {
                openCard.Add(cardToAdd.CardType, false);
            }
            else if (openCard[cardToAdd.CardType])
            {
                RevealCard(cardToAdd.CardType);
            }

        }
        
        
    }

    public void RevealCard(CardType cardType)
    {
        openCard[cardType] = true;
        card.RevealCard(cardType);
    }
    
    public void SetID(int newID)
    {
        id = newID;
    }


    public void SwapCard(PlayerData playerToSwap, CardType cardTypeToSwap)
    {
        Card cardToSwap = playerToSwap.HandCard[cardTypeToSwap];

        playerToSwap.AddCard(handCard[cardTypeToSwap]);
        AddCard(cardToSwap);
    }


    public void RefreshTargetForSpecialCards()
    {
        foreach (Card card in specialCards)
        {
            if (card is SelfVottingSpecialCard)
            {
                SelfVottingSpecialCard cardToChangeTarget = (SelfVottingSpecialCard)card;
                if(!cardToChangeTarget.Used) cardToChangeTarget.CalculateTargetPlayer();
            }
        }
    }


    public void AddOnKickAction(Action NewAction)
    {
        OnKickAction += NewAction;
    }

    public void CleanOnKickAction()
    {
        OnKickAction = null;
    }

    public void KickPlayerAction()
    {
        if (OnKickAction != null)
        {
            OnKickAction.Invoke();
            OnKickAction = null;
        }
    }

    public void RefrshMultipriers()
    {
        multiplierForYourVoices = 1;
        yourVoiceMultiplier = 1;
    }

}
