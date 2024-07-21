using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellUI : MonoBehaviour
{
    [SerializeField] private Image resourceIcon;
    [SerializeField] private Text resourceText;
    [SerializeField] private Text priceText;
    [SerializeField] private Button sellButton;

    public System.Action onSell;

    public void Fill(Resource resource, int price, bool canSell) 
    {
        resourceIcon.sprite = resource.icon;
        resourceIcon.color = resource.Color;
        resourceText.text = $"{resource.name} (10 шт)";
        priceText.text = price.ToString();
        sellButton.interactable = canSell;
        sellButton.onClick.AddListener(OnSell);
    }
    private void OnSell() 
    {
        onSell?.Invoke();
    }
    private void OnDestroy()
    {
        sellButton.onClick.RemoveAllListeners();
    }
}
