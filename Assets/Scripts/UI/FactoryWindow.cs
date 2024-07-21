using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FactoryWindow : BaseWindow 
{
    [Header("Factory name")]
    [SerializeField] private Text nameText;

    [Header("Current Resource")]
    [SerializeField] private Image currentResourceImage;
    [SerializeField] private Text currentResourceText;

    [Header("Recipe Selector")]
    [SerializeField] private Transform recipeSelectorRoot;

    [Header("Upgrades")]
    [SerializeField] private Text currentSpeedText;
    [SerializeField] private Text currentRecipesText;
    [SerializeField] private Text upgradeSpeedText;
    [SerializeField] private Text upgradeRecipesText;
    [SerializeField] private Text upgradePriceText;
    [SerializeField] private GameObject upgradeArrow;
    [SerializeField] private GameObject upgradeBlock;
    [SerializeField] private Button upgradeButton;

    [Header("Close")]
    [SerializeField] private Button closeButton;

    [Header("Prefabs")]
    [SerializeField] private RecipeUI recipePrefab;

    private Factory factory;
    public void SetContext(Factory factory) 
    {
        this.factory = factory;
    }

    public override void Show()
    {
        closeButton.onClick.AddListener(Hide);

        RectTransform UIRect = ((RectTransform)transform);

        Vector2 targetPosition = UIRect.anchoredPosition;
        UIRect.anchoredPosition += new Vector2(0, -Screen.height);
        UIRect.DOAnchorPos(targetPosition, .5f).SetEase(Ease.OutBack);
        
        Draw();
    }
    public override void Draw() 
    {

        nameText.text = factory.factoryName;
        
        Recipe currentRecipe = factory.GetSelectedRecipe();

        if (!currentRecipe.Empty)
        {
            currentResourceImage.sprite = currentRecipe.result.icon;
            currentResourceImage.color = currentRecipe.result.Color;
            currentResourceText.text = $"x{factory.GetCurrentProductivity()}/мин";
        }
        else
        {
            currentResourceImage.sprite = null;
            currentResourceImage.color = Color.clear;
            currentResourceText.text = "Ничего не производится";
        }

        recipeSelectorRoot.ClearRoot();
        for (int i = 0; i < factory.recipes.Length; ++i) 
        {
            RecipeUI recipe = Instantiate(recipePrefab, recipeSelectorRoot);
            recipe.Fill(factory.recipes[i]);
            
            int j = i;
            recipe.GetComponent<Toggle>().isOn = i == factory.GetSelector();
            recipe.GetComponent<Toggle>().onValueChanged.AddListener((a)=>SelectRecipe(a,j));
            recipe.GetComponent<Toggle>().group = recipeSelectorRoot.GetComponent<ToggleGroup>();
        }

        Upgrade currentLevel = factory.GetCurrentLevel();
        currentSpeedText.text = $"x{currentLevel.speed:F2}";
        currentRecipesText.text = factory.recipes.Length.ToString();

        if (!factory.OnMaxLevel())
        {
            Upgrade nextLevel = factory.GetNextLevel();
            upgradeSpeedText.text = $"x{nextLevel.speed:F2}";
            upgradeRecipesText.text = (nextLevel.ChangeRecipeList ? nextLevel.recipes.Length : factory.recipes.Length).ToString();
            upgradePriceText.text = nextLevel.price.ToString();
            upgradeButton.interactable = GameManager.GetManager<ResourceManager>().IsEnoughtMoney(nextLevel.price);
            upgradeButton.onClick.AddListener(delegate { factory.TryUpgrade();Draw(); });
        }
        else 
        {
            upgradeBlock.SetActive(false);
            upgradeArrow.SetActive(false);
        }

        
    }
    public override void Hide() 
    {
        upgradeButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();

        RectTransform UIRect = ((RectTransform)transform);

        Vector2 bufferPos = UIRect.anchoredPosition;
        Vector2 targetPosition = UIRect.anchoredPosition + new Vector2(0, -Screen.height);

        UIRect.DOAnchorPos(targetPosition, .5f).SetEase(Ease.OutBack).OnComplete(() => {
            Destroy(gameObject);
            GameManager.GetManager<TouchManager>().SetIgnoreMode(false);
        });
    }

    private void SelectRecipe(bool isSelected,int id) 
    {
        if (!isSelected) return;
        factory.SelectRecipe(id);
        Draw();
    }
}
