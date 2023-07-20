using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FoodBaseManager : MonoBehaviour
{
    [SerializeField] Image _scrollSign;
    [SerializeField] Image _dragSign;
    [SerializeField] Image _dragSign2;
    [SerializeField] Image _infSign;
    [SerializeField] Image _tapSign;
    [SerializeField] Image _tapSign2;

    [SerializeField] Transform _mustard1Ref1;
    [SerializeField] Transform _mustard1Ref2;
    [SerializeField] Transform _mustard2Ref1;
    [SerializeField] Transform _mustard2Ref2;


    [SerializeField] GameObject _waffleMachine;
    [SerializeField] GameObject _oven;

    [SerializeField] Transform _lidWaffle;
    [SerializeField] Transform _lid;
    [SerializeField] GameObject _roller;
    [SerializeField] GameObject _mustard;
    [SerializeField] GameObject _mustard2;

    [SerializeField] Transform _customerRef;
    [SerializeField] Transform _pizzaRef;
    [SerializeField] Transform _waffleRef;
    [SerializeField] Transform _breadRef0;
    [SerializeField] Transform _breadRef1;

    [SerializeField] GameObject[] WaffleForms;
    [SerializeField] GameObject[] PizzaForms;
    [SerializeField] GameObject _bread;

    [SerializeField] SkinnedMeshRenderer _pizzaMesh1;
    [SerializeField] SkinnedMeshRenderer _pizzaMesh2;
    [SerializeField] SkinnedMeshRenderer _waffleMesh1;
    [SerializeField] SkinnedMeshRenderer _waffleMesh2;


    [SerializeField] bool blendPizza1;
    [SerializeField] bool blendPizza2;
    [SerializeField] bool blendWaffle1;
    [SerializeField] bool blendWaffle2;

    [SerializeField] int _pizzaStep;
    [SerializeField] int _waffleStep;

    public int totalTrueFood;
    public int totalFood;
    public int[] currentQuestCounts;
    [SerializeField] FoodBaseController[] Bases;
    public FoodBaseController currentBase;

    private Vector3 _posFirst;
    private Vector3 _rotFirst;



  
    public bool readyToDeliver;
    float blendValue;
    [SerializeField] int _type;
    public static FoodBaseManager instance;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
       
        _posFirst = transform.position;
        _rotFirst = transform.eulerAngles;

    }

    // Update is called once per frame
    void Update()
    {



        BlendWorks();
        SetTouch();


       
    }

    #region Set FoodBase and manager


    public void SetFoodBase(int index)
    {
        Bases[index].gameObject.SetActive(true);
        currentBase = Bases[index];
        _type = index;
        SetCurrentQuest();

        switch (_type)
        {
            case 0:
                if (QuestManager.instance.questValue==0)
                {
                    SetSign(_scrollSign);
                }
                else
                {
                  //  SetSign(_dragSign);
                }
                currentBase.transform.position = _customerRef.position;
                currentBase.transform.DOLocalMove(Vector3.zero, 0.4f);
                GetComponent<Collider>().enabled = false;
                FoodTabsManager.instance.TabsOn();
                break;
            case 1:
                currentBase.transform.position = _customerRef.position;
                currentBase.transform.DOLocalMove(Vector3.zero, 0.4f);
                SetSign(_dragSign);
                GetComponent<Collider>().enabled = true;
                PizzaForms[0].SetActive(true);
                break;
            case 2:
                SetSign(_infSign);
                GetComponent<Collider>().enabled = true;
                FoodTabsManager.instance.TabsOff();
                currentBase.transform.position = _waffleRef.position;
                WaffleForms[0].SetActive(true);
                _mustard.transform.DOMove(_mustard1Ref2.position, 0.5f);
                _mustard.transform.DORotate(_mustard1Ref2.eulerAngles, 0.5f);
                _lidWaffle.transform.DOLocalRotate(Vector3.right * 60, 0.4f);
                break;
        }
    }

    public void ResetBase()
    {
        for (int i = 0; i < Bases.Length; i++)
        {
            Bases[i].ResetBase();
            Bases[i].transform.parent = transform;
            Bases[i].transform.localPosition = Vector3.zero;
            Bases[i].transform.localEulerAngles = Vector3.zero;
            Bases[i].gameObject.SetActive(false);
            
        }

        currentBase = null;
        totalTrueFood = 0;
        totalFood = 0;
        transform.position = _posFirst;
        transform.eulerAngles = _rotFirst;
        _pizzaStep = 0;
        _waffleStep = 0;
        _pizzaMesh1.SetBlendShapeWeight(0, 0);
        _pizzaMesh2.SetBlendShapeWeight(0, 100);
        _waffleMesh2.SetBlendShapeWeight(0, 0);
        _waffleMesh1.SetBlendShapeWeight(0, 100);
        blendPizza1 = false;
        blendPizza2 = false;
        blendWaffle1 = false;
        blendWaffle2 = false;
        _roller.transform.localPosition = new Vector3(0, 0.15f, -0.5f);
        _mustard.transform.DOMove(_mustard1Ref1.position, 0.5f);
        _mustard.transform.DORotate(_mustard1Ref1.eulerAngles, 0.5f);
        _mustard2.transform.DOMove(_mustard2Ref1.position, 0.5f);
        _mustard2.transform.DORotate(_mustard2Ref1.eulerAngles, 0.5f);

        for (int i = 0; i < PizzaForms.Length; i++)
        {
            PizzaForms[i].SetActive(false);
        }
        for (int i = 0; i < WaffleForms.Length; i++)
        {
            WaffleForms[i].SetActive(false);
        }
        _bread.transform.localPosition =_breadRef0.localPosition;
        _bread.transform.localEulerAngles = _breadRef0.localEulerAngles; 
    }

    public void SetCurrentQuest()
    {
        for (int i = 0; i < 4; i++)
        {

            currentQuestCounts[i] = QuestManager.instance.currentQuest.FoodCounts[i];

        }


    }

    #endregion

    #region Order Settings
    public void SetOrderReady()
    {

        SetSign(_tapSign2);
        readyToDeliver = true;
        FoodTabsManager.instance.TabsOff();
        GetComponent<Collider>().enabled = true;
    }

    public void DeliverOrder()
    {
        readyToDeliver = false;
        StartCoroutine(Delivering());
    }

    IEnumerator Delivering()
    {
        if (_type==0)
        {
            _bread.transform.DOLocalMove(_breadRef1.localPosition, 0.4f); 
            _bread.transform.DOLocalRotate(_breadRef1.localEulerAngles, 0.4f);
            yield return new WaitForSeconds(0.6f);
        }
   
        currentBase.transform.DOMove(_customerRef.position, 1f);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < Bases.Length; i++)
        {
            Bases[i].ResetBase();
            Bases[i].transform.parent = transform;
            Bases[i].transform.localPosition = Vector3.zero;
            Bases[i].transform.localEulerAngles = Vector3.zero;
            Bases[i].gameObject.SetActive(false);

        }
        currentBase.transform.parent = _customerRef;
        currentBase.transform.localEulerAngles = Vector3.zero;
        QuestManager.instance.Mission(1);

    }

#endregion

    #region Pizza Steps
    private void PizzaStep1()
    {
      
        _roller.SetActive(true);
        _pizzaStep = 1;
        blendValue = 0;

    }


    private void PizzaStep2()
    {
  
        _roller.SetActive(false);
        blendPizza1 = false;
        _pizzaStep = 2;
        PizzaForms[1].SetActive(true);
        blendValue = 100;
    }

    private void PizzaStep3()
    {
 
        FoodTabsManager.instance.TabsOn();
        GetComponent<Collider>().enabled = false;
        blendPizza2 = false;
        _pizzaStep = 3;

    }

    private void PizzaStep4()
    {

        GetComponent<Collider>().enabled = false;
        blendPizza1 = false;
        blendPizza2 = false;
        _pizzaStep = 3;
        PizzaForms[2].SetActive(true);

    }


    private void CookPizza()
    {

   
        StartCoroutine(CookingPizza());

    }


    IEnumerator CookingPizza()
    {
        _lid.transform.DOLocalRotate(Vector3.right * 90, 0.4f);
        yield return new WaitForSeconds(0.4f);
        currentBase.transform.DOMove(_pizzaRef.position, 1f);
        yield return new WaitForSeconds(1f);
        _lid.transform.DOLocalRotate(Vector3.zero, 0.4f);
        yield return new WaitForSeconds(1f);
        _lid.transform.DOLocalRotate(Vector3.right * 90, 0.4f);
        PizzaStep4();
        yield return new WaitForSeconds(0.4f);
        currentBase.transform.DOLocalMove(Vector3.zero, 1f);
        yield return new WaitForSeconds(1f);
        _lid.transform.DOLocalRotate(Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.4f);
        GetComponent<Collider>().enabled = true;

        DeliverOrder();

    }




    #endregion


    #region WaffleSteps
    private void WaffleStep1()
    {
        _mustard.SetActive(true);
        _waffleStep = 1;
        blendValue = 100;
    }

    private void WaffleStep2()
    {
   
        blendWaffle1 = false;

        StartCoroutine(WStep2());

    }

    IEnumerator WStep2()
    {
       
        _mustard2.transform.DOMove(_mustard2Ref2.position, 0.5f);
        _mustard2.transform.DORotate(_mustard2Ref2.eulerAngles, 0.5f);
        _waffleStep = 2;
        blendValue = 0;
        yield return new WaitForSeconds(0.5f);
        SetSign(_infSign);
    }

    private void WaffleStep3()
    {
        FoodTabsManager.instance.TabsOn();
        _mustard2.transform.DOMove(_mustard2Ref1.position, 0.5f);
        _mustard2.transform.DORotate(_mustard2Ref1.eulerAngles, 0.5f);
        _waffleStep = 3;
        blendValue = 0;
        blendWaffle2 = false;
        GetComponent<Collider>().enabled = false;

    
    }


    private void CookWaffle()
    {


        StartCoroutine(CookingWaffle());

    }

    IEnumerator CookingWaffle()
    {
        // _mustard.SetActive(false);
        _mustard.transform.DOMove(_mustard1Ref1.position, 0.5f);
        _mustard.transform.DORotate(_mustard1Ref1.eulerAngles, 0.5f);
        GetComponent<Collider>().enabled = false;
         _lidWaffle.transform.DOLocalRotate(Vector3.zero, 0.4f);
        yield return new WaitForSeconds(1f);
        _lidWaffle.transform.DOLocalRotate(Vector3.right * 60, 0.4f);
        WaffleForms[0].SetActive(false);
        WaffleForms[1].SetActive(true);
        yield return new WaitForSeconds(0.4f);
       // _mustard.SetActive(false);
        currentBase.transform.DOLocalMove(Vector3.zero, 1f);
        yield return new WaitForSeconds(1f);
        WaffleStep2();
        _lidWaffle.transform.DOLocalRotate(Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.4f);
  
        GetComponent<Collider>().enabled = true;
   

    }

    #endregion


    #region Input Settings
    
    private void OnMouseDrag()
    {
        if (_type == 1 && !readyToDeliver)
        {

            if (_pizzaStep == 1)
            {
                blendPizza1 = true;
                blendPizza2 = false;
            }
            if (_pizzaStep == 2)
            {
                blendPizza1 = false;
                blendPizza2 = true;
            }
        }

        if (_type == 2 && !readyToDeliver)
        {

            if (_waffleStep == 1)
            {
                blendWaffle1 = true;
                blendWaffle2 = false;
            }
            if (_waffleStep == 2)
            {
                blendWaffle1 = false;
                blendWaffle2 = true;
            }
        }

    }



    private void OnMouseUp()
    {
        if (_type == 1 )
        {
            if (_pizzaStep==1)
            {
                SetSign(_infSign);
            }
            if (_pizzaStep == 2)
            {
                SetSign(_dragSign);
            }

            if (_pizzaMesh2.GetBlendShapeWeight(0) <= 1)
            {
                PizzaStep3();
            }
            blendPizza1 = false;
            blendPizza2 = false;
        }



        if (_type == 2)
        {

          

            if (_waffleMesh2.GetBlendShapeWeight(0) > 99)
            {
                WaffleStep3();
            }
            blendWaffle1 = false;
            blendWaffle2 = false;
        }


    }







    private void SetTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OffSign();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == GetComponent<Collider>())

                {
                    if (readyToDeliver)
                    {
                        readyToDeliver = false;
                        if (_type == 1)
                        {
                            CookPizza();
                        }
                        else
                        {
                            DeliverOrder();
                        }
                    }
                    else
                    {
                        if (_type == 1)
                        {
                            if (_pizzaStep == 0)
                            {
                                if (_pizzaMesh1.GetBlendShapeWeight(0) == 0)
                                {
                                    PizzaStep1();
                            
                                }

                            }
                            else if (_pizzaStep == 1)
                            {
                                if (_pizzaMesh1.GetBlendShapeWeight(0) >= 100)
                                {
                            

                                    PizzaStep2();
                                }

                            }
                        }
                        else if (_type == 2)
                        {
                            if (_waffleStep == 0)
                            {
                                if (_waffleMesh1.GetBlendShapeWeight(0) >= 100)
                                {
                                    WaffleStep1();
                                }

                            }
                            //else if (_waffleStep == 1)
                            //{
                            //    if (_waffleMesh2.GetBlendShapeWeight(0) == 0)
                            //    {
                            //        WaffleStep2();
                            //    }

                            //}
                        }




                    }

                }


            }
        }
    }



    private void BlendWorks()
    {
        if (_type == 1)
        {



            if (blendPizza1)
            {


                if (_pizzaMesh1.GetBlendShapeWeight(0) < 100)
                {
                    blendValue += Time.deltaTime * 100;
                    _pizzaMesh1.SetBlendShapeWeight(0, blendValue);



                }
                if (_roller.transform.localPosition.z <= 0.5f)
                {
                    _roller.transform.localPosition += Vector3.forward * Time.deltaTime;
                }
                else
                {
               
               

                    _roller.SetActive(false);
                }

            }
            else if (blendPizza2)
            {
                if (_pizzaMesh2.GetBlendShapeWeight(0) > 0)
                {
                    blendValue -= Time.deltaTime * 200;
                    _pizzaMesh2.SetBlendShapeWeight(0, blendValue);
                }


            }

        }

        if (_type == 2)
        {
            if (blendWaffle1)
            {


                if (_waffleMesh1.GetBlendShapeWeight(0) >= 0)
                {
                    blendValue -= Time.deltaTime * 100;
                    _waffleMesh1.SetBlendShapeWeight(0, blendValue);



                }
                else
                {
                    CookWaffle();
                }


            }
            else if (blendWaffle2)
            {
                if (_waffleMesh2.GetBlendShapeWeight(0) <= 100)
                {
                    blendValue += Time.deltaTime * 100;
                    _waffleMesh2.SetBlendShapeWeight(0, blendValue);



                }
                else
                {
                    WaffleStep3();
                }
            }



        }
    }
    #endregion

    #region Sing Settings
    private void SetSign(Image sing)
    {
        sing.enabled = true;
    }


    public void SetDragSign()
    {
        _dragSign2.enabled = true;
    }

    public void OffSign()
    {
        _scrollSign.enabled = false;
        _dragSign.enabled = false;
        _dragSign2.enabled = false;
        _infSign.enabled = false;
        _tapSign.enabled = false;
        _tapSign2.enabled = false;
    }


    #endregion

}



