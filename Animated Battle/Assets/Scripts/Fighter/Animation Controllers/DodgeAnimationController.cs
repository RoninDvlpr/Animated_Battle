using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DodgeAnimationController : AnimationController
{
    [SerializeField] List<string> dodgeStateNames;


    /// <summary>
    /// Plays random dodge animation from the list of available animations.
    /// </summary>
    /// <returns>Name of a chosen animation.</returns>
    public void PlayRandomDodgeAnimation(Action onAnimationFinishedCallback)
    {
        this.onAnimationFinishedCallback = onAnimationFinishedCallback;
        PlayRandomAnimation(dodgeStateNames);
    }
}
