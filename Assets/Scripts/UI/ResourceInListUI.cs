using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceInListUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text count;

    public void Fill(Resource resource, int count) 
    {
        image.sprite = resource.icon;
        image.color= resource.Color;
        this.count.text= count.ToString();
    }
}
