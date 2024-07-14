using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public bool ChangeSpeed = true;
    public float speed = 1f;
    public bool ChangeRecipeList = false;
    public Recipe[] recipes;
    public int price;
    public GameObject[] activateOnUpgrade;
}
