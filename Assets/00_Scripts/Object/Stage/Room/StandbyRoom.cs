using System.Collections;
using DataDeclaration;
using UnityEngine;

public class StandbyRoom : Room
{
    private bool isItemRewardComplete;
    
    protected override void Awake()
    {
        base.Awake();
        isItemRewardComplete = true;
        if (endPoint == null)
        {
            endPoint = transform.FindDeepChildByName("EndPoint");
        }
        
        exitGate.Door.OpenDoor += OpenDoor;
        exitGate.Door.DoorClosed += ResetRoom;
        exitGate.OnPassingGate += ExitRoom;
        enterGate.OnPassingGate += EnterRoom;
    }
    
    protected override void OpenDoor()
    {
        base.OpenDoor();
        StageManager.Instance.IsGamePause = false;
    }

    protected override void EnterRoom()
    {
        base.EnterRoom();
        StageManager.Instance.IsStandByRoom = true;
        StageManager.Instance.IsGamePause = true;
        StartCoroutine(OpenRewardUI(1f));
    }

    protected override void ResetRoom()
    {
        isItemRewardComplete = false;
        StartCoroutine(DisableRoom(1f));
    }

    public override bool CanOpenDoor()
    {
        return isItemRewardComplete;
    }

    private IEnumerator DisableRoom(float time)
    {
        yield return new WaitForSeconds(time);
        enterGate.Door.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private IEnumerator OpenRewardUI(float time)
    {
        yield return new WaitForSeconds(time);
        var popupReward = UIManager.Instance.OpenPopupUI<PopupReward>();
        popupReward.OnClose += () => { isItemRewardComplete = true; };
        StageManager.Instance.Player.Controller.enabled = false;
        UIManager.ToggleMouseCursor(true);
    }
}