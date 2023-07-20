using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuTabManager : MonoBehaviour
{



    [SerializeField] GameObject _tab;


    [SerializeField] GameObject _vibOnButton;
    [SerializeField] GameObject _vibOffButton;

    [SerializeField] GameObject _soundOnButton;
    [SerializeField] GameObject _soundOffButton;

    [SerializeField] GameObject _fogOnButton;
    [SerializeField] GameObject _fogOffButton;


    public static MenuTabManager instance;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TabOn()
    {
        _tab.SetActive(true);
    }

    public void TabOff()
    {
        _tab.SetActive(false);
    }


    public void VibOn()
    {
        GameManager.instance.vibration = true;

        _vibOffButton.SetActive(true);
        _vibOnButton.SetActive(false);
    }

    public void VibOff()
    {
        GameManager.instance.vibration = false;

        _vibOffButton.SetActive(false);
        _vibOnButton.SetActive(true);
    }


    public void SoundOn()
    {
        Camera.main.GetComponent<AudioListener>().enabled = true;

        _soundOffButton.SetActive(true);
        _soundOnButton.SetActive(false);
    }

    public void SoundOff()
    {
        Camera.main.GetComponent<AudioListener>().enabled = false;

        _soundOffButton.SetActive(false);
        _soundOnButton.SetActive(true);
    }


    public void FogOn()
    {

        RenderSettings.fog = true;

        _fogOffButton.SetActive(true);
        _fogOnButton.SetActive(false);
    }

    public void FogOff()
    {

        RenderSettings.fog = false;

        _fogOffButton.SetActive(false);
        _fogOnButton.SetActive(true);


    }


}
