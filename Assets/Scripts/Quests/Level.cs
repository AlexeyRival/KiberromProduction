using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Level")]
public class Level : ScriptableObject
{
    [SerializeField] private Quest[] Quests;
    private Quest[] questClones;
    public Quest[] GetQuests() { return questClones; }
    public void Start() 
    {
        questClones = new Quest[Quests.Length];
        for (int i = 0; i < questClones.Length; ++i) 
        {
            questClones[i] = Instantiate(Quests[i]);
        }
    }
}
