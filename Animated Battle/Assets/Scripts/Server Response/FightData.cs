using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FightData
{
    public List<Step> steps;

    public static FightData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<FightData>(jsonString);
    }

    public void PrintToConsole()
    {
        foreach (Step step in steps)
            step.PrintToConsole();
    }
}
