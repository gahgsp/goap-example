using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPAgent : MonoBehaviour
{
    /// <summary>
    /// The Agent's state machine.
    /// </summary>
    private FSM _stateMachine;

    /// <summary>
    /// State where the Agent finds something to do.
    /// </summary>
    private FSM.FSMState _idleState;
    /// <summary>
    /// State where the Agent moves to the target / location.
    /// </summary>
    private FSM.FSMState _moveToState;
    /// <summary>
    /// State where the Agent will perform an action to reach the goal.
    /// </summary>
    private FSM.FSMState _performActionState;

    /// <summary>
    /// A list containing all the actions that this Agent can perform.
    /// </summary>
    private HashSet<GOAPAction> _availableActions;
    /// <summary>
    /// A list containing all the actions planned to reach the current goal.
    /// </summary>
    private Queue<GOAPAction> _currentPlanActions;

    /// <summary>
    /// The class that will provide all the information from our World and will listen for feedbacks on planning.
    /// </summary>
    private IGOAP _worldDataProvider;
    /// <summary>
    /// The class responsible for building a plan based on a specific goal to reach.
    /// </summary>
    private GOAPPlanner _planner;

    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = new FSM();

        _availableActions = new HashSet<GOAPAction>();
        _currentPlanActions = new Queue<GOAPAction>();

        _planner = new GOAPPlanner();

        FindWorldDataProvider();
        CreateIdleState();
        CreateMoveToState();
        CreatePerformActionState();

        _stateMachine.PushState(_idleState);

        LoadAgentActions();
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Update(this.gameObject);
    }

    private void CreateIdleState()
    {
        _idleState = (fsm, owner) => 
        {
            // Retrieve all the World informations and the Goal we will plan for.
            HashSet<KeyValuePair<string, object>> worldState = _worldDataProvider.GetWorldState();
            HashSet<KeyValuePair<string, object>> goal = _worldDataProvider.CreateGoalState();

            Queue<GOAPAction> plan = _planner.Plan(gameObject, _availableActions, worldState, goal);
            if (plan != null)
            {
                // Found a plan!
                _currentPlanActions = plan;
                _worldDataProvider.PlanFound(goal, plan);

                fsm.PopState(); // Moving to the Perform Action State!
                fsm.PushState(_performActionState);
            } else
            {
                // Did not find a plan!
                _worldDataProvider.PlanFailed(goal);
                fsm.PopState();
                fsm.PushState(_idleState); // Returning to Idle State.
            }
        };
    }

    private void CreateMoveToState()
    {
        _moveToState = (fsm, owner) =>
        {
            GOAPAction currentAction = _currentPlanActions.Peek();
            if (currentAction.RequiresInRange() && currentAction.Target == null)
            {
                fsm.PopState(); // Move State
                fsm.PopState(); // Perform State
                fsm.PushState(_idleState);
                return;
            }
            if (_worldDataProvider.MoveAgent(currentAction))
            {
                fsm.PopState();
            }
        };
    }

    private void CreatePerformActionState()
    {
        _performActionState = (fsm, owner) =>
        {
            if (!HasPlan())
            {
                // No actions to perform, so back to Idle!
                fsm.PopState();
                fsm.PushState(_idleState);
                _worldDataProvider.FinishedActions();
                return;
            }

            GOAPAction currentAction = _currentPlanActions.Peek();
            if (currentAction.IsDone())
            {
                // The action is already done, so we remove it to perform the next one.
                _currentPlanActions.Dequeue();
            }

            if (HasPlan())
            {
                // Performing the next action...
                currentAction = _currentPlanActions.Peek();
                bool inRange = currentAction.RequiresInRange() ? currentAction.IsInRange() : true;
                if (inRange)
                {
                    bool success = currentAction.Perform(owner);
                    if (!success)
                    {
                        // The action failed, so we need to do the planning again.
                        fsm.PopState();
                        fsm.PushState(_idleState);
                        CreateIdleState();
                        _worldDataProvider.PlanAborted(currentAction);
                    }
                }
                else
                {
                    // If not in range, we need to move to the target first...
                    fsm.PushState(_moveToState);
                }
            }
            else
            {
                // All actions finished. Back to planning again!
                fsm.PopState();
                fsm.PushState(_idleState);
                _worldDataProvider.FinishedActions();
            }
        };
    }

    private bool HasPlan()
    {
        return _currentPlanActions.Count > 0;
    }

    private void FindWorldDataProvider()
    {
        foreach (Component component in gameObject.GetComponents(typeof(Component)))
        {
            if (typeof(IGOAP).IsAssignableFrom(component.GetType()))
            {
                _worldDataProvider = (IGOAP)component;
                return;
            }
        }
    }

    private void LoadAgentActions()
    {
        GOAPAction[] actions = gameObject.GetComponents<GOAPAction>();
        foreach (GOAPAction action in actions)
        {
            _availableActions.Add(action);
        }
    }
}
