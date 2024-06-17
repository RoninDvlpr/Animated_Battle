using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Actions : IEnumerable
{
    public List<string> own;
    public List<string> opponent;
    List<FightTurn> fightTurns;
    public List<FightTurn> FightTurns
    { 
        get
        {
            if (fightTurns == null)
                ParseFightTurns();
            return fightTurns;
        }
    }


    #region Server Response Parsing

    void ParseFightTurns()
    {
        fightTurns = new List<FightTurn>();

        ParseFightTurn(own, Fighters.Player);
        ParseFightTurn(opponent, Fighters.Opponent);
    }

    void ParseFightTurn(List<string> actions, Fighters attacker)
    {
        if (ActionsDataArePresent(actions))
        {
            AttackTypes? attack = StringToAttackType(actions[0]);
            DefenseTypes? defense = StringToDefenseType(actions[1]);
            if (attack != null && defense != null)
                fightTurns.Add(new FightTurn(attacker, attack.Value, defense.Value));
        }
    }

    bool ActionsDataArePresent(List<string> actions)
    {
        if (actions == null)
        {
            Debug.LogWarning("Given actions wasn't set during server response deserealization!");
            return false;
        }
        else if (actions.Count == 0)
            return false;
        else if (actions.Count < 2)
        {
            Debug.Log("One of given actions wasn't set!");
            return false;
        }
        else
            return true;
    }

    AttackTypes? StringToAttackType(string action)
    {
        switch (action)
        {
            case "standard":
                return AttackTypes.Standard;
            case "crit":
                return AttackTypes.Crit;
            default:
                Debug.LogWarning($"Can't parse string {action} into attack action!");
                return null;
        }
    }

    DefenseTypes? StringToDefenseType(string action)
    {
        switch (action)
        {
            case "standard":
                return DefenseTypes.None;
            case "block":
                return DefenseTypes.Block;
            case "evasion":
                return DefenseTypes.Evasion;
            default:
                Debug.LogWarning($"Can't parse string {action} into defense action!");
                return null;
        }
    }

    #endregion

    #region Debug

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

    #endregion

    public IEnumerator GetEnumerator()
    {
        return new ActionsEnumerator(own, opponent);
    }
}

public class ActionsEnumerator : IEnumerator
{
    List<List<string>> actions;
    int currentActionIndex = -1;


    public ActionsEnumerator(List<string> own, List<string> opponent)
    {
        actions = new List<List<string>> { own, opponent };
    }

    public bool MoveNext()
    {
        currentActionIndex++;
        return currentActionIndex < actions.Count;
    }

    public List<string> Current
    {
        get
        {
            return actions[currentActionIndex];
        }
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public void Reset()
    {
        currentActionIndex = -1;
    }
}
