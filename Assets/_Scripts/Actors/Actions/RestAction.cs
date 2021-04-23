﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestAction : GOAPAction
{
    private bool _rested;

    public RestAction()
    {
        AddPreCondition("HasStamina", false);
        AddEffect("MineOre", true);
    }

    public override bool CheckProceduralPreconditions(GameObject agent)
    {
        Barracks barracks = FindObjectOfType<Barracks>(); // TO-DO: Should be a list! We could also keep track to check if the barrack is full!
        Target = barracks.gameObject;
        return barracks != null;
    }

    public override bool IsDone()
    {
        return this._rested;
    }

    public override bool Perform(GameObject agent)
    {
        Villager villager = agent.GetComponent<Villager>();
        villager.Stamina = 100;
        this._rested = true;
        return true;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        this._rested = false;
        Target = null;
    }
}
