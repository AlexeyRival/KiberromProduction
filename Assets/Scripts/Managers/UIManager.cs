using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button QuestButton;

    [Space]
    [Header("Windows")]
    [SerializeField] private GameObject FactoryUI;
    [SerializeField] private GameObject MarketUI;
    [SerializeField] private GameObject QuestUI;

    [Space]
    [Header("Resources and money")]
    [SerializeField] private GameObject MoneyUI;
    [SerializeField] private Transform ResourcesListRoot;
    
    [Space]
    [Header("Prefabs")]
    [SerializeField] private GameObject ResourceInList;
    [SerializeField] private GameObject ResourceInMarket;
    [SerializeField] private GameObject ResourceInRecipe;
    [Space]
    [SerializeField] private GameObject RecipeInSelector;
    [SerializeField] private GameObject QuestInList;
    [SerializeField] private GameObject droppedText;
    [SerializeField] private GameObject droppedStar;

    #region Cashed
    //other
    private int prevMoney;
    private Text moneyText;

    //Factory Window
    private Button factoryBackButton;
    private Text factoryName;
    private Image factoryCurrentResource;
    private Text factoryCurrentProductivity;
    private Transform factoryRecipeSelector;
    private Transform factoryCurrentState;
    private GameObject factoryUpgradeArrow;
    private Transform factoryUpgradeState;

    //Market Window
    private Button marketBackButton;
    private Image marketHypedResource;
    private Transform marketSellList;

    //Quest Window
    private Button questBackButton;
    private Transform questList;
    private Text questRequiredStars;
    private Button nextLevelButton;
    #endregion


    #region Singleton
    public static UIManager Instance;
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    #endregion

    private void Start()
    {
        moneyText = MoneyUI.transform.Find("Text").GetComponent<Text>();

        factoryBackButton = FactoryUI.transform.Find("Back").Find("Button").GetComponent<Button>();
        factoryName = FactoryUI.transform.Find("Name").GetComponent<Text>();
        factoryCurrentResource= FactoryUI.transform.Find("CurrentResource").Find("ResourceImage").GetComponent<Image>();
        factoryCurrentProductivity= FactoryUI.transform.Find("CurrentResource").Find("Text").GetComponent<Text>();
        factoryRecipeSelector = FactoryUI.transform.Find("Selector").GetChild(0).GetChild(0);
        factoryCurrentState= FactoryUI.transform.Find("Upgrade").Find("Current");
        factoryUpgradeState= FactoryUI.transform.Find("Upgrade").Find("Next");
        factoryUpgradeArrow = FactoryUI.transform.Find("Upgrade").Find("Arrow").gameObject;

        marketBackButton = MarketUI.transform.Find("Back").Find("Button").GetComponent<Button>();
        marketHypedResource = MarketUI.transform.Find("Hype").Find("ResourceImage").GetComponent<Image>();
        marketSellList = MarketUI.transform.Find("ResourcesList");



        QuestButton.onClick.AddListener(OpenQuestWindow);

        questBackButton = QuestUI.transform.Find("Back").Find("Button").GetComponent<Button>();
        questList = QuestUI.transform.Find("QuestList");
        questRequiredStars = QuestUI.transform.Find("Requires").Find("Text").GetComponent<Text>();

        nextLevelButton = QuestUI.transform.Find("Requires").Find("NextLevel").GetComponent<Button>();
        nextLevelButton.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        QuestButton.onClick.RemoveListener(OpenQuestWindow);
    }


    public void DrawResources(Dictionary<Resource,int> resources) 
    {
        ClearRoot(ResourcesListRoot);
        foreach (var resource in resources) 
        {
            if (resource.Value > 0) 
            {
                GameObject ob = Instantiate(ResourceInList, ResourcesListRoot);
                ob.transform.GetComponent<Image>().color = resource.Key.Color;
                ob.transform.GetComponent<Image>().sprite = resource.Key.icon;
                ob.transform.Find("Text").GetComponent<Text>().text = resource.Value.ToString();
            }
        }
    }
    public void DrawMoney(int money) 
    {
        moneyText.text = money.ToString();
        if (prevMoney != money) 
        {
            GameObject ob = Instantiate(droppedText, moneyText.transform.position, Quaternion.identity,moneyText.transform);
            ob.GetComponent<Text>().text = (-(prevMoney - money)).ToString();
        }
        prevMoney = money;
    }

    public void OpenBuildingMenu(Building building) 
    {
        if (building is Factory) 
        {
            OpenFactoryWindow((Factory)building);
        }
        if (building is Market) 
        {
            OpenMarketWindow((Market)building);
        }
    }

    private void OpenFactoryWindow(Factory factory)
    {
        factoryBackButton.onClick.AddListener(CloseFactoryWindow);

        DrawFactory(factory);

        RectTransform factoryUIRect = ((RectTransform)FactoryUI.transform);

        Vector2 targetPosition = factoryUIRect.anchoredPosition;
        factoryUIRect.anchoredPosition += new Vector2(0, -Screen.height);
        factoryUIRect.DOAnchorPos(targetPosition, .5f).SetEase(Ease.OutBack);
        FactoryUI.SetActive(true);
    }
    private void DrawFactory(Factory factory)
    {
        factoryName.text = $"{ factory.factoryName} ({factory.level + 1} ур.)";

        if (factory.GetSelectedRecipe().Empty)
        {
            factoryCurrentResource.sprite = null;
            factoryCurrentResource.color = Color.clear;
            factoryCurrentProductivity.text = "0";
        }
        else
        {
            factoryCurrentResource.sprite = factory.GetSelectedRecipe().result.icon;
            factoryCurrentResource.color = factory.GetSelectedRecipe().result.Color;
            factoryCurrentProductivity.text = $"x{factory.GetCurrentProductivity()}/мин";
        }

        ClearToggleRoot(factoryRecipeSelector);
        for (int i = 0; i < factory.recipes.Length; ++i)
        {
            GameObject ob = Instantiate(RecipeInSelector, factoryRecipeSelector);
            if (!factory.recipes[i].Empty)
            {
                ob.transform.Find("Icon").GetComponent<Image>().sprite = factory.recipes[i].result.icon;
                ob.transform.Find("Icon").GetComponent<Image>().color = factory.recipes[i].result.Color;
                ob.transform.Find("Text").GetComponent<Text>().text = factory.recipes[i].result.name;
                Transform required = ob.transform.Find("Required");
                for (int j = 0; j < factory.recipes[i].materials.Length; ++j)
                {
                    GameObject bb = Instantiate(ResourceInRecipe, required);
                    bb.GetComponent<Image>().sprite = factory.recipes[i].materials[j].icon;
                    bb.GetComponent<Image>().color = factory.recipes[i].materials[j].Color;
                }
            }
            ob.GetComponent<Toggle>().isOn = i == factory.GetSelector();
            int n = i;
            ob.GetComponent<Toggle>().onValueChanged.AddListener(delegate { factory.SelectRecipe(n); DrawFactory(factory); });
        }

        factoryCurrentState.Find("Speed").Find("Text").GetComponent<Text>().text = $"x{factory.speed:F2}";
        factoryCurrentState.Find("Recipes").Find("Text").GetComponent<Text>().text = factory.recipes.Length.ToString();

        if (factory.OnMaxLevel())
        {
            factoryUpgradeArrow.SetActive(false);
            factoryUpgradeState.gameObject.SetActive(false);
        }
        else
        {
            factoryUpgradeArrow.SetActive(true);
            factoryUpgradeState.gameObject.SetActive(true);
            factoryUpgradeState.Find("Speed").Find("Text").GetComponent<Text>().text = $"x{factory.GetNextLevel().speed:F2}";
            factoryUpgradeState.Find("Recipes").Find("Text").GetComponent<Text>().text = factory.GetNextLevel().ChangeRecipeList ? factory.GetNextLevel().recipes.Length.ToString() : factory.recipes.Length.ToString();
            factoryUpgradeState.Find("Price").Find("Text").GetComponent<Text>().text = factory.GetNextLevel().price.ToString();
            factoryUpgradeState.GetComponent<Button>().interactable = ResourceManager.Instance.IsEnoughtMoney(factory.GetNextLevel().price);
            factoryUpgradeState.GetComponent<Button>().onClick.AddListener(delegate { factory.TryUpgrade(); DrawFactory(factory); });
        }

    }
    private void CloseFactoryWindow()
    {
        factoryBackButton.onClick.RemoveListener(CloseFactoryWindow);
        factoryUpgradeState.GetComponent<Button>().onClick.RemoveAllListeners();

        RectTransform factoryUIRect = ((RectTransform)FactoryUI.transform);

        Vector2 bufferPos = factoryUIRect.anchoredPosition;
        Vector2 targetPosition = factoryUIRect.anchoredPosition + new Vector2(0, -Screen.height);

        factoryUIRect.DOAnchorPos(targetPosition, .5f).SetEase(Ease.OutBack).OnComplete(() => { 
            FactoryUI.SetActive(false);
            factoryUIRect.anchoredPosition = bufferPos;
            TouchManager.Instance.SetIgnoreMode(false);
        });
        
        
    }

    private void OpenMarketWindow(Market market) 
    {
        marketBackButton.onClick.AddListener(CloseMarketWindow);

        DrawMarket(market);

        RectTransform marketUIRect = ((RectTransform)MarketUI.transform);

        Vector2 targetPosition = marketUIRect.anchoredPosition;
        marketUIRect.anchoredPosition += new Vector2(0, -Screen.height);
        marketUIRect.DOAnchorPos(targetPosition, .5f).SetEase(Ease.OutBack);
        MarketUI.SetActive(true);
    }
    private void DrawMarket(Market market) 
    {
        marketHypedResource.sprite = ResourceManager.Instance.GetResourceById(market.bestFitResource).icon;
        marketHypedResource.color = ResourceManager.Instance.GetResourceById(market.bestFitResource).Color;

        ClearRoot(marketSellList);
        for (int i = 0; i < ResourceManager.Instance.ResourcesCount; ++i)
        {
            Resource res = ResourceManager.Instance.GetResourceById(i);
            int price = (int)((i == market.bestFitResource ? (market.bestFitMultiplier) : 1) * res.basePrice);
            GameObject ob = Instantiate(ResourceInMarket, marketSellList);
            ob.transform.Find("Image").GetComponent<Image>().sprite = res.icon;
            ob.transform.Find("Image").GetComponent<Image>().color = res.Color;
            ob.transform.Find("Name").GetComponent<Text>().text = $"{res.name} (10 шт.)";
            ob.transform.Find("Price").GetComponent<Text>().text = (price * 10).ToString();
            ob.transform.Find("Sell").GetComponent<Button>().interactable = ResourceManager.Instance.IsEnoughtResource(res, 10);
            ob.transform.Find("Sell").GetComponent<Button>().onClick.AddListener(delegate { ResourceManager.Instance.SellResource(res, price, 10); DrawMarket(market); });
        }
    }
    private void CloseMarketWindow() 
    {
        marketBackButton.onClick.AddListener(CloseMarketWindow);
        RectTransform marketUIRect = ((RectTransform)MarketUI.transform);

        Vector2 bufferPos = marketUIRect.anchoredPosition;
        Vector2 targetPosition = marketUIRect.anchoredPosition + new Vector2(0, -Screen.height);
        TouchManager.Instance.SetIgnoreMode(false);
        marketUIRect.DOAnchorPos(targetPosition, .5f).SetEase(Ease.OutBack).OnComplete(() => {
            MarketUI.SetActive(false);
            marketUIRect.anchoredPosition = bufferPos;
            TouchManager.Instance.SetIgnoreMode(false);
        });
    }

    private void OpenQuestWindow() 
    {
        questBackButton.onClick.AddListener(CloseQuestWindow);
        ClearRoot(questList);
        Quest[] quests = QuestManager.Instance.GetQuests();
        int counter = 0;
        for (int i = 0; i < quests.Length; ++i) 
        {
            GameObject ob = Instantiate(QuestInList, questList);
            ob.transform.Find("Number").GetComponent<Text>().text = $"{i})";
            ob.transform.Find("Description").GetComponent<Text>().text = quests[i].text;
            ob.transform.Find("Toggle").GetComponent<Toggle>().isOn = quests[i].complete;
            if (quests[i].complete) ++counter;
        }
        questRequiredStars.text = $"{counter}/{quests.Length}";

        nextLevelButton.gameObject.SetActive(counter == quests.Length);
        if (counter == quests.Length) 
        {
            nextLevelButton.onClick.AddListener(delegate { QuestManager.Instance.NextLevel(); CloseQuestWindow(); });
        }

        RectTransform questUIRect = ((RectTransform)QuestUI.transform);

        Vector2 targetPosition = questUIRect.anchoredPosition;
        questUIRect.anchoredPosition += new Vector2(-Screen.width,0);
        questUIRect.DOAnchorPos(targetPosition, .5f).SetEase(Ease.OutBack);
        QuestUI.SetActive(true);
    }
    private void CloseQuestWindow() 
    {
        questBackButton.onClick.RemoveListener(CloseQuestWindow);
        nextLevelButton.onClick.RemoveAllListeners();

        RectTransform questUIRect = ((RectTransform)QuestUI.transform);

        Vector2 bufferPos = questUIRect.anchoredPosition;
        Vector2 targetPosition = questUIRect.anchoredPosition + new Vector2(-Screen.width, 0);
        TouchManager.Instance.SetIgnoreMode(false);
        questUIRect.DOAnchorPos(targetPosition, .5f).SetEase(Ease.OutBack).OnComplete(() => {
            QuestUI.SetActive(false);
            questUIRect.anchoredPosition = bufferPos;
            TouchManager.Instance.SetIgnoreMode(false);
        });
    }
    public void QuestCompleted() 
    {
        Instantiate(droppedStar, QuestButton.transform.position, Quaternion.identity, transform);
    }
    #region Utils
    private void ClearRoot(Transform root) 
    {
        for (int i = 0; i < root.childCount; ++i) 
        {
            Destroy(root.GetChild(i).gameObject);
        }
    }
    private void ClearToggleRoot(Transform root) 
    {
        if (root.childCount == 0) return;
        for (int i = 0; i < root.childCount; ++i) 
        {
            root.GetChild(i).GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
            Destroy(root.GetChild(i).gameObject);
        }
    }
    #endregion
}
