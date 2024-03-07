using TMPro;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Server server;

    [SerializeField] private ButtonItem buttonItem;
    [SerializeField] private Transform buttonsContent;

    [SerializeField] private CanvasGroup deletePopup;
    [SerializeField] private TMP_InputField deletePopupInputId;

    [SerializeField] private CanvasGroup updatePopup;
    [SerializeField] private TMP_InputField updatePopupInputId;
    [SerializeField] private TMP_InputField updatePopupInputName;

    [SerializeField] private CanvasGroup refreshPopup;
    [SerializeField] private TMP_InputField refreshPopupInputId;

    private List<ButtonItem> buttons = new List<ButtonItem>();

    public void OpenPopup(CanvasGroup popup)
    {
        popup.DOFade(1, 0.3f);
        popup.blocksRaycasts = true;
        var block = popup.transform.GetChild(1);
        block.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        DOTween.Sequence().Append(block.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.2f))
            .Append(block.DOScale(new Vector3(1f, 1f, 1f), 0.1f));
    }

    public void ClosePopup(CanvasGroup popup)
    {
        popup.DOFade(0, 0.3f);
        popup.blocksRaycasts = false;
        var block = popup.transform.GetChild(1);
        DOTween.Sequence().Append(block.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.1f))
            .Append(block.DOScale(new Vector3(0.8f, 0.8f, 1f), 0.2f));
    }

    public void Create()
    {
        server.CreateButton(AddButton);
    }

    public void OpenDeletePopup()
    {
        OpenPopup(deletePopup);
    }

    public void DeleteButton()
    {
        var id = deletePopupInputId.text;
        deletePopupInputId.text = "";
        if (string.IsNullOrEmpty(id)) return;
        server.DeleteButton(id, () => DeleteButton(id));
        ClosePopup(deletePopup);
    }

    public void OpenUpdatePopup()
    {
        OpenPopup(updatePopup);
    }

    public void UpdateButton()
    {
        var id = updatePopupInputId.text;
        updatePopupInputId.text = "";
        if (string.IsNullOrEmpty(id)) return;
        var name = updatePopupInputName.text;
        updatePopupInputName.text = "";

        server.UpdateButton(id, name, UpdateButton);

        ClosePopup(updatePopup);
    }

    public void OpenRefreshPopup()
    {
        OpenPopup(refreshPopup);
    }

    public void RefreshButton()
    {
        var id = refreshPopupInputId.text;
        refreshPopupInputId.text = "";

        if (string.IsNullOrEmpty(id))
        {
            server.RefreshButtons(buttons => buttons.ForEach(UpdateButton));
        }
        else
        {
            server.RefreshButton(id, UpdateButton);
        }

        ClosePopup(refreshPopup);
    }

    private void AddButton(ButtonData buttonData)
    {
        var newButtonItem = Instantiate(buttonItem, buttonsContent);
        newButtonItem.Initialize(buttonData);
        buttons.Add(newButtonItem);
    }

    private void DeleteButton(string id)
    {
        for (var i = 0; i < buttons.Count; i++)
        {
            if (!buttons[i].Id.Equals(id)) continue;
            var item = buttons[i];
            buttons.Remove(item);
            Destroy(item.gameObject);
            break;
        }
    }

    private void UpdateButton(ButtonData buttonData)
    {
        for (var i = 0; i < buttons.Count; i++)
        {
            if (!buttons[i].Id.Equals(buttonData.id)) continue;
            buttons[i].Initialize(buttonData, true);
            break;
        }
    }
}
