using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTempleAction : GOAPAction
{

    private const float CIRCLE_AREA_RADIUS = 2.5f;
    private const int BUILD_POSITION_OFFSET = 1;

    private bool _isTempleBuilt = false;

    public BuildTempleAction()
    {
        AddPreCondition("HasEnoughOreForTemple", true);
        AddPreCondition("HasEnoughWoodForTemple", true);
        AddEffect("BuildTemple", true);
    }

    public override bool CheckProceduralPreconditions(GameObject agent)
    {
        // We get a random position in a circle where the builder position is the center.
        // This random position will be the location of the temple that is going to be built.
        Vector2 position = new Vector2(agent.transform.position.x, agent.transform.position.y) + Random.insideUnitCircle * CIRCLE_AREA_RADIUS;
        Target = agent.GetComponent<Builder>().Construction;
        // Update the position of our target to the calculated position considering the available range to build.
        Target.transform.position = position;
        return true;
    }

    public override bool IsDone()
    {
        return this._isTempleBuilt;
    }

    public override bool Perform(GameObject agent)
    {
        // Instantiate the temple in the position that was previously generated.
        Instantiate(
            Target, // Our temple prefab.
            new Vector2(Target.transform.position.x + BUILD_POSITION_OFFSET, Target.transform.position.y + BUILD_POSITION_OFFSET), // A silly hack to not build the temple behind the builder.
            Quaternion.identity // We do not need any rotation to our temple game object.
            );  
        // As we can perform the temple's construction, we remove the required materials from the village's storage center.
        VillageCenter villageCenter = FindObjectOfType<VillageCenter>();
        villageCenter.CurrentOre -= 10;
        villageCenter.CurrentWood -= 10;

        this._isTempleBuilt = true;

        return true;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        this._isTempleBuilt = false;
        Target = null;
    }
}
