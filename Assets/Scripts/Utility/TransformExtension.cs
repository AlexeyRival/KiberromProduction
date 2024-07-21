using UnityEngine;
using UnityEngine.UI;

public static class TransformExtension
{
    public static void ClearRoot(this Transform transform) 
    {
        for (int i = 0; i < transform.childCount; ++i) 
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }
    public static void ClearRootWithToggles(this Transform transform) 
    {
        if (transform.childCount == 0) return;
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }
}
