﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS EVENT STILL NEEDS A LOT OF WORK BEFORE IT IS DONE

namespace QuestOTRT
{
    //Lowest ranked player(s) immediately recieve 2 Adventure Cards
    public class QueensFavor : GameElement
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
            



            int curLowestRank = (int)Rank.Squire; //keeps track of curent lowest rank
            int size = 0; //this is used to determine who is the lowest rank
            int i = 0;
            while (i < players.Count)
            {
                //checks if the current players rank is <= to current lowest rank being checked
                if ((int)players[i].getRank() <= curLowestRank)
                {
                    //add a card and add 1 to the amount of players who have drawn 2 cards
                    players[i].addCards(this.game.deck.DrawAdventureCards(2));
                    size++;
                }

                //if no players have this current lowest rank, and no players have been given cards, try next highest rank
                if (i == players.Count - 1 && size == 0)
                {
                    curLowestRank++;
                    i = 0;
                }
                else {
                    i++;
                }
                
            }
            Debug.Log("Queens Favour event Completed");
        }
    }
}