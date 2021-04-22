using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPAction : MonoBehaviour
{
    private HashSet<KeyValuePair<string, object>> _preconditions;
    private HashSet<KeyValuePair<string, object>> _effects;

    // Store if the action must be in range to be executed.
    private bool _inRange;

    // An action often has to perform on an object or a place.
    // This is the object that will store this information.
    // Can be null.
    private GameObject _target;

    private float _cost;

    public GOAPAction()
    {
        _preconditions = new HashSet<KeyValuePair<string, object>>();
        _effects = new HashSet<KeyValuePair<string, object>>();
    }

    public void DoReset()
    {
        _inRange = false;
        _target = null;
        Reset();
    }

    /// <summary>
    /// Resets everything that need to be reset before the planning happens again.
    /// </summary>
    public abstract void Reset();

    /// <summary>
    /// Returns if the action was concluded.
    /// </summary>
    /// <returns>if the action is done.</returns>
    public abstract bool IsDone();

    /// <summary>
    /// Procedurally checks if this action can run.
    /// </summary>
    /// <param name="agent">the agent that is assigned to run this task.</param>
    /// <returns>if the current task can run.</returns>
    public abstract bool CheckProceduralPreconditions(GameObject agent);

    /// <summary>
    /// Runs the action.
    /// Returns <code>true</code> if the action was performed successfully or <code>false</code> if something happened and it can be no longer performed.
    /// In this case, the action queue should be cleared.
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    public abstract bool Perform(GameObject agent);

    public abstract bool RequiresInRange();

    public bool IsInRange()
    {
        return _inRange;
    }

    public void SetInRange(bool inRange)
    {
        _inRange = inRange;
    }

    public void AddPreCondition(string key, object value)
    {
        _preconditions.Add(new KeyValuePair<string, object>(key, value));
    }

    public void RemovePreCondition(string key)
    {
        KeyValuePair<string, object> toRemove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> precondition in _preconditions)
        {
            if (precondition.Key.Equals(key))
            {
                toRemove = precondition;
            }
        }
        if (!default(KeyValuePair<string, object>).Equals(toRemove))
        {
            _preconditions.Remove(toRemove);
        }
    }

    public HashSet<KeyValuePair<string, object>> GetPreConditions()
    {
        return _preconditions;
    }

    public void AddEffect(string key, object value)
    {
        _effects.Add(new KeyValuePair<string, object>(key, value));
    }

    public HashSet<KeyValuePair<string, object>> GetEffects()
    {
        return _effects;
    }

    public float GetCost()
    {
        return _cost;
    }

    public GameObject GetTarget()
    {
        return _target;
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
