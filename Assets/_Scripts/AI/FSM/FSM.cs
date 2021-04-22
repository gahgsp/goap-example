using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    private Stack<FSMState> _states = new Stack<FSMState>();

    public delegate void FSMState(FSM fsm, GameObject owner);

    public void Update(GameObject owner)
    {
        if (_states.Peek() != null)
        {
            _states.Peek().Invoke(this, owner);
        }
    }

    public void PushState(FSMState state)
    {
        _states.Push(state);
    }

    public void PopState()
    {
        _states.Pop();
    }
}
