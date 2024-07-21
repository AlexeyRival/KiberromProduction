using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Manager[] managers;
    [SerializeField] private Transform UIRoot;
    //cashed
    private QuestManager questManager;
    private UIManager uiManager;
    private ResourceManager resourceManager;

    #region Singleton
    public static GameManager Instance;
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(UIRoot.gameObject);
    }
    #endregion
    private void Start()
    {
        questManager = GetManager<QuestManager>();
        uiManager = GetManager<UIManager>();
        resourceManager = GetManager<ResourceManager>();
    }
    public static T GetManager<T>() where T: Manager
    {
        for (int i = 0; i < Instance.managers.Length; ++i) 
        {
            if (Instance.managers[i] is T) return (T)Instance.managers[i];
        }
        throw new System.Exception("Manager Not Found!");
    }
    public static Transform GetUIRoot() 
    {
        return Instance.UIRoot;
    }
    public void ResourceProduced(Resource resource) 
    {
        questManager.ResourceProduced(resource);
        uiManager.DrawResources(resourceManager.GetResources());
    }
    public void DrawMoney(int money) 
    {
        uiManager.DrawMoney(money);
    }
    public Resource[] GetResourcesTypes() 
    {
        return resourceManager.GetResourcesTypes();
    }
}
