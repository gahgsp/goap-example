using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutWoodAction : GOAPAction
{

    private bool _cutLog = false;
    private float _timeToCut = 3.5f;
    private float _elapsedCuttingTime = 0f;

    public CutWoodAction()
    {
        AddPreCondition("HasLogs", false);
        AddPreCondition("HasStamina", true);
        AddEffect("HasLogs", true);

        Cost = 80;
    }

    public override bool CheckProceduralPreconditions(GameObject agent)
    {
        Tree[] trees = FindObjectsOfType<Tree>();
        Tree closestTree = null;

        for (int index = 0; index < trees.Length; index++)
        {
            if (closestTree == null)
            {
                closestTree = trees[index];
            }
            else
            {
                if (Vector3.Distance(trees[index].transform.position, agent.transform.position) < Vector3.Distance(closestTree.transform.position, agent.transform.position))
                {
                    closestTree = trees[index];
                }
            }
        }

        Target = closestTree.gameObject;

        return closestTree != null;
    }

    public override bool IsDone()
    {
        return this._cutLog;
    }

    public override bool Perform(GameObject agent)
    {
        this._elapsedCuttingTime += Time.deltaTime;
        if (this._elapsedCuttingTime >= this._timeToCut)
        {
            WoodCutter woodCutter = agent.GetComponent<WoodCutter>();
            woodCutter.Stamina -= Cost;
            ResourcesBag backpack = gameObject.GetComponent<ResourcesBag>();
            backpack.qtyLogs += 10;
            this._cutLog = true;
        }
        return true;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        this._cutLog = false;
        this._elapsedCuttingTime = 0f;
        Target = null;
    }
}
