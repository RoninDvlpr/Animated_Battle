using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Actions
{
    public List<string> own;
    public List<string> opponent;

    public void PrintToConsole()
    {
        Debug.Log("Own actions:");
        PrintActions(own);
        Debug.Log("Opponent actions:");
        PrintActions(opponent);
    }

    void PrintActions(List<string> actions)
    {
        if (actions.Count > 0)
            Debug.Log($"Attack type: {actions[0]}, opponent reaction: {actions[1]}");
        else
            Debug.Log("No actions");
    }
}
