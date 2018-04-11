﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageModel
{

    public List<AdventureCard> cardsPlayed;

    public StageModel()
    {
        cardsPlayed = new List<AdventureCard>();
    }

    public StageModel(int[] cards)
    {
        cardsPlayed = new List<AdventureCard>();
        foreach(int card in cards)
        {
            cardsPlayed.Add(GameManager.instance.dict.findCard(card) as AdventureCard);
        }
    }

    //Returns the number of cards for that stage
    public int Count
    {
        get
        {
            return cardsPlayed.Count;
        }
    }

    public void RemoveAll()
    {
        cardsPlayed.Clear();
    }

    public bool Contains(string name)
    {
        return (cardsPlayed.Find(i => i.name == name) != null);
    }

    public bool containsAmour()
    {
        bool tag = false;
        for (int i = 0; i < cardsPlayed.Count; i++)
        {
            if (cardsPlayed[i].type == AdventureCardType.AMOUR)
            {
                tag = true;
                break;
            }
        }
        return tag;
    }

    public bool containsTest()
    {
        return (cardsPlayed.Find(i => i.type == AdventureCardType.TEST) != null);
    }

    public bool containsFoe()
    {
        return (cardsPlayed.Find(i => i.type == AdventureCardType.FOE) != null);
    }

    public bool isEmpty()
    {
        return (cardsPlayed.Count == 0);
    }

    public bool isCombat()
    {
        return containsFoe();
    }

    public List<AdventureCard> getAllies()
    {
        return (cardsPlayed.FindAll(i => i.type == AdventureCardType.ALLY));
    }

    public List<AdventureCard> getFoes()
    {
        return (cardsPlayed.FindAll(i => i.type == AdventureCardType.FOE));
    }

    public List<AdventureCard> getAmours()
    {
        return (cardsPlayed.FindAll(i => i.type == AdventureCardType.AMOUR));
    }

    public List<AdventureCard> getWeapons()
    {
        return (cardsPlayed.FindAll(i => i.type == AdventureCardType.WEAPON));
    }

    public List<AdventureCard> getTests()
    {
        return (cardsPlayed.FindAll(i => i.type == AdventureCardType.TEST));
    }

    public List<AdventureCard> removeAllies()
    {
        List<AdventureCard> ret = getAllies();
        if (ret == null) return new List<AdventureCard>();
        foreach (AdventureCard card in ret)
        {
            cardsPlayed.Remove(card);
        }
        return ret;
    }

    public List<AdventureCard> removeAmours()
    {
        List<AdventureCard> ret = getAmours();
        if (ret == null) return new List<AdventureCard>();
        foreach (AdventureCard card in ret)
        {
            cardsPlayed.Remove(card);
        }
        return ret;
    }

    public StageType stageType()
    {
        return (isCombat()) ? StageType.COMBAT : StageType.BIDDING;
    }

    public bool isTest()
    {
        return containsTest();
    }

    //Adds one adventure card to the list
    public void Add(AdventureCard card)
    {
        if (cardsPlayed == null) cardsPlayed = new List<AdventureCard>();
        cardsPlayed.Add(card);
    }

    //Adds a list of adventure cards to the list
    public void addList(List<AdventureCard> cards)
    {
        Debug.Log("[StageModel.cs:addList] Adding list of cards to players cards played");
        if (cards == null) cards = new List<AdventureCard>();
        this.cardsPlayed.AddRange(cards);
    }

    //Remove one adventure card from the list, this can be used to remove the amour card at the end of the quest. So we can keep the other cards
    public void Remove(AdventureCard card)
    {
        if (cardsPlayed == null) return;
        cardsPlayed.Remove(card);
    }

    //Will remove all weapons cards from the list, this can be used for removing the weapons at the end of each stage
    public List<AdventureCard> RemoveWeapons()
    {
        if (cardsPlayed == null) return new List<AdventureCard>();
        List<AdventureCard> toRemove = new List<AdventureCard>();

        for (int i = 0; i < cardsPlayed.Count; i++)
        {
            if (cardsPlayed[i].type == AdventureCardType.WEAPON)
            {
                toRemove.Add(cardsPlayed[i]);
            }
        }

        foreach (AdventureCard card in toRemove)
        {
            cardsPlayed.Remove(card);
        }
        return toRemove;
    }

    public bool validState()
    {
        //makes sure that the stage is not empty
        if (cardsPlayed.Count == 0)
        {
            Debug.Log("[StageModel.cs:validState] Error in stage: No cards played");
            return false;
        }


        //if no foe and no test
        if (cardsPlayed.Find(i => i.type == AdventureCardType.TEST) == null && cardsPlayed.Find(i => i.type == AdventureCardType.FOE) == null)
        {
            Debug.Log("[StageModel.cs:validState] Error in stage: Contains only weapons");
            return false;
        }

        Debug.Log("[StageModel.cs:validState] Stage is in a valid state");
        return true;
    }

    public void Empty()
    {
        cardsPlayed.Clear();
    }

    //Gets the total BP of the stage
    public int totalBP()
    {
        int total = 0;

        foreach (AdventureCard card in cardsPlayed)
        {
            total += card.getBP();
        }

        return total;
    }

    public AdventureCard getTest()
    {
        return cardsPlayed.Find(i => i.type == AdventureCardType.TEST);
    }

    public int totalBids()
    {
        int total = 0;

        foreach (AdventureCard card in cardsPlayed)
        {
            total += card.getBids();
        }

        return total;
    }

    public List<int> getIndexList()
    {
        List<int> ret = new List<int>();
        foreach(AdventureCard card in cardsPlayed)
        {
            ret.Add(card.index);
        }
        return ret;
    }
}