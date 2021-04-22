using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineOreAction : GOAPAction
{
    private bool _minedOred = false;
    private float _timeToMine = 3f;
    private float _elapsedMiningTime = 0f;

    public MineOreAction()
    {
        AddPreCondition("HasOre", false);
        AddPreCondition("HasStamina", true);
        AddEffect("HasOre", true);

        Cost = 40;
    }

    public override bool CheckProceduralPreconditions(GameObject agent)
    {
        OreSource[] oreSources = FindObjectsOfType<OreSource>();
        OreSource closestOreSource = null;

        for (int index = 0; index < oreSources.Length; index++)
        {
            if (closestOreSource == null)
            {
                closestOreSource = oreSources[index];
            } else
            {
                if (Vector3.Distance(oreSources[index].transform.position, agent.transform.position) < Vector3.Distance(closestOreSource.transform.position, agent.transform.position)) {
                    closestOreSource = oreSources[index];
                }
            }
        }

        Target = closestOreSource.gameObject;

        return closestOreSource != null;
    }

    public override bool IsDone()
    {
        return this._minedOred;
    }

    public override bool Perform(GameObject agent)
    {
        this._elapsedMiningTime += Time.deltaTime;
        if (this._elapsedMiningTime >= this._timeToMine)
        {
            Miner miner = agent.GetComponent<Miner>();
            miner.Stamina -= Cost;
            ResourcesBag backpack = gameObject.GetComponent<ResourcesBag>();
            backpack.qtyOre += 5;
            this._minedOred = true;
        }
        return true;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        this._minedOred = false;
        this._elapsedMiningTime = 0f;
        Target = null;
    }
}
