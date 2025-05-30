using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InteractManager : SingletonBehaviour<InteractManager>
{
    private PopupInteract interactPopup;
    private PopupInteract reloadPopup;

    public void SpawnInteractItem()
    {
        if (interactPopup == null)
        {
            interactPopup = UIManager.Instance.OpenPopUpUIMultiple<PopupInteract>();
        }
        interactPopup.SetText("E", "문 열기");

        var rect = interactPopup.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = new Vector2(-700f, 500f);
        }
    }

    public void SpawnReloadItem()
    {
        if (reloadPopup == null)
        {
            reloadPopup = UIManager.Instance.OpenPopUpUIMultiple<PopupInteract>();
        }
        reloadPopup.SetText("R", "재장전");
        var rect = reloadPopup.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = new Vector2(-249f, 257f);
        }
    }

    public void CloseInteractItem()
    {
        if (interactPopup != null)
        {
            UIManager.Instance.ClosePopUpUI(interactPopup);
            interactPopup = null;
        }
    }


    public void CloseReloadItem()
    {
        if (reloadPopup != null)
        {
            UIManager.Instance.ClosePopUpUI(reloadPopup);
            reloadPopup = null;
        }
        
    }
}
