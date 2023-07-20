using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FoodTabsManager : MonoBehaviour
{
    [SerializeField] FoodTabController[] FoodTabs;
    [SerializeField] Transform _foodParent;



    public FoodTabController lastFoodTab;
    public FoodTabController firstFoodTab;
    



    public static FoodTabsManager instance;
    private void Awake()
    {
        instance = this;
        SetTabs();
    }

    void Start()
    {

    
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region TabsMovementStack

    public void SetTabsParent(Transform tr)
    {
        for (int i = 0; i < FoodTabs.Length; i++)
        {


            if (FoodTabs[i].transform!=tr)
            {
                FoodTabs[i].transform.parent = tr;
            }
            
        }
    }


    public void ResetTabsParent()
    {
        {
            for (int i = 0; i < FoodTabs.Length; i++)
            {

                FoodTabs[i].transform.parent = _foodParent;


            }
        }
    }

    #endregion





    public void SetTabs()
    {
        for (int i = 0; i < _foodParent.childCount; i++)
        {
            FoodTabs[i] = _foodParent.GetChild(i).GetComponent<FoodTabController>();

      
            FoodTabs[i].SetTab(i);
        }
        for (int i = 1; i < _foodParent.childCount; i++)
        {
            FoodTabs[i].preTab = _foodParent.GetChild(i - 1).GetComponent<FoodTabController>();
        }

        for (int i = 0; i < _foodParent.childCount - 1; i++)
        {
            FoodTabs[i].nextTab = _foodParent.GetChild(i + 1).GetComponent<FoodTabController>();
        }

        lastFoodTab = FoodTabs[^1];
        firstFoodTab = FoodTabs[0];

        firstFoodTab.preTab = lastFoodTab;
        lastFoodTab.nextTab = firstFoodTab;
    }


    public void TabsOff()
    {
        transform.DOMoveZ(-5, 1f);

        for (int i = 0; i < FoodTabs.Length; i++)
        {
            FoodTabs[i].TabsOff();
        }
    }

    public void TabsOn()
    {
        transform.DOMoveZ(0, 1f);

        for (int i = 0; i < FoodTabs.Length; i++)
        {
            FoodTabs[i].TabsOn();
        }

    }


}
