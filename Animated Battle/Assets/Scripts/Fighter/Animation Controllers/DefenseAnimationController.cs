using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefenseAnimationController : AnimationController
{
    protected MovementController movementController;
    protected AttackTypes attackType;

    /// <summary>
    /// Plays damage animation.
    /// </summary>
    public void ReactToOpponentAttackStart(AttackTypes attackType, MovementController movementController, Action onAnimationFinishedCallback)
    {
        this.attackType = attackType;
        this.movementController = movementController;
        this.onAnimationFinishedCallback = onAnimationFinishedCallback;
        OnOpponentAttackStart();
    }

    protected abstract void OnOpponentAttackStart();

    public abstract void ReactToBeingHit();
}
