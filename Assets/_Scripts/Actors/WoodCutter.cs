using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodCutter : Villager
{
    public override HashSet<KeyValuePair<string, object>> CreateGoalState()
    {
        HashSet<KeyValuePair<string, object>> goals = new HashSet<KeyValuePair<string, object>>();
        goals.Add(new KeyValuePair<string, object>("CutWood", true));
        return goals;
    }
}
