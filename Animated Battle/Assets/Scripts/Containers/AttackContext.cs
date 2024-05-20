using System;

public struct AttackContext
{
    public readonly Fighter attackTarget;
    public readonly AttackTypes attackType;
    public readonly Action onAttackFinishedCallback;

    public AttackContext(Fighter attackTarget, AttackTypes attackType, Action onAttackFinishedCallback)
    {
        this.attackTarget = attackTarget;
        this.attackType = attackType;
        this.onAttackFinishedCallback = onAttackFinishedCallback;
    }
}
