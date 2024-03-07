using TMPro;
using UnityEngine;

public class ButtonItem : MonoBehaviour
{
    public string Id => buttonData?.id ?? "";

    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Animation updateAnimation;

    private ButtonData buttonData;

    public void Initialize(ButtonData data, bool updateFlag = false)
    {
        buttonData = data;

        idText.text = buttonData.id;
        nameText.text = buttonData.text;

        if (updateFlag) updateAnimation.Play();
    }
}
