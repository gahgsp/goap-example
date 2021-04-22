using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPPlanner
{
    public Queue<GOAPAction> Plan(GameObject agent, HashSet<GOAPAction> availableActions, HashSet<KeyValuePair<string, object>> worldState, HashSet<KeyValuePair<string, object>> goal)
    {
        foreach (GOAPAction action in availableActions)
        {
            action.DoReset();
        }

        // Check if we can run the action based on it's pre-conditions
        // and store them.
        HashSet<GOAPAction> possibleActions = new HashSet<GOAPAction>();
        foreach (GOAPAction action in availableActions)
        {
            if (action.CheckProceduralPreconditions(agent))
            {
                possibleActions.Add(action);
            }
        }

        // Build up a tree that will create a path to the goal.
        List<Node> leaves = new List<Node>();

        // Building the graph.
        Node start = new Node(null, 0, worldState, null);
        bool success = BuildGraph(start, leaves, possibleActions, goal);

        if (!success)
        {
            // We weren't able to find a path to the solution!
            return null;
        }

        // Search and find the cheapest leaf in the graph.
        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            } else
            {
                if (leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        // Now we will follow a regressive path through the parents.
        List<GOAPAction> result = new List<GOAPAction>();
        Node node = cheapest;
        while (node != null)
        {
            if (node.action != null)
            {
                result.Insert(0, node.action);
            }
            node = node.parent;
        }

        // With a list in the correct order of the actions, we will build the queue for the plan.
        Queue<GOAPAction> plan = new Queue<GOAPAction>();
        foreach (GOAPAction actionInPlan in result)
        {
            plan.Enqueue(actionInPlan);
        }
        return plan;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, HashSet<GOAPAction> possibleActions, HashSet<KeyValuePair<string, object>> goal)
    {
        bool foundSolution = false;
        // We iterate through each action to see if it is possible to use it.
        foreach(GOAPAction action in possibleActions)
        {
            if (InState(action.GetPreConditions(), parent.state))
            {
                // Apply the current Action's Effects to the Parent State.
                HashSet<KeyValuePair<string, object>> currentState = PopulateState(parent.state, action.GetEffects());
                Node node = new Node(parent, parent.cost + action.GetCost(), currentState, action);
                if (InState(goal, currentState))
                {
                    // Nice! We found a solution!
                    leaves.Add(node);
                    foundSolution = true;
                } else
                {
                    // We did not find a solution yet, so let's test all the others actions and branch out the tree.
                    HashSet<GOAPAction> actionsSubset = BuildActionsSubset(possibleActions, action);
                    bool foundAnotherSolution = BuildGraph(node, leaves, actionsSubset, goal);
                    if (foundAnotherSolution)
                    {
                        foundSolution = true;
                    }
                }
            }
        }
        return foundSolution;
    }

    private bool InState(HashSet<KeyValuePair<string, object>> test, HashSet<KeyValuePair<string, object>> state)
    {
        bool allStatesMatch = true;
        foreach (KeyValuePair<string, object> t in test)
        {
            bool match = false;
            foreach (KeyValuePair<string, object> s in state)
            {
                if (s.Equals(t))
                {
                    match = true;
                    break;
                }
            }
            if (!match)
            {
                allStatesMatch = false;
            }
        }
        return allStatesMatch;
    }

    private HashSet<KeyValuePair<string, object>> PopulateState(HashSet<KeyValuePair<string, object>> currentState, HashSet<KeyValuePair<string, object>> stateChange)
    {
        // Creating a new copy for the states.
        HashSet<KeyValuePair<string, object>> states = new HashSet<KeyValuePair<string, object>>();
        foreach (KeyValuePair<string, object> s in currentState)
        {
            states.Add(new KeyValuePair<string, object>(s.Key, s.Value));
        }

        foreach (KeyValuePair<string, object> change in stateChange)
        {
            // If the key exists in the current state, we update the value.
            bool exists = false;
            foreach (KeyValuePair<string, object> state in states)
            {
                if (states.Equals(change))
                {
                    exists = true;
                    break;
                }
            }
            if (exists)
            {
                states.RemoveWhere((KeyValuePair<string, object> kvp) => { return kvp.Key.Equals(change.Key); });
                KeyValuePair<string, object> updated = new KeyValuePair<string, object>(change.Key, change.Value);
                states.Add(updated);
            } else
            {
                // If the key does not exists in the current state, we add a new value.
                states.Add(new KeyValuePair<string, object>(change.Key, change.Value));
            }
        }
        return states;
    }

    private HashSet<GOAPAction> BuildActionsSubset(HashSet<GOAPAction> actions, GOAPAction actionToRemove)
    {
        HashSet<GOAPAction> actionSubset = new HashSet<GOAPAction>();
        foreach (GOAPAction action in actions)
        {
            if (!action.Equals(actionToRemove))
            {
                actionSubset.Add(action);
            }
        }
        return actionSubset;
    }

    private class Node
    {
        public Node parent;
        public float cost;
        public HashSet<KeyValuePair<string, object>> state;
        public GOAPAction action;

        public Node(Node parent, float cost, HashSet<KeyValuePair<string, object>> state, GOAPAction action)
        {
            this.parent = parent;
            this.cost = cost;
            this.state = state;
            this.action = action;
        }
    }

}
