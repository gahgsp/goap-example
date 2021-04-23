using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreWoodAction : GOAPAction
{

    private bool _storedWood = false;

    public StoreWoodAction()
    {
        AddPreCondition("HasLogs", true);
        AddEffect("HasLogs", false);
        AddEffect("CutWood", true);
    }

    public override bool CheckProceduralPreconditions(GameObject agent)
    {
        VillageCenter villageCenter = FindObjectOfType<VillageCenter>();
        Target = villageCenter.gameObject;
        return villageCenter != null;
    }

    public override bool IsDone()
    {
        return this._storedWood;
    }

    public override bool Perform(GameObject agent)
    {
        ResourcesBag backpack = gameObject.GetComponent<ResourcesBag>();
        backpack.qtyLogs = 0;
        this._storedWood = true;
        return true;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        this._storedWood = false;
        Target = null;
    }
}
