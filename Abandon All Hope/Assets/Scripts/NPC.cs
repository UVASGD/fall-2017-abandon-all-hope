﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//done: When player walks into NPC's range, dialogue box appears, Press button to initiate dialogue, press button to continue dialogue
//to do: remove dialogue box when NPC dialogue is exhausted and the player presses Z again, make components more generalized so they work for any NPC
public class NPC : MonoBehaviour
{
    private bool alreadyPlayed = false;
    private bool TouchingPlayer = false;
    [SerializeField]
    Canvas messageCanvas;
    public GameObject npctext;
    Text npcText;
    private bool initText = true; //flag for if the initial "press to talk" text should be showing or not
    public string[] DialogueSequence = new string []{ "Welcome", "to", "Hell!" };
    private int SeqInd = 0;
    private bool DoneTalking = false; //flag for if the NPC's dialogue has been exhausted
    private int waitframes = 0;
    void Start()
    {
        messageCanvas.enabled = false;
        npcText = npctext.GetComponent<Text>();
        npcText.text = "Press Z to Interact";
        initText = true;
    }

    void Update ()
    {
        if (DoneTalking == false && waitframes == 0)
        {
            if (TouchingPlayer == true && alreadyPlayed == false)
            {
                npcText.text = DialogueSequence[SeqInd];
                TurnOnMessage();
                
                
                    SeqInd++;
                if (SeqInd == (DialogueSequence.Length))
                {
                    print("done");
                    DoneTalking = true;

                    SeqInd = 0;
                    print(SeqInd);
                    waitframes = 220;
                }

                initText = false;
                waitframes = 220;
            }
            else if (TouchingPlayer == true && initText == true && alreadyPlayed == false)
            {
                npcText.text = "Press Z to Interact";
                TurnOnMessage();
            }
            
        }
        else if(waitframes != 0)
        {
            waitframes--;
        }
        else if(DoneTalking == true)
        {
            TurnOffMessage();
        }
           
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            TouchingPlayer = true;
            //TurnOnMessage();
        }
    }

    private void TurnOnMessage()
    {
        messageCanvas.enabled = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            TouchingPlayer = false;
            TurnOffMessage();
            DoneTalking = false;
            SeqInd = 0;
            alreadyPlayed = true;
        }
    }

    private void TurnOffMessage()
    {
        messageCanvas.enabled = false;
        if(initText == false)
        {
            initText = true;
        }
    }
}
