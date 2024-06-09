using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using Random = UnityEngine.Random;


public class Fighter : MonoBehaviour
{
    //refs
    [SerializeField] MovementController movementController;
    [SerializeField] AttackAnimationController attackAnimController;
    [SerializeField] BlockAnimationController blockAnimController;
    [SerializeField] DodgeAnimationController dodgeAnimController;
    [SerializeField] DamageAnimationController damageAnimController;
    [SerializeField] Weapon weapon;

    //settings
    float AttackDistance { get; set; } = 1.25f;

    //state
    public bool IsBusy { get; private set; } = false;
    public event Action onBecomingFree;
    DefenseAnimationController currentDefenseAnimationController;
    Action onAttackFinished;


    private void Awake()
    {
        currentDefenseAnimationController = damageAnimController;
    }

    #region Controlls

    public void MoveForward(float distanceToTravel, Action onDistancetraveled = null)
    {
        movementController.MoveForward(distanceToTravel, onDistancetraveled);
    }

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
        currentDefenseAnimationController = blockAnimController;
    }

    public void DodgeNextAttack()
    {
        currentDefenseAnimationController = dodgeAnimController;
    }

    #endregion


    #region Actions

        #region Attack
        void PerformAttack(AttackContext attackContext)
        {
            attackAnimController.PlayRandomAttackAnimation(
                attackContext,
                weapon,
                () => OnOwnAttackLanded(attackContext),
                OnAttackFinished
            );

            attackContext.attackTarget.OnOpponentAttackStarted(attackContext.attackType); //inform opponent that my attack has started
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
        public void OnOpponentAttackStarted(AttackTypes attackType)
        {
            IsBusy = true;
            currentDefenseAnimationController.ReactToOpponentAttackStart(attackType, movementController, MarkAsFree);
        }

        /// <summary>
        /// Used to react to being hit.
        /// </summary>
        public void OnHit(AttackTypes attackType)
        {
            currentDefenseAnimationController.ReactToBeingHit();
            currentDefenseAnimationController = damageAnimController;
        }
        #endregion

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
