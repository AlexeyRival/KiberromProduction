using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ResourceManager : Manager
{
    [SerializeField] private Resource[] resources;
    [SerializeField] private Dictionary<Resource, int> resourcesCount;
    public int ResourcesCount { get { return resources.Length; } }
    public int money = 100;

    private UIManager uiManager;

    private void Start()
    {
        resourcesCount = new Dictionary<Resource, int>();
        for (int i = 0; i < resources.Length; ++i) 
        {
            resourcesCount.Add(resources[i], 0);
        }
        uiManager =GameManager.GetManager<UIManager>();
        uiManager.DrawResources(resourcesCount);
        uiManager.DrawMoney(money);
    }

    public void SellResource(Resource res,int price,int count) 
    {
        DeltaMoney(price * count);
        DeltaResource(res, -count);
    }
    public Resource GetResourceById(int id) { return resources[id]; }

    public void DeltaMoney(int delta) 
    { 
        money += delta; 
        GameManager.Instance.DrawMoney(money);
    }
    public bool IsEnoughtMoney(int required) 
    {
        return money >= required;
    }
    public void DeltaResource(Resource resource, int delta) 
    {
        resourcesCount[resource] += delta;
        if (delta > 0) GameManager.Instance.ResourceProduced(resource);
    }
    public void DeltaResource(int id, int delta) { DeltaResource(resources[id], delta); }

    public Dictionary<Resource, int> GetResources() { return resourcesCount; }
    public Resource[] GetResourcesTypes() { return resources; }
    public bool IsEnoughtResource(Resource resource, int count) 
    {
        return resourcesCount[resource] >= count;
    }

    public string Save() 
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < resources.Length; ++i) 
        {
            sb.Append(resourcesCount[resources[i]]);
            sb.Append(':');
        }
        sb.Append(money);
        return sb.ToString();
    }
    public void Load(string data) 
    {
        string[] spt = data.Split(':');
        for (int i = 0; i < resources.Length; ++i) 
        {
            resourcesCount[resources[i]] = int.Parse(spt[i]);
        }
        money = int.Parse(spt[spt.Length - 1]);
        DeltaMoney(0);
    }
}
