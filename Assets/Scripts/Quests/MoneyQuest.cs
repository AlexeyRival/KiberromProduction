using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Custom/Quest/Money")]
public class MoneyQuest : Quest 
{
    [Header("Квест денежный")]
    public int target;
    public override void Check()
    {
        complete |= GameManager.GetManager<ResourceManager>().IsEnoughtMoney(target);
    }
}
