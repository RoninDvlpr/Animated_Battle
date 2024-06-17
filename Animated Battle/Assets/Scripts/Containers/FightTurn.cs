using UnityEngine;

public class FightTurn
{
    public readonly Fighters attacker;
    public readonly AttackTypes attackType;
    public readonly DefenseTypes defenseType;

    public FightTurn(Fighters attacker, AttackTypes attackType, DefenseTypes defenseType)
    {
        this.attacker = attacker;
        this.attackType = attackType;
        this.defenseType = defenseType;
    }

    public static FightTurn GenerateRandomFightTurn()
    {
        Fighters attacker = (Fighters) Random.Range(0, 2);
        return GenerateRandomFightTurn(attacker);
    }

    public static FightTurn GenerateRandomFightTurn(Fighters attacker)
    {
        AttackTypes attackType = (AttackTypes)Random.Range(0, 2);
        DefenseTypes defenseType = (DefenseTypes)Random.Range(0, 3);
        return new FightTurn(attacker, attackType, defenseType);
    }

    public void PrintToConsole()
    {
        Debug.Log($"{attacker} performs {attackType} attack, his opponent reaction is {defenseType}");
    }
}
