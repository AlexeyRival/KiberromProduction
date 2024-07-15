using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Recipe")]
public class Recipe : ScriptableObject
{
    public bool Empty;
    public Resource[] materials;
    public Resource result;
    public float time;
}
