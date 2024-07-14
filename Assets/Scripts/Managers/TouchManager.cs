using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private Camera mainCamera;
    
    private bool isTouched;
    private bool ignoreTouches;
    private Ray ray;
    private RaycastHit hit;

    #region Singleton
    public static TouchManager Instance;
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    #endregion

    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (ignoreTouches) return;
        if (!mainCamera) { mainCamera = Camera.main; }
        isTouched = false;

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            isTouched = true;
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isTouched = true;
                ray = mainCamera.ScreenPointToRay(touch.position);
            }
        }
#endif
        if (isTouched)
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Building")) 
                {
                    UIManager.Instance.OpenBuildingMenu(hit.transform.GetComponent<Building>());
                    SetIgnoreMode(true);
                }
                Debug.Log("Рейкаст попал в объект: " + hit.collider.gameObject.name);
            }
        }
    }

    public void SetIgnoreMode(bool ignore) 
    {
        ignoreTouches = ignore;
    }
}
