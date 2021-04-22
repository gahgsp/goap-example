using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Villager : MonoBehaviour, IGOAP
{
    private int _stamina = 100;
    private float _moveSpeed = 5f;

    public abstract HashSet<KeyValuePair<string, object>> CreateGoalState();

    public void FinishedActions() {}

    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldInformation = new HashSet<KeyValuePair<string, object>>();
        worldInformation.Add(new KeyValuePair<string, object>("HasOre", gameObject.GetComponent<ResourcesBag>().qtyOre > 0));
        worldInformation.Add(new KeyValuePair<string, object>("HasStamina", this._stamina > 0));
        return worldInformation;
    }

    public bool MoveAgent(GOAPAction nextAction)
    {
        float step = this._moveSpeed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextAction.Target.transform.position, step);

        if (gameObject.transform.position.Equals(nextAction.Target.transform.position))
        {
            nextAction.SetInRange(true);
            return true;
        } else
        {
            return false;
        }

    }

    public void PlanAborted(GOAPAction aborterAction) {}

    public void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal) {}

    public void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> actions) {}

    public int Stamina { get => _stamina; set => _stamina = value; }
}
