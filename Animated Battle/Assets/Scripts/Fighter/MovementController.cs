using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MovementController : MonoBehaviour
{
    [SerializeField] Animator animator;
    float RunSpeed { get; set; } = 4f;
    float FallBackSpeed { get; set; } = 4f;
    Coroutine currentMovementCoroutine;



    #region Controls

    public void CloseInOnOpponent(Fighter opponent, float distanceToStop, Action onOppenentReached)
    {
        StopCurrentMovement();
        currentMovementCoroutine = StartCoroutine(CloseInCoroutine(opponent, distanceToStop, RunSpeed, onOppenentReached));
    }

    public void FallBack(float distanceToFallBack, Action onDistanceCovered)
    {
        StopCurrentMovement();
        currentMovementCoroutine = StartCoroutine(FallBackCoroutine(distanceToFallBack, FallBackSpeed, onDistanceCovered));
    }

    public void StopCurrentMovement()
    {
        if (currentMovementCoroutine != null)
            StopCoroutine(currentMovementCoroutine);
        animator.SetBool("isWalking", false);
    }

    #endregion

    #region Movement

    /// <summary>
    /// Close in with the opponent until a specified distance to opponent is reached.
    /// </summary>
    /// <param name="distanceToReach">What distance from opponent shall we stop.</param>
    /// <param name="onOpponentReached">On movement finished callback.</param>
    IEnumerator CloseInCoroutine(Fighter opponent, float distanceToReach, float movementSpeed, Action onOpponentReached)
    {
        while (CalculateDistance(opponent.transform.position, transform.position) > distanceToReach)
        {
            animator.SetBool("isWalking", true);    //currently only forward motion animation is used
            transform.LookAt(opponent.transform);
            Vector3 movementDirection = Vector3.forward;
            transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
            yield return null;
        }

        animator.SetBool("isWalking", false);
        onOpponentReached?.Invoke();
    }

    /// <summary>
    /// Fall back the specified distance. The fallback direction is straight back from the direction the character is facing.
    /// </summary>
    /// <param name="onDistanceCovered">On movement finished callback.</param>
    IEnumerator FallBackCoroutine(float distanceToFallBack, float movementSpeed, Action onDistanceCovered)
    {
        float traveledDistance = 0f;
        while (traveledDistance < distanceToFallBack)
        {
            Vector3 translationVector = Vector3.back * movementSpeed * Time.deltaTime;
            transform.Translate(translationVector);
            traveledDistance += translationVector.magnitude;
            yield return null;
        }

        //currently only forward motion animation is used
        onDistanceCovered?.Invoke();
    }

    #endregion

    float CalculateDistance(Vector3 poisition1, Vector3 position2)
    {
        return (poisition1 - position2).magnitude;
    }
}
