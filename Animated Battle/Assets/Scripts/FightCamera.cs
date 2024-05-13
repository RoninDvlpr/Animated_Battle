using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCamera : MonoBehaviour
{
    [SerializeField] Transform player, opponent;
    Vector3 cameraOffset;
    float smoothTime = 0.125f;
    Vector3 currentVelocity = Vector3.one;


    void Awake()
    {
        CalculateCameraOffset();
    }

    void CalculateCameraOffset()
    {
        Vector3 positionToFollow = GetMiddlePoint();
        cameraOffset = transform.position - positionToFollow;
    }

    void LateUpdate()
    {
        SmoothFollowMiddlePoint();
    }

    void SmoothFollowMiddlePoint()
    {
        SmoothFollowPoint(GetMiddlePoint);
    }

    void SmoothFollowPlayer()
    {
        SmoothFollowPoint(GetPlayerPosition);
    }

    void SmoothFollowPoint(Func<Vector3> pointToFollowGetter)
    {
        Vector3 positionToFollow = pointToFollowGetter();
        Vector3 targetCameraPosition = positionToFollow + cameraOffset;
        Vector3 updatedCameraPosition = Vector3.SmoothDamp(transform.position, targetCameraPosition, ref currentVelocity, smoothTime);
        transform.position = updatedCameraPosition;
    }

    Vector3 GetMiddlePoint()
    {
        return Vector3.Lerp(player.position, opponent.position, 0.5f);
    }

    Vector3 GetPlayerPosition()
    {
        return player.position;
    }
}
