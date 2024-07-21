using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : Manager
{
    [Header("UI")]
    [SerializeField] private Text moneyText;
    [SerializeField] private Transform ResourcesListRoot;
    
    [Header("Prefabs")]
    [SerializeField] private ResourceInListUI ResourceInList;
    [SerializeField] private GameObject droppedText;

    private int prevMoney;

    public void DrawResources(Dictionary<Resource,int> resources) 
    {
        ResourcesListRoot.ClearRoot();
        foreach (var resource in resources) 
        {
            if (resource.Value > 0) 
            {
                ResourceInListUI ob = Instantiate(ResourceInList, ResourcesListRoot);
                ob.Fill(resource.Key, resource.Value);
            }
        }
    }
    public void DrawMoney(int money) 
    {
        moneyText.text = money.ToString();
        if (prevMoney != money) 
        {
            GameObject ob = Instantiate(droppedText, moneyText.transform.position, Quaternion.identity,moneyText.transform);
            ob.GetComponent<Text>().text = (money - prevMoney).ToString();
        }
        prevMoney = money;
    }

  
}
