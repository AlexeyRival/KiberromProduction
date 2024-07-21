using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : Building
{
    private float bestFitRatio = 50;
    private float timer = .5f;
    public int bestFitResource;
    public float bestFitMultiplier;

    public override void ShowUI()
    {
        MarketWindow window = (MarketWindow)Instantiate(UIPrefab, GameManager.GetUIRoot().transform);
        window.SetContext(this);
        window.Show();
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) 
        {
            timer = bestFitRatio;
            bestFitMultiplier = 1 + Random.Range(1, 21) * .15f;
            bestFitResource = Random.Range(0, GameManager.Instance.GetResourcesTypes().Length);
        }
    }
}
