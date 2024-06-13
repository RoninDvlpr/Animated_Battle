using System;

[Serializable]
public class Step
{
    public Hp hp;
    public Actions actions;

    public void PrintToConsole()
    {
        actions.PrintToConsole();
    }
}
