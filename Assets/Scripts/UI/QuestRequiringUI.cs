using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequiringUI : MonoBehaviour
{
    [SerializeField] private Text numberText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Toggle toggle;

    public void Fill(int id, string description, bool toggleMode) 
    {
        numberText.text = $"{id})";
        descriptionText.text = description;
        toggle.isOn = toggleMode;
    }
}
