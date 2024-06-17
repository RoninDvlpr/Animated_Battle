using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class FightData
{
    public List<Step> steps;


    public List<FightTurn> GetFightTurns()
    {
        List<FightTurn> turns = new List<FightTurn>();
        for (int i = 0; i < steps.Count; i++)
                turns.AddRange(steps[i].FightTurns);
        return turns;
    }

    public void PrintToConsole()
    {
        foreach (Step step in steps)
            step.PrintToConsole();
    }

    public static FightData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<FightData>(jsonString);
    }
}
