using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPool : MonoBehaviour
{



    [SerializeField] GameObject _foodParticle;
    [SerializeField] GameObject _foodPrefab;

    private ObjectPool _particlePool;
    private ObjectPool _foodPool;

    public static FoodPool instance;




    private void Awake()
    {
        instance = this;
    }

    void Start()
    {


        _foodPool = new ObjectPool(_foodPrefab);
        _foodPool.FillPool(2);

        _particlePool = new ObjectPool(_foodParticle);
        _particlePool.FillPool(2);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SpawnFood(Vector3 pos, Vector3 rot, int index, FoodSourceManager source)
    {


        GameObject food = _foodPool.CallObjectToPool();
        food.transform.parent = null;
        food.transform.position = pos;
        food.transform.eulerAngles = rot;
        // food.GetComponent<SlimeController>().SetPoolSlime(lv,QuestManager.instance.typeMax);
        food.GetComponent<FoodController>().SetFood(index);
        source.currentFood = food;
    }

    public void SpawnMergePart(Vector3 pos, float f)
    {
        GameObject mergePart = _particlePool.CallObjectToPool();
        mergePart.transform.position = pos;


    }

    public void RemoveParticle(GameObject particle)
    {
        _particlePool.AddObjectToPool(particle);
    }


    public void RemoveFood(GameObject food)
    {
        _foodPool.AddObjectToPool(food);
    }


}
