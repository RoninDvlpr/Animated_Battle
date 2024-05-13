using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] Animator animator;
    float MovementSpeed { get; set; } = 2.5f;
    float FallBackSpeed { get; set; } = 3.5f;
    Coroutine currentMovementCoroutine;



    #region Controls

    public void CloseInOnOpponent(Fighter opponent, float distanceToStop, Action onOppenentReached)
    {
        if (currentMovementCoroutine != null)
            StopCoroutine(currentMovementCoroutine);
        currentMovementCoroutine = StartCoroutine(CloseInCoroutine(opponent, distanceToStop, onOppenentReached));
    }

    public void FallBack(float distanceToFallBack, Action onDistanceCovered)
    {
        if (currentMovementCoroutine != null)
            StopCoroutine(currentMovementCoroutine);
        currentMovementCoroutine = StartCoroutine(FallBackCoroutine(distanceToFallBack, onDistanceCovered));
    }

    public float StaggerBackwards()
    {
        animator.Play("Stagger Separate");
        FallBack(1.5f, null);
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }

    #endregion

    #region Movement Coroutines

    IEnumerator CloseInCoroutine(Fighter opponent, float distanceToStop, Action onOppenentReached)
    {
        while (CalculateDistance(opponent.transform.position, transform.position) > distanceToStop)
        {
            animator.SetBool("isWalking", true);
            Vector3 direction = Vector3.forward;
            transform.LookAt(opponent.transform);
            transform.Translate(direction * MovementSpeed * Time.deltaTime);
            yield return null;
        }

        yield return null;
        animator.SetBool("isWalking", false);
        onOppenentReached?.Invoke();
    }

    IEnumerator FallBackCoroutine(float distanceToFallBack, Action onDistanceCovered)
    {
        //animator.SetBool("isStaggering", true);
        Vector3 startingPosition = transform.position;
        while (CalculateDistance(startingPosition, transform.position) <= distanceToFallBack)
        {
            transform.Translate(Vector3.back * FallBackSpeed * Time.deltaTime);
            yield return null;
        }

        yield return null;
        //animator.SetBool("isStaggering", false);
        onDistanceCovered?.Invoke();
    }

    #endregion

    float CalculateDistance(Vector3 poisition1, Vector3 position2)
    {
        return (poisition1 - position2).magnitude;
    }
}
