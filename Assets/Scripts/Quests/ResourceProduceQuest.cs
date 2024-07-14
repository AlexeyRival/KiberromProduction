using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Quest/Produce Resource")] 
public class ResourceProduceQuest : Quest 
{
    public Resource resource;
    public int target;
    private int progress;
    public override void Check()
    {
        complete |= progress >= target;
    }
    public override void Progress(int amout)
    {
        progress += amout;
    }
}
