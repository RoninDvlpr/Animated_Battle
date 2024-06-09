using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[DisallowMultipleComponent]
public class MovementController : MonoBehaviour
{
    [SerializeField] Animator animator;
    float RunSpeed { get; set; } = 4f;
    float FallBackSpeed { get; set; } = 4f;
    Coroutine currentMovementCoroutine;
    bool TransitionInProgres
    {
        get
        {
            return animator.GetAnimatorTransitionInfo(0).normalizedTime != 0f;
        }
    }



    #region Controls

    public void CloseInOnOpponent(Fighter opponent, float distanceToStop, Action onOppenentReached)
    {
        StopCurrentMovement();
        currentMovementCoroutine = StartCoroutine(CloseInCoroutine(opponent, distanceToStop, RunSpeed, onOppenentReached));
    }

    public void MoveForward(float distanceToTravel, Action onDistanceTraveled)
    {
        StopCurrentMovement();
        currentMovementCoroutine = StartCoroutine(ForwardMovementCoroutine(distanceToTravel, RunSpeed, onDistanceTraveled));
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

    void MoveToOpponent(float movementSpeed, Fighter opponent)
    {
        transform.LookAt(opponent.transform);
        MoveForward(movementSpeed);
    }

    void MoveForward(float movementSpeed)
    {
        Vector3 movementDirection = Vector3.forward;
        transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Move forward until the specified distance from a starting position is traveled.
    /// </summary>
    /// <param name="onDistanceTraveled">On movement finished callback.</param>
    IEnumerator ForwardMovementCoroutine(float distanceToTravel, float movementSpeed, Action onDistanceTraveled)
    {
        animator.SetBool("isWalking", true);    //currently only forward motion animation is used

        Debug.Log(animator.GetAnimatorTransitionInfo(0).duration);
        yield return null;
        Debug.Log(animator.GetAnimatorTransitionInfo(0).duration);
        yield return null;  //it takes a few frames for a tranistion to start and it's normalized time to become greater than 0
        while (TransitionInProgres)
        {
            Debug.Log("Awaiting transition");
            Debug.Log(animator.GetAnimatorTransitionInfo(0).duration);
            yield return null;
        }


        Vector3 startingPosition = transform.position;
        while (CalculateDistance(startingPosition, transform.position) < distanceToTravel)
        {
            MoveForward(movementSpeed);
            yield return null;
        }

        animator.SetBool("isWalking", false);
        onDistanceTraveled?.Invoke();
    }

    /// <summary>
    /// Close in with the opponent until a specified distance to opponent is reached.
    /// </summary>
    /// <param name="distanceToReach">What distance from opponent shall we stop.</param>
    /// <param name="onOpponentReached">On movement finished callback.</param>
    IEnumerator CloseInCoroutine(Fighter opponent, float distanceToReach, float movementSpeed, Action onOpponentReached)
    {
        if (CalculateDistance(opponent.transform.position, transform.position) > distanceToReach)
        {
            animator.SetBool("isWalking", true);    //currently only forward motion animation is used
            
            Debug.Log(animator.GetAnimatorTransitionInfo(0).duration);
            yield return null;
            Debug.Log(animator.GetAnimatorTransitionInfo(0).duration);
            yield return null;  //it takes a few frames for a tranistion to start and it's normalized time to become greater than 0
            while (TransitionInProgres)
            {
                Debug.Log("Awaiting transition");
                Debug.Log(animator.GetAnimatorTransitionInfo(0).duration);
                yield return null;
            }


            while (CalculateDistance(opponent.transform.position, transform.position) > distanceToReach)
            {
                MoveToOpponent(movementSpeed, opponent);
                yield return null;
            }

            animator.SetBool("isWalking", false);
        }

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
