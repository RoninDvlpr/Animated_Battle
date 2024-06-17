using System;
using System.Collections.Generic;

[Serializable]
public class Step
{
    public Hp hp;
    public Actions actions;
    public List<FightTurn> FightTurns
    {
        get
        {
            return actions.FightTurns;
        }
    }

    public void PrintToConsole()
    {
        actions.PrintToConsole();
    }
}
