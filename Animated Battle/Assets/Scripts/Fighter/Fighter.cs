using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;


public class Fighter : MonoBehaviour
{
    //settings
    MovementController movementController;
    [SerializeField] AttackAnimationController attackAnimController;
    [SerializeField] BlockAnimationController blockAnimController;
    [SerializeField] DodgeAnimationController dodgeAnimController;
    [SerializeField] Weapon weapon;
    float AttackDistance { get; set; } = 1.25f;

    //state
    public bool IsBusy { get; private set; } = false;
    public event Action onBecomingFree;

    Action onAttackFinished;

    Action nextDefenceAnimation;


    private void Awake()
    {
        movementController = GetComponent<MovementController>();
    }

    #region Controlls

    public void CloseInOnOpponent(Fighter opponent)
    {
        movementController.CloseInOnOpponent(opponent, AttackDistance, null);
    }

    public void AttackOpponent(AttackContext attackContext)
    {
        //Debug.Log($"{name} received command to attack");
        IsBusy = true;
        onAttackFinished = attackContext.onAttackFinishedCallback;
        movementController.CloseInOnOpponent(
            attackContext.attackTarget,
            AttackDistance,
            () => PerformAttack(attackContext)
        );
    }

    public void BlockNextAttack()
    {
        nextDefenceAnimation = PlayBlockAnimation;
    }

    public void DodgeNextAttack()
    {
        nextDefenceAnimation = PlayDodgeAnimation;
    }

    #endregion


    #region Actions

        #region Attack
        void PerformAttack(AttackContext attackContext)
        {
            PlayAttackAnimation(attackContext);
            attackContext.attackTarget.OnOpponentAttackStarted(); //inform opponent that my attack has started
        }

        /// <summary>
        /// Called when attack hits an opponent.
        /// Used in event-based hit detection to inform an opponent that my attack has landed.
        /// </summary>
        void OnOwnAttackLanded(AttackContext attackContext)
        {
            attackContext.attackTarget.OnHit(attackContext.attackType);
        }
        #endregion


        #region Defense
        /// <summary>
        /// Used to react to an opponent attack.
        /// </summary>
        public void OnOpponentAttackStarted()
        {
            if (nextDefenceAnimation != null)
                nextDefenceAnimation.Invoke();
        }

        /// <summary>
        /// Used to react to being hit.
        /// </summary>
        public void OnHit(AttackTypes attackType)
        {
            if (nextDefenceAnimation != null)
                nextDefenceAnimation = null;
            else
                PlayStaggerAnimation(attackType);
        }
        #endregion

    #endregion


    #region Animations

    /// <summary>
    /// Plays a random attack animation
    /// </summary>
    /// <returns>Name of a chosen animation</returns>
    void PlayAttackAnimation(AttackContext attackContext)
    {
        Debug.Log(gameObject.name + " attacks");
        attackAnimController.PlayRandomAttackAnimation(
            attackContext,
            weapon,
            () => OnOwnAttackLanded(attackContext),
            OnAttackFinished
        );
    }

    void PlayBlockAnimation()
    {
        Debug.Log(gameObject.name + " blocks");
        IsBusy = true;
        blockAnimController.PlayRandomBlockAnimation(MarkAsFree);
    }

    void PlayDodgeAnimation()
    {
        Debug.Log(gameObject.name + " dodges");
        IsBusy = true;
        //to increase dodge travell distance we can use: movementController.FallBack(1.5f, null);
        dodgeAnimController.PlayRandomDodgeAnimation(MarkAsFree);
    }

    void PlayStaggerAnimation(AttackTypes attackType)
    {
        Debug.Log(gameObject.name + " staggers");
        IsBusy = true;

        float fallBackDistance;
        if (attackType == AttackTypes.Crit)
            fallBackDistance = Random.Range(1f, 1.5f);
        else
            fallBackDistance = Random.Range(0f, 0.5f);
        Debug.Log($"{attackType} attack, fallback distance is {fallBackDistance}");

        movementController.StaggerBackwards(fallBackDistance, MarkAsFree);
    }

    #endregion

    #region Action Finishers

    void MarkAsFree()
    {
        IsBusy = false;
        onBecomingFree?.Invoke();
    }

    void OnAttackFinished()
    {
        MarkAsFree();
        onAttackFinished?.Invoke();
    }

    #endregion

}
