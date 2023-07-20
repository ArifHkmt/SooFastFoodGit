using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTabController : MonoBehaviour
{

    [SerializeField] FoodSourceManager _foodSource;

    [SerializeField] GameObject[] Foods;

    public FoodTabController nextTab;
    public FoodTabController preTab;

    [SerializeField] Camera _camera;
    private Vector3 offset;
    public int dir;
    [SerializeField]  bool selected;


    public Transform refRight;
    public Transform refLeft;
    [SerializeField] int foodType;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;


        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("BorderRight"))
        {
            transform.position = nextTab.refLeft.position;
        }

        if (other.CompareTag("BorderLeft"))
        {
            transform.position = preTab.refRight.position;
        }

    }



    private Vector3 GetMousePos(Vector3 pos)
    {

        // return _camera.WorldToScreenPoint(transform.position);




        return _camera.WorldToScreenPoint(pos);
    }


    #region TabMovement

    // Yiyecek bloklarinin serit halinde hareket etmesi

    private void OnMouseDown()
    {
        //offset = gameObject.transform.position -
        //    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
       // GetComponent<BoxCollider>().enabled = false;
        selected = true;
       // PositionObject(transform);
        offset = Input.mousePosition - GetMousePos(transform.position);
        //GameManager.instance.Vib();

        FoodTabsManager.instance.SetTabsParent(transform);
    }

    private void PositionObject(Transform hTransform)
    {
        Vector3 direction = (transform.position - _camera.transform.position).normalized;
        float distance = Vector3.Distance(transform.position, _camera.transform.position);
        hTransform.position = _camera.transform.position + (distance / 2) * direction;
    }

    void OnMouseDrag()
    {
        //Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        //transform.position = Camera.main.ScreenToWorldPoint(newPosition) + offset;

       //Vector3 hTransform =transform.position;

        Vector3 hTransform = _camera.ScreenToWorldPoint(Input.mousePosition - offset);

        hTransform.y = transform.position.y;
        hTransform.z = transform.position.z;
        transform.position = hTransform;

    }

    private void OnMouseUp()
    {
        selected = false;
        if (QuestManager.instance.questValue==0&&FoodBaseManager.instance.totalFood==0)
        {
            if (FoodBaseManager.instance.totalFood == 0)
            {
                FoodBaseManager.instance.SetDragSign();
            }
            else
            {
                FoodBaseManager.instance.OffSign();
            }
           
        }
        FoodTabsManager.instance.ResetTabsParent();
    }

    #endregion


    public void SetTab(int index)
    {
        //foodType = index;
        Foods[foodType].SetActive(true);
        _foodSource.SetFoodSource(foodType);
    }

    public void TabsOff()
    {
        GetComponent<Collider>().enabled = false;
        _foodSource.GetComponent<Collider>().enabled = false;

    }

    public void TabsOn()
    {
        GetComponent<Collider>().enabled = true;
        _foodSource.GetComponent<Collider>().enabled = true;
    }

    
  

}
