using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketWindow : BaseWindow
{
    [Header("UI")]
    [SerializeField] private Image hypedResourceImage;
    [SerializeField] private Transform resourceList;

    [SerializeField] private Button closeButton;

    [Header("Prefabs")]
    [SerializeField] private SellUI sellPrefab;

    private Market market;
    private ResourceManager resourceManager;
    public void SetContext(Market market) 
    {
        this.market = market;
    }

    public override void Show()
    {
        resourceManager = GameManager.GetManager<ResourceManager>();
        closeButton.onClick.AddListener(Hide);

        RectTransform UIRect = ((RectTransform)transform);

        Vector2 targetPosition = UIRect.anchoredPosition;
        UIRect.anchoredPosition += new Vector2(0, -Screen.height);
        UIRect.DOAnchorPos(targetPosition, .5f).SetEase(Ease.OutBack);
        
        Draw();
    }
    public override void Draw()
    {

        Resource[] resourceTypes = resourceManager.GetResourcesTypes();

        hypedResourceImage.sprite = resourceTypes[market.bestFitResource].icon;
        hypedResourceImage.color = resourceTypes[market.bestFitResource].Color;

        resourceList.ClearRoot();
        for (int i = 0; i < resourceTypes.Length; ++i)
        {
            Resource res = resourceTypes[i];
            int price = (int)((i == market.bestFitResource ? (market.bestFitMultiplier) : 1) * res.basePrice);
            SellUI ob = Instantiate(sellPrefab, resourceList);
            int count = 10;
            ob.Fill(res, price * count, resourceManager.IsEnoughtResource(res, count));
            int j = i;
            ob.onSell += delegate { SellResource(j, price, 10); };
        }
    }
    public override void Hide()
    {
        closeButton.onClick.RemoveAllListeners();


        RectTransform UIRect = ((RectTransform)transform);

        Vector2 bufferPos = UIRect.anchoredPosition;
        Vector2 targetPosition = UIRect.anchoredPosition + new Vector2(0, -Screen.height);

        UIRect.DOAnchorPos(targetPosition, .5f).SetEase(Ease.OutBack).OnComplete(() => {
            Destroy(gameObject);
            GameManager.GetManager<TouchManager>().SetIgnoreMode(false);
        });

    }

    private void SellResource(int id, int price, int count) 
    {
        resourceManager.DeltaMoney(price*count);
        resourceManager.DeltaResource(id, -count);
        Draw();
    }
}
