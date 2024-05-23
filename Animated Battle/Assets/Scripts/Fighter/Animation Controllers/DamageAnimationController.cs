using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;


public class DamageAnimationController : DefenseAnimationController
{
    [SerializeField] string staggerAnimationName = "Stagger Backwards";


    protected override void OnOpponentAttackStart()
    {

    }

    /// <summary>
    /// Plays damage animation.
    /// </summary>
    public override void ReactToBeingHit()
    {
        StaggerBackwards(attackType, movementController);
    }

    void StaggerBackwards(AttackTypes attackType, MovementController movementController)
    {
        Debug.Log(gameObject.name + " staggers");

        float fallBackDistance;
        if (attackType == AttackTypes.Crit)
            fallBackDistance = Random.Range(0.75f, 1.25f);
        else
            fallBackDistance = Random.Range(0f, 0.5f);
        Debug.Log($"{attackType} attack, fallback distance is {fallBackDistance}");

        animator.CrossFadeInFixedTime(staggerAnimationName, 0.2f);
        movementController.FallBack(fallBackDistance, null);
    }

    public override void ReportAnimationFinish(AnimatorStateInfo stateInfo)
    {
        //Debug.Log($"Damage state finished: {name}");
        movementController.StopCurrentMovement();
        onAnimationFinishedCallback?.Invoke();
    }
}
