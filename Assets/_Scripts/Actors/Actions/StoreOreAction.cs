using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreOreAction : GOAPAction
{
    private bool _storedOre;

    public StoreOreAction()
    {
        AddPreCondition("HasOre", true);
        AddEffect("HasOre", false);
        AddEffect("MineOre", true);
    }

    public override bool CheckProceduralPreconditions(GameObject agent)
    {
        VillageCenter villageCenter = FindObjectOfType<VillageCenter>();
        Target = villageCenter.gameObject;
        return villageCenter != null;
    }

    public override bool IsDone()
    {
        return this._storedOre;
    }

    public override bool Perform(GameObject agent)
    {
        ResourcesBag backpack = gameObject.GetComponent<ResourcesBag>();

        // Add the collected ore quantity to the village center...
        VillageCenter villageCenter = FindObjectOfType<VillageCenter>();
        villageCenter.CurrentOre += backpack.qtyOre;

        // Then remove the ore from the backpack.
        backpack.qtyOre = 0;

        this._storedOre = true;
        return true;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        this._storedOre = false;
        Target = null;
    }
}
