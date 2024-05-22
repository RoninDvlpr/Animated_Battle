using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AttackAnimationController : AnimationController
{
    [SerializeField] List<string> slashAttackStateNames, stabAttackStateNames;
    Action onAttackLandedCallback;


    /// <summary>
    /// Plays random attack animation from the list of available animations.
    /// </summary>
    /// <returns>Name of a chosen animation.</returns>
    public void PlayRandomAttackAnimation(AttackContext context, Weapon characterWeapon, Action onAttackLandedCallback, Action onAnimationFinishedCallback)
    {
        this.onAttackLandedCallback = onAttackLandedCallback;
        this.onAnimationFinishedCallback = onAnimationFinishedCallback;

        List<string> validStates = new List<string>();
        if (characterWeapon.CanSlash)
            validStates.AddRange(slashAttackStateNames);
        if (characterWeapon.CanStab)
            validStates.AddRange(stabAttackStateNames);

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

    public void OnAttackLandedCallback()
    {
        onAttackLandedCallback?.Invoke();
    }
}
