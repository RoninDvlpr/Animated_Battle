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
        if (distanceToTravel > 0)
        {
            Vector3 startingPosition = transform.position;
            animator.SetBool("isWalking", true);    //currently only forward motion animation is used

            yield return null; //it takes 1 frame for a tranistion to start

            // movement during transition to animation
            float currentSpeed = 0f;
            while (animator.IsInTransition(0) && CalculateDistance(startingPosition, transform.position) < distanceToTravel)
            {
                currentSpeed = CalculateInitialAcceleration(movementSpeed);
                MoveForward(currentSpeed);
                yield return null;
            }

            // movement when fully transitioned to animation
            while (CalculateDistance(startingPosition, transform.position) < distanceToTravel)
            {
                MoveForward(movementSpeed);
                yield return null;
            }

            animator.SetBool("isWalking", false);
        }
        else
            Debug.LogWarning("You set invalid distance to travel parameter: 'distanceToTravel' should be greater than 0.");

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

            yield return null; //it takes 1 frame for transition to start

            // movement during transition to animation
            float currentSpeed = 0f;
            while (animator.IsInTransition(0) && CalculateDistance(opponent.transform.position, transform.position) > distanceToReach)
            {
                currentSpeed = CalculateInitialAcceleration(movementSpeed);
                MoveToOpponent(currentSpeed, opponent);
                yield return null;
            }

            // movement when fully transitioned to animation
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

    /// <summary>
    /// Returns gradually incrementing value of the movement speed.
    /// Used to prevent fighter's "slipping" at the start of the movement.
    /// </summary>
    /// <param name="maxSpeed">The standard movement speed that's used when transition to the movement animation was completed</param>
    /// <returns>A movement speed that correspond's to the current transition's frame</returns>
    float CalculateInitialAcceleration(float maxSpeed)
    {
        if (animator.IsInTransition(0))
            return Mathf.Lerp(0f, maxSpeed, animator.GetAnimatorTransitionInfo(0).normalizedTime);
        return maxSpeed;
    }

    #endregion

    float CalculateDistance(Vector3 poisition1, Vector3 position2)
    {
        return (poisition1 - position2).magnitude;
    }
}
