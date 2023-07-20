using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FoodSourceManager : MonoBehaviour
{
    [SerializeField] Camera _camera;
 
    bool selected;
    bool contact;
   


    [SerializeField] GameObject _particle;
    [SerializeField] GameObject[] Foods;
    public GameObject currentFood;

    public int dir;
    private Vector3 offset;
    private Vector3 _posFirst;

    private int foodType;

    [SerializeField] Vector3 _contactPoint;
    FoodBaseController _contactBase;

    // [SerializeField] GunSlotManager _contactSlot;


    // Start is called before the first frame update
    void Start()
    {
        _posFirst = transform.localPosition;
   
        _camera = Camera.main;
    }


    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            if (Input.GetMouseButton(0))
            {


                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("FoodBase"))
                    {
                        contact = true;
                        _contactPoint = hit.point;

                        if (QuestManager.instance.currentQuest.foodBaseIndex == 1)
                        {
                            _contactBase = hit.collider.transform.parent.GetComponent<FoodBaseController>();
                        }
                        else
                        {
                            _contactBase = hit.collider.GetComponent<FoodBaseController>();
                        }
                    }
                    else
                    {
                        contact = false;
                        _contactPoint = transform.position;
                        _contactBase = null;

                    }




                    // Debug.Log(hit.collider.tag);
                }
            }






        }



    }



    private void OnTriggerEnter(Collider other)
    {

    }


    private void OnTriggerExit(Collider other)
    {
  
    }



   

    IEnumerator Inflate(float lastScale, Transform tr)
    {

        tr.DOScale(Vector3.one * lastScale * 1.4f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        tr.DOScale(Vector3.one * lastScale, 0.1f);
        yield return new WaitForSeconds(0.1f);

    }




    private Vector3 GetMousePos(Vector3 pos)
    {

        // return _camera.WorldToScreenPoint(transform.position);




        return _camera.WorldToScreenPoint(pos);
    }


    private void OnMouseDown()
    {
        //offset = gameObject.transform.position -
        //    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        GetComponent<BoxCollider>().enabled = false;
        selected = true;
        Transform hTransform = FoodHandlerManager.instance.transform;
        FoodPool.instance.SpawnFood(hTransform.position, hTransform.localEulerAngles, foodType,this);
        currentFood.transform.parent = hTransform;
        PositionObject(hTransform);
        offset = Input.mousePosition - GetMousePos(hTransform.position);
        //GameManager.instance.Vib();
    
    }



    private void PositionObject(Transform hTransform)
    {
        Vector3 direction = (transform.position - _camera.transform.position).normalized;
        float distance = Vector3.Distance(transform.position, _camera.transform.position);

        hTransform.position = _camera.transform.position + (distance*0.8f ) * direction;
    }




    void OnMouseDrag()
    {
        //Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        //transform.position = Camera.main.ScreenToWorldPoint(newPosition) + offset;

        Transform hTransform = FoodHandlerManager.instance.transform;

        hTransform.position = _camera.ScreenToWorldPoint(Input.mousePosition - offset);

        currentFood.transform.position = hTransform.position;

    }


    private void OnMouseUp()
    {
        currentFood.transform.parent = null;
        //  currentFood.transform.position = _contactPoint;

        if (contact)
        {
            currentFood.GetComponent<FoodController>().Locate(_contactPoint,_contactBase);
        }
        else
        {
            currentFood.GetComponent<FoodController>().LocateBack(transform.position);
        }
       
       // currentFood.GetComponent<FoodController>().BackToPool();
        currentFood = null;
        GetComponent<BoxCollider>().enabled = true;
        selected = false;
    
    }


    public void ResetPos()
    {


        transform.localPosition = _posFirst;
        GetComponent<BoxCollider>().enabled = true;
        contact = false;


    }

    IEnumerator Moving()
    {
        // GetComponent<SphereCollider>().enabled = false;
        transform.DOLocalMove(_posFirst, 0.2f);
        yield return new WaitForSeconds(0.2f);
        GetComponent<BoxCollider>().enabled = true;
        // GetComponent<SphereCollider>().enabled = true;
    }

  



    public void SetFoodSource(int index)
    {
        foodType = index;
    }

}
