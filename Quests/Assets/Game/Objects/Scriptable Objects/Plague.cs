﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Drawer loses 2 shields, if possible
public class Plague : BaseEvent
{

    public override void apply()
    {
        //throw new System.NotImplementedException();
        List<NetPlayerController> players = GameManager.players;

        foreach (NetPlayerController player in players)
            player.removeShields(true, 2);

        Debug.Log("[Plague.cs:play] Plague complete -> local player loses 2 shields");
    }
}
