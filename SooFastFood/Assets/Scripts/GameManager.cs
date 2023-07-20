using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Taptic;

public class GameManager : MonoBehaviour
{


    public bool vibration;
    public bool vib;

    [SerializeField] GameObject[] CustomerFoods;
    [SerializeField] GameObject[] Faces;

    [SerializeField] Animator _animator;
    [SerializeField] Vector3 _camRotFirst;
    [SerializeField] Vector3 _camRotCustomer;


    [SerializeField] float _money;
    [SerializeField] float _currentMoney;
    [SerializeField] float _foodCost;

    [Header("UI Settings")]

    [SerializeField] GameObject _nextTab;
    [SerializeField] GameObject _moneyParticle;
    [SerializeField] TMP_Text _moneyText;
    [SerializeField] Image _moneyBlob;

    public static GameManager instance;

    public bool moneyUp;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

        _camRotFirst = Camera.main.transform.eulerAngles;
        StartCoroutine(MoneyBlob());
    }

    // Update is called once per frame
    void Update()
    {

        if (_money<_currentMoney-1)
        {
            _moneyBlob.enabled = true;
            _moneyParticle.SetActive(true);
            moneyUp = true;
            _money = Mathf.Lerp(_money, _currentMoney, Time.deltaTime );
        }
        else
        {
            _moneyBlob.enabled = false;
            _moneyParticle.SetActive(false);
            moneyUp = false;
            _money = _currentMoney;
        }

        _moneyText.text = ((int)_money).ToString();
    }


    public void MoneyUp(float count) {
        _currentMoney += _foodCost*count;
    }

  

    IEnumerator MoneyBlob()
    {
        while (true)
        {
            _moneyBlob.transform.DOScale(Vector3.one * 1.2f, 0.1f);
            yield return new WaitForSeconds(0.1f);
            _moneyBlob.transform.DOScale(Vector3.one, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
    }


    public void NextOrder(float count)
    {

        int result = 0;


        int res = FoodBaseManager.instance.totalFood;
        int score = (int)(FoodBaseManager.instance.totalTrueFood*0.5f);

        if (score>=res)
        {
            result = 1;
        }
     




        
        Customer(count,result);
    }


    #region Customer Setting

    public void Customer(float count, int result)
    {

        CustomerFoods[QuestManager.instance.currentQuest.foodBaseIndex].SetActive(true);
    


        StartCoroutine(CustomerReact(count, result));

      
    }



    IEnumerator CustomerReact(float count, int result)
    {

        Camera.main.transform.DORotate(_camRotCustomer, 0.4f);
        yield return new WaitForSeconds(0.4f);
        _animator.SetTrigger("Eat");
        SetFaces(1);
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < CustomerFoods.Length; i++)
        {
            CustomerFoods[i].SetActive(false);
        }

        if (result==1)
        {
            SetFaces(2);
            _animator.SetTrigger("Happy");
        }
        else
        {
            SetFaces(3);
            _animator.SetTrigger("Vomit");
        }
        MoneyUp(count);
        yield return new WaitForSeconds(4f);
        SetFaces(0);
     
        Camera.main.transform.DORotate(_camRotFirst, 0.4f);
        yield return new WaitForSeconds(0.4f);
        _nextTab.SetActive(true);
    }

    private void SetFaces(int index)
    {
        for (int i = 0; i < Faces.Length; i++)
        {
            Faces[i].SetActive(false);
        }

        Faces[index].SetActive(true);

    }

    public void CloseTab(GameObject tab)
    {
        tab.SetActive(false);
    }


    #endregion


    #region Vibrate Setting

    public void Vibrate()
    {

        if (vibration)
        {



            if (!vib)
            {
                Debug.Log("vib");


                vib = true;
                Vibration.Vibrate(30, 70, true);
                StartCoroutine(VibDelay());
            }


        }

    }

    IEnumerator VibDelay()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        vib = false;
    }


    public void VibrateMed()
    {
        if (vibration)
        {

            if (!vib)
            {
                Debug.Log("vib");


                vib = true;
                Vibration.Vibrate(70, 170, true);
                StartCoroutine(VibDelayMed());
            }

        }


    }

    IEnumerator VibDelayMed()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        vib = false;
    }



    public void VibrateHigh()
    {
        if (vibration)
        {

            if (!vib)
            {
                Debug.Log("vib");


                vib = true;
                Vibration.Vibrate(70, 255, true);
                StartCoroutine(VibDelayHigh());
            }



        }
    }

    IEnumerator VibDelayHigh()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        vib = false;
    }

    #endregion



}
