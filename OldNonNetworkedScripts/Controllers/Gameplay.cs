﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class GameElement : MonoBehaviour
{

    protected Gameplay game;

    void Awake()
    {
        findGame();
    }

    void OnEnable()
    {
        findGame();
    }

    private void Update()
    {
        if (game == null)findGame();
    }

    void findGame()
    {
        if (GameObject.FindGameObjectWithTag("Game") != null) game = GameObject.FindGameObjectWithTag("Game").GetComponent<Gameplay>();
        else game = GameObject.FindGameObjectWithTag("ActiveArea").GetComponent<Gameplay>();
    }

}

public class Gameplay : NetworkBehaviour
{
    #region Singleton

    public static Gameplay instance;

    #endregion


    public GameState state;
    public int currPlayer;
    public int activePlayer;

    public AdventureDeckModel AdventureDeck;
    public StoryDeckController StoryDeck;

    public int numPlayers;
    public List<GameObject> players;
    public GameObject playerPrefab;
    public GameObject StatsPrefab;
    public Transform PlayerStats;

    public GameView view;
    public GameObject endTurn;
    public GameObject startTurn;

    public SetupModel stageModels;
    private int sponsorId;
    public QuestController quest;
    public TournamentController tournament;

    private void Awake()
    {
        Debug.Log("[Gameplay.cs:Awake] Starting game initialization...");
        // Get the settings from the main menu

        numPlayers = 0;

        // Declare values
        players = new List<GameObject>();
        state = new GameState();

        currPlayer = -1;
        activePlayer = -1;
        instance = this;
    }

    public GameObject makePlayer()
    {
        numPlayers += 1;

        GameObject player = Instantiate(playerPrefab, this.transform);
        GameObject stats = Instantiate(StatsPrefab, PlayerStats);

        player.name = "Player " + numPlayers;
        stats.name = "Player " + numPlayers + " Stats";

        player.GetComponent<PlayerModel>().index = (numPlayers-1);

        PlayerView tmp = player.GetComponent<PlayerView>();
        tmp.nameText = stats.transform.Find("Name").GetComponent<Text>();
        tmp.rankText = stats.transform.Find("Rank").GetComponent<Text>();
        tmp.shieldText = stats.transform.Find("Shields").GetComponent<Text>();
        tmp.cardsText = stats.transform.Find("Num Cards").GetComponent<Text>();
        tmp.highlight = stats.transform.Find("Highlight").gameObject;

        player.GetComponent<PlayerController>().addManyCards(AdventureDeck.drawMany(12));

        //Instantiate the stageModel for each player that will hold the cards they play for each stage on a quest.
        player.GetComponent<PlayerModel>().cardsPlayed4Quest = new StageModel();

        players.Add(player);
        Debug.Log("[Gamplay.cs:initPlayers] Player " + numPlayers + " initialized with 12 cards");
        return player;
    }

    private void Start()
    {
        // Initialize players with 12 cards
        /*initPlayers();
        setActivePlayer(nextPlayer());
        setCurrPlayer(nextPlayer());

        Debug.Log("[Gameplay.cs:Start] Game initialization complete");*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            view.toggleMenu();
        }

        //Game Rigging to Spawn Boar Hunt card instantly
        if (Input.GetKeyDown(KeyCode.B) && gameObject.tag == "ActiveArea")
        {
            StoryDeck.gameRig('b');
        }

        //Game Rigging to Spawn Chivalrous Deed (+3 shields) card instantly
        if (Input.GetKeyDown(KeyCode.C) && gameObject.tag == "ActiveArea")
        {
            StoryDeck.gameRig('c');
        }

        //Game Rigging to Spawn an Event card instantly
        if (Input.GetKeyDown(KeyCode.E) && gameObject.tag == "ActiveArea")
        {
            StoryDeck.gameRig('e');
        }

        //Game Rigging to Spawn Prosperity Throughout the Kingdom card instantly
        if (Input.GetKeyDown(KeyCode.P) && gameObject.tag == "ActiveArea")
        {
            StoryDeck.gameRig('p');
        }

        //Game Rigging to Spawn a Quest card instantly
        if (Input.GetKeyDown(KeyCode.Q) && gameObject.tag == "ActiveArea")
        {
            StoryDeck.gameRig('q');
        }

        //Game Rigging to Spawn Slay the Dragon card instantly
        if (Input.GetKeyDown(KeyCode.S) && gameObject.tag == "ActiveArea")
        {
            StoryDeck.gameRig('s');
        }

        //Game Rigging to Spawn a Tournement card instantly
        if (Input.GetKeyDown(KeyCode.T) && gameObject.tag == "ActiveArea")
        {
            StoryDeck.gameRig('t');
        }

        //Game Rigging to Spawn King's Call card instantly, used for testing
        if (Input.GetKeyDown(KeyCode.K) && gameObject.tag == "ActiveArea")
        {
            StoryDeck.gameRig('k');
        }

    }

    private void initPlayers()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            
            GameObject player = Instantiate(playerPrefab, this.transform);
            GameObject stats = Instantiate(StatsPrefab, PlayerStats);

            player.name = "Player " + (i + 1);
            stats.name = "Player " + (i + 1) + " Stats";

            player.GetComponent<PlayerModel>().index = i;

            PlayerView tmp = player.GetComponent<PlayerView>();
            tmp.nameText = stats.transform.Find("Name").GetComponent<Text>();
            tmp.rankText = stats.transform.Find("Rank").GetComponent<Text>();
            tmp.shieldText = stats.transform.Find("Shields").GetComponent<Text>();
            tmp.cardsText = stats.transform.Find("Num Cards").GetComponent<Text>();
            tmp.highlight = stats.transform.Find("Highlight").gameObject;

            player.GetComponent<PlayerController>().addManyCards(AdventureDeck.drawMany(12));

            //Instantiate the stageModel for each player that will hold the cards they play for each stage on a quest.
            player.GetComponent<PlayerModel>().cardsPlayed4Quest = new StageModel();

            players.Add(player);
            Debug.Log("[Gamplay.cs:initPlayers] Player " + (i + 1) + " initialized with 12 cards");
        }

    }

    public void addCardsToPlayer(int id, int numCards)
    {
        PlayerController player = players[id].GetComponent<PlayerController>();
        player.addManyCards(AdventureDeck.drawMany(numCards));
        Debug.Log("[Gameplay.cs:addCardsToPlayer] " + numCards + " cards added to player " + (id + 1));
    }

    public int nextPlayer()
    {
        return (currPlayer + 1) % numPlayers;
    }

    public void setActivePlayer(int player)
    {
        if (activePlayer > -1) players[activePlayer].GetComponent<PlayerView>().turnOff();
        activePlayer = player;
        players[player].GetComponent<PlayerView>().turnOn();
        view.ShowPlayerOverlay(player + 1);
        Debug.Log("[Gameplay.cs:setActivePlayer] Active player set to player " + (player + 1));
    }

    public void setCurrPlayer(int player)
    {
        currPlayer = player;
        Debug.Log("[Gameplay.cs:setCurrPlayer] Current player set to player " + (player + 1));
    }

    public void setNextPlayer()
    {
        setActivePlayer(nextPlayer());
        currPlayer = activePlayer;
        Debug.Log("[Gameplay.cs:setNextPlayer] Current and Active player set to player " + (currPlayer + 1));
    }

    public void PlayStoryCard()
    {
        if (state.currCard != null)
        {
            state.currCard.Apply();
        }
    }

    public void StartSponsor()
    {
        view.LoadJoinSponsor();
    }

    public void EndQuest(int stages, int numCardsSponsor)
    {
        PlayerController ctrl = players[sponsorId].GetComponent<PlayerController>();
        ctrl.addManyCards(AdventureDeck.drawMany(stages + numCardsSponsor));
        setNextPlayer();
    }

    public void CreateSponsor(int sponsor)
    {
        if (sponsor < 0)
        {
            StoryDeck.discard();
            setNextPlayer();
        }
        else
        {
            view.LoadSponsor();
        }
    }

    public void CreateTournament(List<int> players)
    {
        if (players.Count == 0)
        {
            StoryDeck.discard();
            setNextPlayer();
        }
        else if (players.Count == 1)
        {
            int numShields = GameObject.FindGameObjectWithTag("CurrStory").GetComponent<TournamentCard>().BonusShields;
            EndTournament(players[0], 1, numShields);
        }
        else
        {
            view.LoadTournament();
            tournament.startTournament(players);
        }
    }

    public void EndTournament(int playerId, int numPlayers, int numBonus)
    {
        players[playerId].GetComponent<PlayerModel>().addShields(numPlayers + numBonus);
        view.promptUser("Player " + (playerId + 1) + " won the tournament and earned " + (numPlayers + numBonus) + " shields.");
        setNextPlayer();
    }

    public void EndTournamentTie(List<int> winners, int numPlayers, int numBonus)
    {
        foreach(int player in winners)
        {
            players[player].GetComponent<PlayerModel>().addShields(numPlayers + numBonus);
        }
        view.promptUser(winners.Count + " players won the tournament and earned " + (numPlayers + numBonus) + " shields.");
        setNextPlayer();
    }

    public void storeSponsors(SetupModel models)
    {
        stageModels = new SetupModel(models);
        sponsorId = activePlayer;
    }
    
    public void CreateQuest(List<int> players)
    {
        if (players.Count == 0)
        {
            int stages = GameObject.FindGameObjectWithTag("CurrStory").GetComponent<QuestCard>().stages;
            EndQuest(stages, stageModels.totalNumCards());
            view.removeSponsor();
            stageModels.discardAll();
            StoryDeck.discard();
        }
        else
        {
            view.LoadQuest();
            quest.startQuest(players, stageModels);
        }
    }

    public void PromptTournament()
    {
        view.LoadJoinTournament();
    }

    public void PromptQuest()
    {
        view.LoadJoinQuest();
    }
}
