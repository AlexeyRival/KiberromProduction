using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : ScriptableObject
{
    public string text;
    public bool complete;
    public virtual void Check() { }
    public virtual void Progress(int amout) { }
}
