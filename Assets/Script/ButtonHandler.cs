using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public ButtonData buttonData;
    private Button button;
    private int playerCoins;

    void Start()
    {
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
            UImanager.instance.play(); // Call the Play method on the UIManager
        }
        else
        {
            Debug.Log("Insufficient coins"); // Show insufficient coins message
        }
    }
}