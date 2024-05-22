using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BlockAnimationController : AnimationController
{
    [SerializeField] List<string> blockStateNames;


    /// <summary>
    /// Plays random dodge animation from the list of available animations.
    /// </summary>
    /// <returns>Name of a chosen animation.</returns>
    public void PlayRandomBlockAnimation(Action onAnimationFinishedCallback)
    {
        this.onAnimationFinishedCallback = onAnimationFinishedCallback;
        PlayRandomAnimation(blockStateNames);
    }
}
