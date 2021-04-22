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
        AddEffect("HasOre", true);
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

        SetTarget(closestOreSource.gameObject);

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
        SetTarget(null);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
