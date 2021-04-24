using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Villager : MonoBehaviour, IGOAP
{
    private int _stamina = 100;
    private float _moveSpeed = 5f;

    private VillageCenter _villageCenter;

    private void Awake()
    {
        this._villageCenter = FindObjectOfType<VillageCenter>();
    }

    public abstract HashSet<KeyValuePair<string, object>> CreateGoalState();

    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldInformation = new HashSet<KeyValuePair<string, object>>();
        worldInformation.Add(new KeyValuePair<string, object>("HasOre", gameObject.GetComponent<ResourcesBag>().qtyOre > 0));
        worldInformation.Add(new KeyValuePair<string, object>("HasLogs", gameObject.GetComponent<ResourcesBag>().qtyLogs > 0));
        worldInformation.Add(new KeyValuePair<string, object>("HasStamina", this._stamina > 0));
        worldInformation.Add(new KeyValuePair<string, object>("HasEnoughOreForTemple", this._villageCenter.CurrentOre >= 10));
        worldInformation.Add(new KeyValuePair<string, object>("HasEnoughWoodForTemple", this._villageCenter.CurrentWood >= 10));
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

    public void FinishedActions() { 
        // All the actions for the plan were finished.
    }

    public void PlanAborted(GOAPAction aborterAction) {
        // An action for the current plan wasn't completed.
        // After that, we will go back to the planning phase again.
    }

    public void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal) {
        // Nothing to do here as we ensure that our plans will always succeed.
    }

    public void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> actions) {
        // Good job! We have found a plan!
    }

    public int Stamina { get => _stamina; set => _stamina = value; }
}
