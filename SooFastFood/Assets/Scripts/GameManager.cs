using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

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

    public void Customer(float count, int result)
    {



        StartCoroutine(CustomerReact(count, result));

      
    }



    IEnumerator CustomerReact(float count, int result)
    {

        Camera.main.transform.DORotate(_camRotCustomer, 0.4f);
        yield return new WaitForSeconds(0.4f);
        _animator.SetTrigger("Eat");
        yield return new WaitForSeconds(2f);
        if (result==1)
        {
            _animator.SetTrigger("Happy");
        }
        else
        {
            _animator.SetTrigger("Vomit");
        }
        MoneyUp(count);
        yield return new WaitForSeconds(4f);
        Camera.main.transform.DORotate(_camRotFirst, 0.4f);
        yield return new WaitForSeconds(0.4f);
        _nextTab.SetActive(true);
    }



    public void CloseTab(GameObject tab)
    {
        tab.SetActive(false);
    }

}
