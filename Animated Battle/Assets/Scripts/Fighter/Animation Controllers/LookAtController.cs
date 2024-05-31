using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform objectToLookAt;

    [SerializeField] float defaultHeadWeight = 1f, defaultBodyWeight = 0f;
    public float HeadWeight { get; private set; }
    public float BodyWeight { get; private set; }

    Coroutine weightsUpdateCoroutine;


    private void Awake()
    {
        ResetWeights();
    }

    void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtPosition(objectToLookAt.transform.position);
        animator.SetLookAtWeight(1, BodyWeight, HeadWeight);
    }

    public void SetWeights(float newHeadWeight, float newBodyWeight, float transitionTime = 0.35f)
    {
        if (weightsUpdateCoroutine != null)
            StopCoroutine(weightsUpdateCoroutine);
        StartCoroutine(WeightsSmoothUpdate(newHeadWeight, newBodyWeight, transitionTime));
    }

    IEnumerator WeightsSmoothUpdate(float targetHeadWeight, float targetBodyWeight, float smoothingTime = 0.35f)
    {
        float startingHeadWeight = HeadWeight;
        float startingBodyWeight = BodyWeight;
        float transitionStartTime = Time.time;

        while (HeadWeight != targetHeadWeight || BodyWeight != targetBodyWeight)
        {
            float timeSinceTransitionStart = Time.time - transitionStartTime;
            float progress = timeSinceTransitionStart / smoothingTime;
            HeadWeight = Mathf.Lerp(startingHeadWeight, targetHeadWeight, progress);
            BodyWeight = Mathf.Lerp(startingBodyWeight, targetBodyWeight, progress);
            yield return null;
        }
    }

    public void ResetWeights(float transitionTime = 0.35f)
    {
        if (weightsUpdateCoroutine != null)
            StopCoroutine(weightsUpdateCoroutine);
        StartCoroutine(WeightsSmoothUpdate(defaultHeadWeight, defaultBodyWeight, transitionTime));
    }
}
