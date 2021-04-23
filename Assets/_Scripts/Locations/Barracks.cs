using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : MonoBehaviour
{
    private int _maxRestingVillagers = 1;
    private int _currRestingVillagers = 0;

    public int CurrRestingVillagers { get => _currRestingVillagers; set => _currRestingVillagers = value; }

    public int MaxRestingVillagers { get => _maxRestingVillagers; set => _maxRestingVillagers = value; }
}
