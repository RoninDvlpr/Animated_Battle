using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AttackAnimationController : AnimationController
{
    [SerializeField] LookAtController lookAtController;
    [SerializeField] string idleStateName = "Idle";
    [SerializeField] List<string> slashAttackStateNames, stabAttackStateNames;
    Action onAttackLandedCallback;


    /// <summary>
    /// Plays random attack animation from the list of available animations.
    /// </summary>
    public void PlayRandomAttackAnimation(AttackContext context, Weapon characterWeapon, Action onAttackLandedCallback, Action onAnimationFinishedCallback)
    {
        OnAttackStart();

        this.onAttackLandedCallback = onAttackLandedCallback;
        this.onAnimationFinishedCallback = onAnimationFinishedCallback;

        List<string> validStates = new List<string>();
        if (characterWeapon.CanSlash)
            validStates.AddRange(slashAttackStateNames);
        if (characterWeapon.CanStab)
            validStates.AddRange(stabAttackStateNames);

        Debug.Log(gameObject.name + " attacks");
        if (validStates.Count == 0)
            if (!characterWeapon.CanSlash && !characterWeapon.CanStab)
            {
                Debug.LogError($"The weapon {characterWeapon.name} can't either 'slash' or 'stab'. Probably it's set up incorrectly.");
                PlayRandomAnimation(slashAttackStateNames);
            }
            else
                Debug.LogError("There's no states specified for this weapon's attack types.");

        PlayRandomAnimation(validStates);
    }

    void InterruptAttack()
    {
        animator.CrossFadeInFixedTime(idleStateName, 0.4f);
    }

    void OnAttackStart()
    {
        lookAtController?.SetWeights(0.1f, 0f);
    }

    public void OnAttackLandedCallback()
    {
        lookAtController?.ResetWeights();
        onAttackLandedCallback?.Invoke();
        InterruptAttack();
    }
}
