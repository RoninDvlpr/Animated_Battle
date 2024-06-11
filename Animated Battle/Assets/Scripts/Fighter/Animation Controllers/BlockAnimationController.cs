using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BlockAnimationController : DefenseAnimationController
{
    [SerializeField] GameObject blockParticles;
    [SerializeField] List<string> blockStateNames;

    
    protected override void OnOpponentAttackStart()
    {
        PlayRandomBlockAnimation();
    }

    /// <summary>
    /// Plays random dodge animation from the list of available animations.
    /// </summary>
    public void PlayRandomBlockAnimation()
    {
        //Debug.Log(gameObject.name + " blocks");
        PlayRandomAnimation(blockStateNames);
    }

    public override void ReactToBeingHit()
    {
        animator.SetTrigger("getHit");
        blockParticles?.SetActive(true);
    }
}
