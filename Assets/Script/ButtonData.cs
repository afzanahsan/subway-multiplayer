using UnityEngine;

[CreateAssetMenu(fileName = "NewButtonData", menuName = "Buttons/ButtonData", order = 1)]
public class ButtonData : ScriptableObject
{
    public string buttonName;
    public int price;
    public int reward;
}
