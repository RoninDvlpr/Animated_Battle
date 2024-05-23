using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtController : MonoBehaviour
{
    [SerializeField] Transform objectToLookAt;
    [SerializeField] float headWeight;
    [SerializeField] float bodyWeight;
    [SerializeField] Animator animator;


    void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtPosition(objectToLookAt.transform.position);
        animator.SetLookAtWeight(1, bodyWeight, headWeight);
    }
}
