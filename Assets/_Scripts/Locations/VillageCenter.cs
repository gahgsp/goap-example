using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageCenter : MonoBehaviour
{

    private int _currentOre;
    private int _currentWood;

    public int CurrentOre { get => _currentOre; set => _currentOre = value; }

    public int CurrentWood { get => _currentWood; set => _currentWood = value; }
}
