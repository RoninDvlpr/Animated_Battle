using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimationController : MonoBehaviour
{
    [SerializeField] List<string> stateNames;
    Animator animator;
    Action animationCallback;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Plays random animation from the list of available animations.
    /// </summary>
    /// <returns>Duration of a chosen animation</returns>
    public float PlayRandomAnimation(Action animationCallback = null)
    {
        this.animationCallback = animationCallback;
        int randomIndex = Random.Range(0, stateNames.Count);
        animator.Play(stateNames[randomIndex]);
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public void AnimationCallback()
    {
        animationCallback?.Invoke();
    }
}
