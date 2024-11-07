using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
    #region DECKS
    [SerializeField] Deck catastropheDeck;
    [SerializeField] Deck bunkerDeck;
    [SerializeField] Deck bioDeck;
    [SerializeField] Deck healthDeck;
    [SerializeField] Deck jobDeck;
    [SerializeField] Deck hobbyDeck;
    [SerializeField] Deck factDeck;
    [SerializeField] Deck baggageDeck;
    [SerializeField] Deck specialDeck;
    #endregion DECKS

    [SerializeField] GameObject playerPrefab;

    [SerializeField] TextHandler textHandler;

    Catastrophe choosenCatastrophe;


    [SerializeField] ScreensTransition transition;

    #region MENUES
    [SerializeField] PlayerList playerList;
    [SerializeField] RevealCardMenu revealCardMenu;
    [SerializeField] IntroWindow introWindow;
    [SerializeField] ResultWindow resultWindow;
    #endregion MENUES


    BunkerCard []bunkerCards = new BunkerCard[5];

    int playersAmount = 0;


    List<PlayerData> allPlayers = new List<PlayerData>();
    [SerializeField] List<PlayerData> currentPlayers = new List<PlayerData>();
    [SerializeField] List<PlayerData> kickedPlayers = new List<PlayerData>();

    List<GameObject> newPlayersToKick;

    public List<PlayerData> CurrentPlayers
    {
        get { return currentPlayers; }
    }

    int playersToBunker = 0;
    int playersToKick = 0;
    int hiddenCards = 0;
    int roundCardsToReavel = 0;
    int roundPlayersToKick = 0;
    int round = 0;
    public int CurrentRound
    {
        get { return round; }
    }
    
    int roundsLeft = 0;

    int currentPlayerIndex = 0;
    int playersLeftToPlay = 0;

    bool votingSkipped = false;
    int amountOfExcusers = 0;

    bool familyMode = false;

    public bool FamilyMode
    {
        set
        {
            familyMode = value;
        }
    }

    CardType mandatoryCardToReveal = CardType.NONE;

    public CardType MandatoryCard
    {
        set
        {
            mandatoryCardToReveal = value;
        }
    }

    string filePath;


    [SerializeField] DiscordBotHandler discord;

    string textForVottingResult = "";
    private void Start()
    {
        filePath = Path.Combine(Application.dataPath, "Players");
        //filePath = "Players";

        //discord.StartDiscrodBot();
    }



    public void SetUpTurns(int currentPlayer, int playersLeft)
    {
        currentPlayerIndex = currentPlayer;
        playersLeftToPlay = playersLeft;
    }


    #region REVEALING_METHODS
    private void CalculateCardsToReveal()
    {
        if (playersToKick >= 5)
        {
            roundCardsToReavel = 1;
            hiddenCards--;
        }
        else
        {
            if (hiddenCards - roundsLeft > 2)
            {
                roundCardsToReavel = 2;
                hiddenCards -= 2;
            }
            else
            {
                roundCardsToReavel = 1;
                hiddenCards--;
            }
        }
    }

    public void StartRevealing()
    {
        currentPlayerIndex = round - 1;
        playersLeftToPlay = currentPlayers.Count;
        CalculateCardsToReveal();
        OpenRevealCardWindow();
       // discord.GetMessageFromDiscord();
    }

    public void OpenRevealCardWindow()
    {
        transition.SetTransitionFinalAction(revealCardMenu.ShowCurrentPlayerWindow);
        if (round == 1) revealCardMenu.StartTurn(currentPlayers[currentPlayerIndex], roundCardsToReavel, CardType.JOB);
        else revealCardMenu.StartTurn(currentPlayers[currentPlayerIndex], roundCardsToReavel, mandatoryCardToReveal);

    }

    public void FinishSpeech()
    {
        playersLeftToPlay--;
        currentPlayerIndex = (currentPlayerIndex + 1) % currentPlayers.Count;

        if (playersLeftToPlay == 0)
        {
            //Procceed to open conversation
            if (playersAmount < 11) roundPlayersToKick = 1;
            else if (playersAmount > 10 && playersAmount < 13)
            {
                if (round == 5) roundPlayersToKick = 2;
                else roundPlayersToKick = 1;
            }
            else if (playersAmount > 12 && playersAmount < 15)
            {
                if (round >= 4) roundPlayersToKick = 2;
                else roundPlayersToKick = 1;
            }
            else if (playersAmount >= 15)
            {
                if (round >= 3) roundPlayersToKick = 2;
                else roundPlayersToKick = 1;
            }
            if (votingSkipped)
            {
                roundPlayersToKick++;
            }
            transition.SetTransitionMiddleAction(ChangeToPlayerListWindow);
            transition.StartTransition();

        }
        else
        {
            transition.SetTransitionMiddleAction(OpenRevealCardWindow);
            transition.StartTransition();
        }
    }
    
    public void ChangeToPlayerListWindow()
    {
        revealCardMenu.gameObject.SetActive(false);
        playerList.SetPlayersAmountToKickText(roundPlayersToKick);
        playerList.StartConversation();
    }

    #endregion REVEALING_METHODS

    public void StartVoting()
    {
        playerList.StartVoting(currentPlayers, round);
        transition.SetTransitionFinalAction(StartVote);
        transition.StartTransition();
    }

    public void StartVote()
    {
        playerList.StartVote(currentPlayerIndex);
    }

    public void TransitionToExcuse()
    {
        playerList.StartExcuse(currentPlayerIndex);
    }

    public void StartExcuse()
    {
        transition.SetTransitionFinalAction(TransitionToExcuse);
        transition.StartTransition();
    }


    public void FinishConversation()
    {
        //Start vote
        foreach (PlayerData player in currentPlayers)
        {
            player.RefreshTargetForSpecialCards();
        }
        StartVoting();
    }

    public void FinishVote()
    {
        playersLeftToPlay--;
        currentPlayerIndex = (currentPlayerIndex + 1) % playerList.VotersAmount();
        playerList.VoteForPlayer();

        if (playersLeftToPlay == 0)
        {
            amountOfExcusers = 0;
            int result = playerList.AnayzeVoting(roundPlayersToKick, out amountOfExcusers);
            textForVottingResult = "";
            switch (result)
            {

                case -1:
                    textForVottingResult = "Выбранное количество кандидатов недостаточно, так что будет проведено повторное голосование!";
                    transition.SetTransitionFinalAction(ShowVottingResultWithContinueVotting);
                    transition.StartTransition();
                    break;
                case 0:
                    if (!votingSkipped && roundPlayersToKick == 1 && roundsLeft > 1)
                    {
                        //TO DO -- skip votting if possible
                        votingSkipped = true;
                        textForVottingResult = "Не один игрок не был изгнан, но это означает, что в следующем раунде вам нужно будет изгнать на 1 игрока больше!";
                        transition.SetTransitionFinalAction(ShowVottingResultWithFinishVotting);
                        transition.StartTransition();
                    }
                    else
                    {
                        textForVottingResult = "Не один игрок не был изгнан, но в это раунде вы не может так сделать, так что будет проведено повторное голосование!";
                        transition.SetTransitionFinalAction(ShowVottingResultWithContinueVotting);
                        transition.StartTransition();
                    }
                    break;
                case 1:
                    KickPlayers();
                    break;
                case 2:
                    playerList.StartExuses(amountOfExcusers);
                    break;
            }      
        }
        else
        {
            playerList.StartVote(currentPlayerIndex);
        }
    }

    public void ShowVottingResultWithContinueVotting()
    {
        playerList.ShowVottingResult(textForVottingResult, StartVoting);
    } 
    public void ShowVottingResultWithFinishVotting()
    {
        playerList.ShowVottingResult(textForVottingResult, FinishTurn);
    }
    
    public void ShowVottingResultWithFinishVottingWithPlayersKicked()
    {
        playerList.ShowVottingResult(textForVottingResult, FinishTurn, newPlayersToKick);

        foreach (GameObject player in newPlayersToKick)
        {
            PlayerData playerToKick = player.GetComponent<PlayerCard>().Data;

            playerToKick.KickPlayerAction();
            playerList.RemovePlayerCard(playerToKick);
            currentPlayers.Remove(playerToKick);
            kickedPlayers.Add(playerToKick);
        }

        votingSkipped = false;
    }

    public bool IsChangingVoiceAvaliable()
    {
        bool isAvaliable = false;
        foreach (PlayerData player in currentPlayers)
        {
            if (player.ChangeVoice)
            {
                isAvaliable = true;
                break;
            }
        }

        return isAvaliable;
    }

    public void FinishExcuse()
    {
        playersLeftToPlay--;
        currentPlayerIndex = (currentPlayerIndex + 1) % playerList.ExcusersAmount();
        if (playersLeftToPlay == 0)
        {
            //Procceed to changing voices
            bool check = IsChangingVoiceAvaliable();
            if (check)
                playerList.AskForChangeVoices();
            else
            {
                KickPlayers();
            }

        }
        else
        {
            StartExcuse();
        }
    }

    public void KickPlayers()
    {
        newPlayersToKick = playerList.FindPlayersToKick(roundPlayersToKick);

        textForVottingResult = "";
        if (newPlayersToKick.Count == 1) textForVottingResult = "Был изгнан";
        else textForVottingResult = "Были изгнаны";

        transition.SetTransitionFinalAction(ShowVottingResultWithFinishVottingWithPlayersKicked);
        transition.StartTransition();

    }

    private void FinishTurn()
    {
        round++;
        roundsLeft--;
        mandatoryCardToReveal = CardType.NONE;
        foreach (var player in currentPlayers)
        {
            player.Card.Voices = 0;
            player.SetNewPossiblePlayersForVote(currentPlayers);
            player.RefrshMultipriers();
            player.RefreshTargetForSpecialCards();
        }

        if (roundsLeft == 0)
        {
            //Finish game
            transition.SetTransitionMiddleAction(ShowResultwindow);
            transition.StartTransition();

        }
        else
        {
            transition.SetTransitionMiddleAction(StartTurn);
            transition.StartTransition();
        }

    }

    public void StartTurn()
    {

        playerList.gameObject.SetActive(false);
        StartRevealing();
    }

    public void ShowResultwindow()
    {
        resultWindow.ShowResultWindow(currentPlayers, kickedPlayers, familyMode);
    }

    public void StartGame(int playersCount)
    {
        RefreshAllDecks();

        allPlayers.Clear();
        currentPlayers.Clear();
        kickedPlayers.Clear();

        playersAmount = playersCount;
        CreatePlayers();

        currentPlayers.Shuffle();
        playerList.SyncPlayersCardsPositions(currentPlayers);


        playersToBunker = playersCount / 2;
        playersToKick = playersCount - playersToBunker;

        hiddenCards = 6;

        round = 1;

        if (playersToKick > 5) roundsLeft = 5;
        else roundsLeft = playersToKick;


        choosenCatastrophe = (Catastrophe)catastropheDeck.GetRandomCard();
        for(int i = 0; i< bunkerCards.Length; i++)
        {
            bunkerCards[i] = (BunkerCard) bunkerDeck.GetRandomCard();
        }

        foreach (var player in currentPlayers)
        {
            player.Voices = 0;
            player.SetNewPossiblePlayersForVote(currentPlayers);
        }

 //       discord.SendMessageToDiscord("Игра начанается");

        introWindow.StartIntro(choosenCatastrophe, bunkerCards);
    }

    private void CreatePlayers()
    {
        for (int i = 0; i < playersAmount; i++)
        {

            PlayerData newPlayerData = playerList.AddPlayerCard();
            newPlayerData.CleanHand();

            //TO DO -- Chose playername
            newPlayerData.PlayerName = textHandler.GetPlayerName(i);




            GiveCardsToPlayer(newPlayerData);

            //newPlayerData.RevealAllCards();
            
            allPlayers.Add(newPlayerData);
            currentPlayers.Add(newPlayerData);

            string fileName = newPlayerData.PlayerName + ".txt";

            filePath = Path.Combine(Application.dataPath, "Players", fileName);
            string directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }


            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                string stringToAdd = "";

                stringToAdd = $"\t{newPlayerData.PlayerName}";
                writer.WriteLine(stringToAdd);


                stringToAdd = $"Профессия: {newPlayerData.HandCard[CardType.JOB].Name} (Стаж {(newPlayerData.HandCard[CardType.JOB] as JobCard).Exp} ";
                if ((newPlayerData.HandCard[CardType.JOB] as JobCard).Exp == 1)
                {
                    stringToAdd += "год";
                } else if ((newPlayerData.HandCard[CardType.JOB] as JobCard).Exp >= 2 
                    && (newPlayerData.HandCard[CardType.JOB] as JobCard).Exp <= 4)
                {
                    stringToAdd += "года";
                } else
                {
                    stringToAdd += "лет";
                }
                stringToAdd += ")";
                writer.WriteLine(stringToAdd);


                stringToAdd = $"Биология: {newPlayerData.HandCard[CardType.BIOLOGY].Name}";
                writer.WriteLine(stringToAdd);


                stringToAdd = $"Здоровье: {newPlayerData.HandCard[CardType.HEALTH].Name}";

                if ((newPlayerData.HandCard[CardType.HEALTH] as HealthCard).HasStage)
                {
                    stringToAdd += $" (Стадия {(newPlayerData.HandCard[CardType.HEALTH] as HealthCard).Stage})";
                }

                writer.WriteLine(stringToAdd);


                stringToAdd = $"Хобби: {newPlayerData.HandCard[CardType.HOBBY].Name} " +
                    $"(Стаж {(newPlayerData.HandCard[CardType.HOBBY] as HobbyCard).Exp} ";
                if ((newPlayerData.HandCard[CardType.HOBBY] as HobbyCard).Exp == 1)
                {
                    stringToAdd += "год";
                }
                else if ((newPlayerData.HandCard[CardType.HOBBY] as HobbyCard).Exp >= 2
                    && (newPlayerData.HandCard[CardType.HOBBY] as HobbyCard).Exp <= 4)
                {
                    stringToAdd += "года";
                }
                else
                {
                    stringToAdd += "лет";
                }
                stringToAdd += ")";
                writer.WriteLine(stringToAdd);


                stringToAdd = $"Багаж: {newPlayerData.HandCard[CardType.BAGGAGE].Name}";
                writer.WriteLine(stringToAdd);


                stringToAdd = $"Факт: {newPlayerData.HandCard[CardType.FACT].Name}";
                writer.WriteLine(stringToAdd);

                stringToAdd = $"\tОсобые условия";
                writer.WriteLine(stringToAdd);

                stringToAdd = $"1: {newPlayerData.SpecialCards[0].Name}";
                writer.WriteLine(stringToAdd);

                stringToAdd = $" Описание: {newPlayerData.SpecialCards[0].description}";
                writer.WriteLine(stringToAdd);
                //writer.WriteLine($"2: {newPlayerData.SpecialCards[1].Name}");
                //writer.WriteLine($" Описание: {newPlayerData.SpecialCards[1].description}");
            }

        }
    }



    public void RewritePlayersCards()
    {

        foreach (PlayerData newPlayerData in allPlayers)
        {
            string fileName = newPlayerData.PlayerName + ".txt";

            filePath = Path.Combine(Application.dataPath, "Players", fileName);
            string directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }


            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine($"\t{newPlayerData.PlayerName}");
                writer.WriteLine($"Профессия: {newPlayerData.HandCard[CardType.JOB].Name}");
                writer.WriteLine($"Биология: {newPlayerData.HandCard[CardType.BIOLOGY].Name}");
                writer.WriteLine($"Здоровье: {newPlayerData.HandCard[CardType.HEALTH].Name}");
                writer.WriteLine($"Хобби: {newPlayerData.HandCard[CardType.HOBBY].Name}");
                writer.WriteLine($"Багаж: {newPlayerData.HandCard[CardType.BAGGAGE].Name}");
                writer.WriteLine($"Факт: {newPlayerData.HandCard[CardType.FACT].Name}");
                writer.WriteLine($"\tОсобые условия");
                writer.WriteLine($"1: {newPlayerData.SpecialCards[0].Name}");
                writer.WriteLine($" Описание: {newPlayerData.SpecialCards[0].description}");
                writer.WriteLine($"2: {newPlayerData.SpecialCards[1].Name}");
                writer.WriteLine($" Описание: {newPlayerData.SpecialCards[1].description}"); 
                
                if(newPlayerData.SpecialCards.Count == 3)
                {
                    writer.WriteLine($"3: {newPlayerData.SpecialCards[2].Name}");
                    writer.WriteLine($" Описание: {newPlayerData.SpecialCards[2].description}");
                }

            }
        }

        
    }

    private void GiveCardsToPlayer(PlayerData player)
    {
        player.AddCard(bioDeck.GetRandomCard());
        player.AddCard(healthDeck.GetRandomCard());



        player.AddCard(jobDeck.GetRandomCard());
        player.AddCard(hobbyDeck.GetRandomCard());
        player.AddCard(factDeck.GetRandomCard());
        player.AddCard(baggageDeck.GetRandomCard());

        player.AddCard(specialDeck.GetRandomCard());
        //player.AddCard(specialDeck.GetRandomCard());
    }

    private void RefreshAllDecks()
    {
        bioDeck.RefreshCards();
        healthDeck.RefreshCards();
        jobDeck.RefreshCards();
        hobbyDeck.RefreshCards();
        factDeck.RefreshCards();
        baggageDeck.RefreshCards();
        specialDeck.RefreshCards();
        bunkerDeck.RefreshCards();
        catastropheDeck.RefreshCards();
    }

    public Card GetRandomCard(CardType typeOfCardToGet)
    {
        Card newCard = new Card();

        switch (typeOfCardToGet)
        {
            case CardType.BIOLOGY:
                newCard = bioDeck.GetRandomCard();
                break;
            case CardType.BAGGAGE:
                newCard = baggageDeck.GetRandomCard();
                break;
            case CardType.HEALTH:
                newCard = healthDeck.GetRandomCard();
                break;
            case CardType.HOBBY:
                newCard = hobbyDeck.GetRandomCard();
                break;
            case CardType.FACT:
                newCard = factDeck.GetRandomCard();
                break;
            case CardType.JOB:
                newCard = jobDeck.GetRandomCard();
                break;
            case CardType.BUNKER:
                newCard = bunkerDeck.GetRandomCard();
                break;
            case CardType.SPECIAL:
                newCard = specialDeck.GetRandomCard();
                break;
        }

        return newCard;
    }

    public void AddCardToDeck(Card cardToAdd)
    {
        switch (cardToAdd.CardType)
        {
            case CardType.BIOLOGY:
                bioDeck.AddCArd(cardToAdd);
                break;

            case CardType.BAGGAGE:
                baggageDeck.AddCArd(cardToAdd);
                break;

            case CardType.HEALTH:
                healthDeck.AddCArd(cardToAdd);
                break;

            case CardType.JOB:
                jobDeck.AddCArd(cardToAdd);
                break;

            case CardType.HOBBY:
                hobbyDeck.AddCArd(cardToAdd);
                break;

            case CardType.FACT:
                factDeck.AddCArd(cardToAdd);
                break;

        }
    }
}
