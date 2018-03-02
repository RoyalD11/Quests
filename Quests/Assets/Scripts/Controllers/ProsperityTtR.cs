﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS EVENT STILL NEEDS A LOT OF WORK BEFORE IT IS DONE

namespace QuestOTRT
{
    //All players can immediately draw 2 Adventure Cards
    public class ProsperityTtR : GameElement
    {

        void Start()
        {
            play();
        }

        public void play()
        {
            //Creates a new list of players to be filled 
            List<PlayerController> players = new List<PlayerController>();

            //Loops through each game object and adds them to the list of players
            foreach (GameObject player in this.game.players)
            {
                players.Add(player.GetComponent<PlayerController>());
            }

            //Loop through all players to add cards to their hands
            for (int i = 0; i < players.Count; i++)
            {
                //draws 2 cards to the players hand
                players[i].addCards(this.game.deck.DrawAdventureCards(2));
            }
            Debug.Log("Prosperity Throught the Realm Event played");
        }
        
    }
}