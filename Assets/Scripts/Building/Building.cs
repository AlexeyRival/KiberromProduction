using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    //Base class
    [SerializeField] protected BaseWindow UIPrefab;
    public virtual void ShowUI() { }
}
