using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    //settings
    MovementController movementController;
    [SerializeField] AnimationController attackAnimController, blockAnimController, dodgeAnimController;
    float AttackDistance { get; set; } = 1.25f;

    //state
    public bool IsBusy { get; private set; } = false;
    public event Action onBecomingFree;

    Fighter opponentToAttack;
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

    public void AttackOpponent(Fighter targetFighter, Action onExecutedCallback)
    {
        IsBusy = true;
        opponentToAttack = targetFighter;
        onAttackFinished = onExecutedCallback;
        movementController.CloseInOnOpponent(targetFighter, AttackDistance, PerformAttack);
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

    void PerformAttack()
    {
        PlayAttackAnimation(OnOwnAttackLanded);
        opponentToAttack.OnOpponentAttackStarted();
    }

    void OnOwnAttackLanded()
    {
        opponentToAttack.OnHit();
    }

    public void OnOpponentAttackStarted()
    {
        if (nextDefenceAnimation != null)
            nextDefenceAnimation.Invoke();
    }

    public void OnHit()
    {
        if (nextDefenceAnimation != null)
            nextDefenceAnimation = null;
        else
            PlayStaggerAnimation();
    }

    #endregion


    #region Animations

    /// <summary>
    /// Plays a random attack animation
    /// </summary>
    /// <returns>Duration of a chosen animation</returns>
    float PlayAttackAnimation(Action onHitCallback)
    {
        Debug.Log(gameObject.name + " attacks");
        float animationDuration = attackAnimController.PlayRandomAnimation(onHitCallback);
        Invoke("OnAttackFinished", animationDuration + 0.05f);
        return animationDuration;
    }

    void PlayBlockAnimation()
    {
        Debug.Log(gameObject.name + " blocks");
        IsBusy = true;
        float animationDuration = blockAnimController.PlayRandomAnimation();
        Invoke("MarkAsFree", animationDuration + 0.05f);
    }

    void PlayDodgeAnimation()
    {
        Debug.Log(gameObject.name + " dodges");
        IsBusy = true;
        //movementController.FallBack(1.5f, null);
        float animationDuration = dodgeAnimController.PlayRandomAnimation();
        Invoke("MarkAsFree", animationDuration + 0.05f);
    }

    void PlayStaggerAnimation()
    {
        Debug.Log(gameObject.name + " staggers");
        IsBusy = true;
        //movementController.FallBack(1.5f, MarkAsFree);
        float animationDuration = movementController.StaggerBackwards();
        Invoke("MarkAsFree", animationDuration + 0.05f);
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
