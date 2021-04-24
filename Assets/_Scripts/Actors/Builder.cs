using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Villager
{
    
    [SerializeField]
    private GameObject _constructionObject;

    public override HashSet<KeyValuePair<string, object>> CreateGoalState()
    {
        // TO-DO: The builder has the ability to build more than one type of building.
        // Maybe the builder could have some specializations and different goals.
        HashSet<KeyValuePair<string, object>> goals = new HashSet<KeyValuePair<string, object>>();
        goals.Add(new KeyValuePair<string, object>("BuildTemple", true));
        return goals;
    }

    public GameObject Construction { get => _constructionObject; }
}
