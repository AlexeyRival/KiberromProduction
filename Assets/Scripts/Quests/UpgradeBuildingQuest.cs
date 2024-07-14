using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Quest/Upgrade Factory")] 
public class UpgradeBuildingQuest : Quest 
{
    public string targetFactoryName;
    public int target;
    public override void Check()
    {
        bool compl = false;
        Factory[] factories = FindObjectsOfType<Factory>();
        for (int i = 0; i < factories.Length; ++i) 
        {
            if (factories[i].factoryName == targetFactoryName)
            {
                compl = factories[i].level >= target;
                break;
            }
        }
        complete |= compl;
    }
}
