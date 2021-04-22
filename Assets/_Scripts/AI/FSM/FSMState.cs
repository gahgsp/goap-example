using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FSMState
{
    void Update(FSMState fsm, GameObject owner);
}
