using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DodgeAnimationController : DefenseAnimationController
{
    [SerializeField] List<string> dodgeStateNames;


    protected override void OnOpponentAttackStart()
    {
        PlayRandomDodgeAnimation();
    }

    /// <summary>
    /// Plays random dodge animation from the list of available animations.
    /// </summary>
    void PlayRandomDodgeAnimation()
    {
        Debug.Log(gameObject.name + " dodges");
        PlayRandomAnimation(dodgeStateNames);
        //to increase dodge travell distance we can use: movementController.FallBack(1.5f, null);
    }

    public override void ReactToBeingHit()
    {
        
    }
}
