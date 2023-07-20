using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FoodController : MonoBehaviour
{
    [SerializeField] GameObject[] Foods;
    [SerializeField] FoodBlendController currentFood;

    [SerializeField] Transform _foodParent;
    private int foodType;

    private Rigidbody _rb;
    private BoxCollider _collider;

    [SerializeField] Vector3[] Scales;
    [SerializeField] Vector3[] ColliderSizes;
    [SerializeField] Vector3 _colliderBaseSize;
    Vector3 currentScale;


    [SerializeField] float yDif;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Set_ResetFood

    public void SetFood(int index)
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        foodType = index;
        _collider.size = ColliderSizes[foodType];
        Foods[foodType].SetActive(true);
        currentFood = Foods[foodType].GetComponent<FoodBlendController>();
        currentScale = Scales[foodType];
        currentFood.SetBlend();
    }


    public void ResetFood()
    {
        for (int i = 0; i < Foods.Length; i++)
        {
            Foods[i].SetActive(false);
        }
        _foodParent.localScale = Vector3.one;
        _rb.isKinematic = true;
        _collider.size = _colliderBaseSize;
        _collider.enabled = false;

        currentFood.ResetBlend();
        currentFood = null;
    }

    public void BackToPool()
    {
        ResetFood();
        FoodPool.instance.RemoveFood(gameObject);

    }

    #endregion



    #region Locating

    // Secilen yiyecegin yerlestirilmesi mekanigi

    public void Locate(Vector3 pos,FoodBaseController _base) {

        if (QuestManager.instance.questValue == 0 && FoodBaseManager.instance.totalFood == 0)
        {
    
         FoodBaseManager.instance.OffSign();
            

        }

        for (int i = 0; i < 4; i++)
        {
            if (foodType== QuestManager.instance.currentQuest.FoodIndexes[i])
            {
                if (FoodBaseManager.instance.currentQuestCounts[i] > 0)
                {
                    FoodBaseManager.instance.currentQuestCounts[i]--;
                    FoodBaseManager.instance.totalTrueFood++;


                }
            }

       
        }
        FoodBaseManager.instance.totalFood++;
        StartCoroutine(Locating(pos, _base));

    }

    public void LocateBack(Vector3 pos)
    {


        StartCoroutine(LocatingBack(pos));



    }


    IEnumerator Locating(Vector3 pos,FoodBaseController _base)
    {


        transform.DOMove(pos+yDif*Vector3.up, 0.2f);
        transform.DOScale(currentScale, 0.2f);
        yield return new WaitForSeconds(0.2f);
        currentFood.SetBlend2();
        _rb.isKinematic = false;
        _collider.enabled = true;
        yield return new WaitForSeconds(0.4f);
        _rb.isKinematic = true;
        transform.parent = _base.transform;
        _base.AddFood(this);

    }


    IEnumerator LocatingBack(Vector3 pos)
    {
        transform.DOMove(pos, 0.2f);
        transform.DOScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(0.2f);
        BackToPool();
    }


    #endregion
}
