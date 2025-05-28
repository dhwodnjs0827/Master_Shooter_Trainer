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
        interactPopup.SetText("E", "Open Door");
    }

    public void SpawnReloadItem()
    {
        if (reloadPopup == null)
        {
            reloadPopup = UIManager.Instance.OpenPopUpUIMultiple<PopupInteract>();
        }
        reloadPopup.SetText("R", "Reload");
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
