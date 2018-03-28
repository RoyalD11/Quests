﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sponsor : GameElement {


    public Transform storyCardTransform;
    public GameObject questCard;
    public static int stages;
            
    public GameObject[] stagesObjects;
    public static StageModel[] stageModels;
            
    public GameObject promptBox;
    public Text promptText;

    bool testFlag = false;

    public GameObject[] questStagesObjects;

private void OnEnable()
    {
        if (storyCardTransform.GetChild(0) != null)
        {
            questCard = storyCardTransform.GetChild(0).gameObject;
        }

        stages = questCard.GetComponent<QuestCard>().stages;

        for(int i=0; i<stages; i++)
        {
            stagesObjects[i].SetActive(true);
        }

        stageModels = new StageModel[stages];

        for (int i=0; i<stages; i++)
        {
            stageModels[i] = new StageModel();
        }
    }

    public void promptUser(string message)
    {
        Debug.Log("the message is: " + message);
             
        promptText.text = message;
        promptBox.SetActive(true);
    }

    public bool testValid(int id, Draggable d)
    {
        testFlag = false;

        AdventureCard currCard = d.gameObject.GetComponent<AdventureCard>();

        if (currCard.type == AdventureCard.Type.AMOUR || currCard.type == AdventureCard.Type.ALLY) return false;

        List<AdventureCard> currStage = new List<AdventureCard>(stagesObjects[id].GetComponentsInChildren<AdventureCard>());

        if (currStage.Find(i => i.type == AdventureCard.Type.TEST) != null) return false;

        if ((currCard.type == AdventureCard.Type.FOE) && currStage.Find(i => i.type == AdventureCard.Type.FOE) != null) return false;

        if ((currCard.type == AdventureCard.Type.WEAPON) && 
            ((currStage.Find(i => i.type == AdventureCard.Type.TEST) != null) || 
            !(currStage.Find(i => i.type == AdventureCard.Type.FOE) != null) || 
            (currStage.Find(i=>i.Name == currCard.Name) != null))) return false;

        //This nested for loop is to check if there is a test already in play
        for (int i = 0; i < stages; i++)
        {
            List<AdventureCard> testStage = new List<AdventureCard>(stagesObjects[i].GetComponentsInChildren<AdventureCard>());
            for(int j = 0; j<testStage.Count; j++)
            {
                if (testStage[j].type == AdventureCard.Type.TEST)
                {
                    testFlag = true;
                }
            }
        }

        if ((currCard.type == AdventureCard.Type.TEST) && ((currStage.Count > 0) || testFlag)) return false;

        
        return true;
    }

    public bool validateStages()
    {
        Debug.Log("In the validate stage method");
        bool valid = true;
        for (int i = 0; i < stages; i++)
        {
            if (!stageModels[i].validState())
            {
                promptUser("There was a errer with stage " + (i + 1) + ". Please go back and fix it.");
                valid = false;
            }
        }

        if (valid)
        {
            Debug.Log("Currently all stages passed");
        }
        else
        {
            Debug.Log("Currently all stages did not pass");
            for (int i=0; i<stages; i++)
            {
                stageModels[i].RemoveAll();
            }
        }
        return valid;
    }

    public void End()
    {
        Debug.Log("Currently in the sponsor end function, num stages is " + stages);

        for(int i=0; i<stages; i++)
        {
            List<AdventureCard> tmp = new List<AdventureCard>(stagesObjects[i].GetComponentsInChildren<AdventureCard>());
            stageModels[i].addList(tmp);
        }

        //if (validateStages())
        //{
            questCard = null;
            stages = 0;
            List<AdventureCard> allCards = new List<AdventureCard>();
            List<StageModel> allStages = new List<StageModel>();
            for (int i = 0; i < stages; i++)
            {
                allCards.AddRange(stageModels[i].cardsPlayed);
                foreach (Transform child in stagesObjects[i].transform)
                {
                    child.SetParent(questStagesObjects[i].transform);
                }
                stagesObjects[i].SetActive(false);
                allStages.Add(stageModels[i]);
            }

            this.game.players[this.game.currPlayer].GetComponent<PlayerController>().removeCards(allCards);
            this.game.storeSponsors(allStages);
            this.gameObject.SetActive(false);
            this.game.setNextPlayer();
        //}
    }
}
