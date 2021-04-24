using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageCenter : MonoBehaviour
{
    /*
     * This class represents the warehouse of the village.
     * It should have only one per village and keep track of all the materials that are current store.
     * This is the source for the building goals.
     */
    private int _currentOre;
    private int _currentWood;

    public int CurrentOre { get => _currentOre; set => _currentOre = value; }

    public int CurrentWood { get => _currentWood; set => _currentWood = value; }
}
