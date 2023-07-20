using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class QuestManager : MonoBehaviour
{

    [SerializeField] GameObject[] FoodPngs1;
    [SerializeField] GameObject[] FoodPngs2;
    [SerializeField] GameObject[] FoodPngs3;
    [SerializeField] GameObject[] FoodPngs4;

    [SerializeField] GameObject[] QuestTabs;
    [SerializeField] GameObject[] QuestDoneTabs;
    [SerializeField] TMP_Text[] QuestCountText;

    [SerializeField] GameObject _missionCompleteTab;
    [SerializeField] GameObject _missionFailedTab;
    [SerializeField] Image _timerTab;
    [SerializeField] Image _timerHurryTab;
    [SerializeField] TMP_Text _timeText;
    [SerializeField] TMP_Text _timeHurryText;

    [SerializeField] GameObject[] QuestPngs;
    [SerializeField] TMP_Text[] CountTexts;

    [SerializeField] Quest[] Quests;
    public bool missionStart;
    public bool missionOpened;
    public bool missionEnd;
    //public bool missionComplete;
    //public bool missionFailed;
    public Quest currentQuest;
    public int questValue;
    public float _time;
    public float _curretTime;
    

    public static QuestManager instance;

    // Start is called before the first frame update



    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetQuestFirst();
    }

    // Update is called once per frame
    void Update()
    {
        Timers();

  

    }

    #region Quest Settings

    public void SetQuestFirst()
    {
        _curretTime = _time;
        missionOpened = true;
        StartCoroutine(HurryBlob());
        currentQuest = Quests[questValue];
        FoodBaseManager.instance.SetFoodBase(currentQuest.foodBaseIndex);
    }

    public void SetQuest()
    {
        missionEnd = false;
        missionOpened = true;
        _missionCompleteTab.SetActive(false);
        _missionFailedTab.SetActive(false);
        missionStart = false;

        FoodBaseManager.instance.ResetBase();
        currentQuest = Quests[questValue];
        FoodTabsManager.instance.TabsOn();
        FoodBaseManager.instance.SetFoodBase(currentQuest.foodBaseIndex);

        Debug.Log(currentQuest.TotalCount());
    }


    public void FinishQuest()
    {
        questValue += 1;
        if (questValue>=Quests.Length)
        {
            questValue = 0;
        }

        StartCoroutine(FinishingQuest());
    }


    IEnumerator FinishingQuest()
    {
        yield return new WaitForSeconds(0.5f);
        //SetQuest();
        GameManager.instance.NextOrder(FoodBaseManager.instance.totalTrueFood);
    }



    [Serializable]
    public class Quest
    {
        public int foodBaseIndex;
        public int[] FoodIndexes;
        public int[] FoodCounts;


        public int TotalCount()
        {

            int t = 0;

            for (int i = 0; i < FoodCounts.Length; i++)
            {
                t += FoodCounts[i];
            }

            return t;
        }
    }

    #endregion





    public void TabBlob(Transform tab)
    {
        StartCoroutine(BlobingTab(tab));
    }

    IEnumerator BlobingTab(Transform tab)
    {
        tab.DOScale(Vector3.one * 1.4f,0.2f);
        yield return new WaitForSeconds(0.2f);
        tab.DOScale(Vector3.one , 0.1f);
        yield return new WaitForSeconds(2);
        tab.DOScale(Vector3.zero, 0.2f);
        yield return new WaitForSeconds(0.5f);
        FinishQuest();
      
    }


    #region Timer Setting

    public void Timers()
    {
        if (missionOpened)
        {
            if (!missionStart)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    missionStart = true;
                }
            }



            for (int i = 0; i < 4; i++)
            {
                if (FoodBaseManager.instance.currentQuestCounts[i]>0)
                {
                    QuestTabs[i].SetActive(true);
                    QuestCountText[i].text = FoodBaseManager.instance.currentQuestCounts[i].ToString();




                    switch (i)
                    {
                        case 0:

                            FoodPngs1[currentQuest.FoodIndexes[0]].SetActive(true);
                            break;
                        case 1:

                            FoodPngs2[currentQuest.FoodIndexes[1]].SetActive(true);
                            break;
                        case 2:

                            FoodPngs3[currentQuest.FoodIndexes[2]].SetActive(true);
                            break;
                        case 3:

                            FoodPngs4[currentQuest.FoodIndexes[3]].SetActive(true);
                            break;
                    }


                }
                else
                {
                    if (QuestTabs[i].activeSelf == true)
                    {
                        QuestDoneTabs[i].SetActive(true);

                        QuestCountText[i].text = "";

                    }


                }
            }








        }


        if (missionStart&&!missionEnd)
        {
            if (_curretTime > 10)
            {
                _timeText.enabled = true;
                _timerTab.enabled = true;
                _timeHurryText.enabled = false;
                _timerHurryTab.enabled = false;

                _curretTime -= Time.deltaTime;

                int minutes = Mathf.FloorToInt(_curretTime / 60);
                int seconds = Mathf.FloorToInt(_curretTime % 60);
                _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            }
            else 
            if (_curretTime > 0)  
            {
                _timeText.enabled = false;
                _timerTab.enabled = false;
                _timeHurryText.enabled = true;
                _timerHurryTab.enabled = true;

                _curretTime -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(_curretTime / 60);
                int seconds = Mathf.FloorToInt(_curretTime % 60);
                _timeHurryText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
                
            else
                
            {
                missionStart = false;
                    Mission(0);
                
            }

            }

    }



    public void Mission(int i)
    {



        for (int c = 0; c < 4; c++)
        {
            QuestTabs[c].SetActive(false);
            QuestDoneTabs[c].SetActive(false);
        }


        for (int c = 0; c < FoodPngs1.Length; c++)
        {
            FoodPngs1[c].SetActive(false);
            FoodPngs2[c].SetActive(false);
            FoodPngs3[c].SetActive(false);
            FoodPngs4[c].SetActive(false);
        }


        missionOpened = false;
        missionEnd = true;
        ResetTimer();
        if (i==1)
        {
            //_timeText.enabled = false;
            //_timerTab.enabled = false;
            //_timeHurryText.enabled = false;
            //_timerHurryTab.enabled = false;

            _missionCompleteTab.SetActive(true);
            TabBlob(_missionCompleteTab.transform);
        }
        else if (i == 0)
        {
            //_timeText.enabled = false;
            //_timerTab.enabled = false;
            //_timeHurryText.enabled = false;
            //_timerHurryTab.enabled = false;

            _missionFailedTab.SetActive(true);
            TabBlob(_missionFailedTab.transform);
        }
    }

    IEnumerator HurryBlob()
    {
        while (true)
        {
            _timerHurryTab.transform.DOScale(Vector3.one * 1.2f, 0.3f);
            yield return new WaitForSeconds(0.3f);
            _timerHurryTab.transform.DOScale(Vector3.one, 0.3f);
            yield return new WaitForSeconds(0.3f);
        }
    }



    public void ResetTimer()
    {
        _curretTime = _time;

        _timeText.enabled = true;
        _timerTab.enabled = true;
        _timeHurryText.enabled = false;
        _timerHurryTab.enabled = false;
        int minutes = Mathf.FloorToInt(_curretTime / 60);
        int seconds = Mathf.FloorToInt(_curretTime % 60);
        _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    #endregion


}
