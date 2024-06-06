using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonHandler : MonoBehaviour
{
    public ButtonData buttonData;
    private Button button;
    private int playerCoins;
    public GameObject messageObject;

    void Start()
    {
        messageObject.SetActive(false);
        button = GetComponent<Button>();
        playerCoins = managerdata.manager.Getcoin();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if (playerCoins >= buttonData.price)
        {
            playerCoins -= buttonData.price; // Deduct the price from player's coins
            PlayerPrefs.SetInt("coin", playerCoins);
            PlayerPrefs.Save();
            NetworkManager.instance.JoinRoom();
            //UImanager.instance.play(); // Call the Play method on the UIManager
        }
        else
        {
            messageObject.SetActive(true);
            StartCoroutine(DelayMessage());
            Debug.Log("Insufficient coins"); // Show insufficient coins message
        }
    }

    IEnumerator DelayMessage()
    {
        yield return new WaitForSeconds(1f);
        messageObject.SetActive(false);
    }
}