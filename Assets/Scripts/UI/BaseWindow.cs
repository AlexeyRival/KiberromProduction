using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWindow : MonoBehaviour
{
    public virtual void Show() { }
    public virtual void Draw() { }
    public virtual void Hide() { }
}
