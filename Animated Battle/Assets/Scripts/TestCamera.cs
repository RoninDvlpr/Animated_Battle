using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LeapRight(1.7f));
    }

    IEnumerator LeapRight(float leapFrequency)
    {
        yield return new WaitForSeconds(1.2f);

        while (true)
        {
            transform.Translate(Vector3.right * 6.85f);
            yield return new WaitForSeconds(leapFrequency);
        }
    }
}
