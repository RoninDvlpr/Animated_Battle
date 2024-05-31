using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class AnimationController : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    protected Action onAnimationFinishedCallback;


    /// <summary>
    /// Plays random animation from the list of available animations.
    /// </summary>
    /// <returns>Name of a chosen animation.</returns>
    protected string PlayRandomAnimation(List<string> animationStateNames)
    {
        int randomIndex = Random.Range(0, animationStateNames.Count);
        animator.CrossFadeInFixedTime(animationStateNames[randomIndex], 0.2f);
        return animationStateNames[randomIndex];
    }

    public virtual void ReportAnimationFinish(AnimatorStateInfo stateInfo)
    {
        OnAnimiationFinish();
        onAnimationFinishedCallback?.Invoke();
    }

    protected virtual void OnAnimiationFinish()
    {
        //Debug.Log($"State finished: {name}");
    }
}
