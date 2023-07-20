using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBaseController : MonoBehaviour
{

    [SerializeField] List<FoodController> Foods;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddFood(FoodController food) {

        if (!Foods.Contains(food))
        {
            Foods.Add(food);
        }


        if (Foods.Count>=QuestManager.instance.currentQuest.TotalCount())
        {
            //if (QuestManager.instance.currentQuest.foodBaseIndex==1)
            //{
          
                FoodBaseManager.instance.SetOrderReady();
            //}
         
        }
    
    }


    public void ResetBase()
    {
        for (int i = 0; i < Foods.Count; i++)
        {
            Foods[i].BackToPool();
        }


        Foods.Clear();
    }


}
