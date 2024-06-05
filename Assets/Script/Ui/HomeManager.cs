using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private Animator OneMenu;
    [SerializeField] private Animator FourMenu;
    [SerializeField] private Animator EightMenu;
    [SerializeField] private GameObject OneVSOneMenu;
    [SerializeField] private GameObject OneVSFourMenu;
    [SerializeField] private GameObject OneVSEightMenu;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private Text[] coinsText;

    void Start()
    {
        UpdateCoinsText();
        OneVSOneMenu.SetActive(false);
        OneVSFourMenu.SetActive(false);
        OneVSEightMenu.SetActive(false);
    }

    void UpdateCoinsText()
    {
        string coinValue = managerdata.manager.Getcoin().ToString();

        foreach (Text coinsText in coinsText)
        {
            coinsText.text = coinValue;
        }
    }

    public void Open1v1()
    {
        OneVSOneMenu.SetActive(true);
        MainMenu.SetActive(false);
        OneMenu.SetTrigger("Open");
    }

    public void Open1v4()
    {
        OneVSFourMenu.SetActive(true);
        MainMenu.SetActive(false);
        FourMenu.SetTrigger("Open");
    }

    public void Open1v8()
    {
        OneVSEightMenu.SetActive(true);
        MainMenu.SetActive(false);
        EightMenu.SetTrigger("Open");
    }

    public void Close1v1()
    {
        OneMenu.SetTrigger("Close");
        FourMenu.SetTrigger("Close");
        EightMenu.SetTrigger("Close");
        StartCoroutine(MenuClose());
    }

    IEnumerator MenuClose()
    {
        yield return new WaitForSeconds(1f);
        MainMenu.SetActive(true);
        OneVSOneMenu.SetActive(false);
        OneVSFourMenu.SetActive(false);
        OneVSEightMenu.SetActive(false);
    }
}
