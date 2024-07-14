using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Factory : Building
{
    public string factoryName;
    public Recipe[] recipes;
    public Upgrade[] upgrades;
    public int level = 0;

    public float speed = 1f;
    private int selectedRecipe;

    private float timer;
    private bool isProduced = false;
    private bool upgradeShook;

    public void TryUpgrade()
    {
        if (OnMaxLevel()) return;
        if (upgradeShook) return;
        if (ResourceManager.Instance.IsEnoughtMoney(GetNextLevel().price))
        {
            ResourceManager.Instance.DeltaMoney(-GetNextLevel().price);
            Upgrade();
        }
    }
    private void Upgrade()
    {
        for (int i = 0; i < GetNextLevel().activateOnUpgrade.Length; ++i)
        {
            GetNextLevel().activateOnUpgrade[i].SetActive(true);
        }
        if (GetNextLevel().ChangeSpeed) { speed = GetNextLevel().speed; }
        if (GetNextLevel().ChangeRecipeList) { recipes = GetNextLevel().recipes; }
        level++;
        upgradeShook = true;
    }
    public bool OnMaxLevel() 
    {
        return level >= upgrades.Length-1;
    }
    public Upgrade GetNextLevel() 
    {
        return upgrades[level + 1];
    }
    void Update()
    {
        upgradeShook = false;
        if (recipes[selectedRecipe].Empty) return;

        timer -= Time.deltaTime*speed;

        if (timer <= 0)
        {
            if (!isProduced) 
            {
                ResourceManager.Instance.DeltaResource(recipes[selectedRecipe].result, 1);
                isProduced = true;
            }


            if (TryProduce())
            {
                timer = recipes[selectedRecipe].time;
                isProduced = false;
            }
        }
    }
    private bool TryProduce() 
    {

        if (recipes[selectedRecipe].materials.Length == 0)
        {
            return true;
        }
        else
        {
            bool canProduce = true;
            for (int i = 0; i < recipes[selectedRecipe].materials.Length; ++i)
            {
                if (!ResourceManager.Instance.IsEnoughtResource(recipes[selectedRecipe].materials[i], 1))
                {
                    canProduce = false;
                    break;
                }
            }
            if (canProduce)
            {
                for (int i = 0; i < recipes[selectedRecipe].materials.Length; ++i)
                {
                    ResourceManager.Instance.DeltaResource(recipes[selectedRecipe].materials[i], -1);
                }
                return true;
            }
        }
        return false;
    }
    
    public void SelectRecipe(int id) 
    {
        selectedRecipe = id;

        if (TryProduce())
        {
            timer = recipes[selectedRecipe].time;
            isProduced = false;
        }
    }
    public int GetSelector() { return selectedRecipe; }
    public Recipe GetSelectedRecipe() { return recipes[selectedRecipe]; }
    
    public int GetCurrentProductivity() 
    {
        return (int)(60/recipes[selectedRecipe].time * speed);
    }

    public string Save() 
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(level);
        sb.Append(':');
        sb.Append(selectedRecipe);
        return sb.ToString();
    }
    public void Load(string data) 
    {
        int newlevel = int.Parse(data.Split(':')[0]);
        selectedRecipe = int.Parse(data.Split(':')[1]);
        for (int i = 0; i < newlevel; ++i) 
        {
            Upgrade();
        }
    }
}
