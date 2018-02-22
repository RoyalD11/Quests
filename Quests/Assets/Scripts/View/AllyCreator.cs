﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class AllyCreator : CreatorBase<QuestOTRT.Ally>
{
    public override void create(QuestOTRT.Ally card)
    {
        Sprite display = sprites[0];
        GameObject newcard = Instantiate(prefab);
        SpriteRenderer sr = newcard.GetComponent<SpriteRenderer>();
        sr.sprite = display;
    }
}
