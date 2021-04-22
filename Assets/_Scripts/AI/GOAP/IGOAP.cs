using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGOAP
{
    HashSet<KeyValuePair<string, object>> GetWorldState();

    HashSet<KeyValuePair<string, object>> CreateGoalState();

    void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal);

    void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> actions);

    void FinishedActions();

    void PlanAborted(GOAPAction aborterAction);

    bool MoveAgent(GOAPAction nextAction);
}
