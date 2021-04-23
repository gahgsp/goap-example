using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestAction : GOAPAction
{
    private bool _rested;
    private float _timeToRest = 8f;
    private float _elapsedTimeResting = 0f;

    private Barracks _currentBarracks;

    public RestAction()
    {
        AddPreCondition("HasStamina", false);
        AddEffect("MineOre", true);
        AddEffect("CutWood", true);
    }

    public override bool CheckProceduralPreconditions(GameObject agent)
    {
        Barracks barracks = FindObjectOfType<Barracks>();

        // We need to keep track of the selected barrack because once we finish this action,
        // the values and status of the barrack should be reseted.
        this._currentBarracks = barracks;

        Target = barracks.gameObject;
        return barracks != null && barracks.CurrRestingVillagers + 1 <= barracks.MaxRestingVillagers;
    }

    public override bool IsDone()
    {
        return this._rested;
    }

    public override bool Perform(GameObject agent)
    {
        if (this._elapsedTimeResting == 0)
        {
            // This action takes time to be performed, that's why this method will be executed more than one time.
            // We need to add this village to the barrack's count only once.
            this._currentBarracks.CurrRestingVillagers += 1;
        }

        this._elapsedTimeResting += Time.deltaTime;

        if (this._elapsedTimeResting >= this._timeToRest)
        {
            Villager villager = agent.GetComponent<Villager>();
            villager.Stamina = 100;
            this._rested = true;
        }

        return true;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        ResetBarracks();
        this._rested = false;
        this._elapsedTimeResting = 0f;
        Target = null;
    }

    private void ResetBarracks()
    {
        if (this._rested)
        {
            this._currentBarracks.CurrRestingVillagers -= 1;
            this._currentBarracks = null;
        }
    }
}
