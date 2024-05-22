using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] bool canSlash, canStab;
    public bool CanSlash { get { return canSlash; } }
    public bool CanStab { get {  return canStab; } }

    [SerializeField] float reach;
    public float Reach { get { return reach; } }
}
