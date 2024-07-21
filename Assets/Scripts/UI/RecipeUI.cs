using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [Header("UI")]
    public Image icon;
    public Text nameText;
    public Transform requiredResourcesRoot;

    [Header("Prefabs")]
    public Image materialPrefab;
    public void Fill(Recipe recipe) 
    {
        if (recipe.Empty) 
        {
            return;
        }
        icon.sprite = recipe.result.icon;
        icon.color = recipe.result.Color;
        nameText.text = recipe.result.name;
        for (int i = 0; i < recipe.materials.Length; ++i) 
        {
            Image im = Instantiate(materialPrefab, requiredResourcesRoot);
            im.color = recipe.materials[i].Color;
            im.sprite = recipe.materials[i].icon;
        }
    }
}
