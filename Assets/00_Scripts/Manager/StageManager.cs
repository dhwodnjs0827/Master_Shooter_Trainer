using System;
using DataDeclaration;
using UnityEngine;
using UnityEngine.Analytics;

public class StageManager : SingletonBehaviour<StageManager>
{
    public bool IsGamePause { get; set; }
    private float remainTime;
    
    private float quickShotTimer;

    public Player Player { get; private set; }
    public AnalyticsManager AnalyticsManager { get; private set; }
    public int GameScore { get; set; }
    public float RemainTime
    {
        get => Math.Max(0f, remainTime);
        set => remainTime = value;
    }
    
    public int DestroyTargetCombo { get; set; }
    public int MaxDestroyTargetCombo { get; set; }
    public bool IsQuickShot { get; set; }
    public float QuickShotTimer
    {
        set => quickShotTimer = value;
    }
    public int ShotCount { get; set; }
    public int HitCount { get; set; }
    public int HeadHitCount { get; set; }

    private float shotAccuracy;
    private float headShotAccuracy;

    bool isGameOver = false;
    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();
        AnalyticsManager = GetComponent<AnalyticsManager>();    
        IsGamePause = true;
        RemainTime = Constants.INIT_STAGE_TIME;
        IsQuickShot = false;
    }

    private void Update()
    {
        if (!IsGamePause)
        {
            remainTime -= Time.deltaTime;
        }

        if (IsQuickShot)
        {
            quickShotTimer += Time.deltaTime;
            if (quickShotTimer >= Constants.QUICK_SHOT_TIME)
            {
                quickShotTimer = 0f;
                IsQuickShot = false;
                Debug.Log("큇샷 시간 초과");
            }
        }

        if (remainTime <= 0 && !isGameOver)
        {
            GameOver();
            isGameOver = true;
        }
       
    }

    public void GameOver()
    {
        UIManager.ToggleMouseCursor(true);

        UIManager.Instance.InitMainUI<ResultUI>();
        var resultUI = UIManager.Instance.CurMainUI as ResultUI;
        if (resultUI != null)
        {
            shotAccuracy = ShotCount == 0 ? 0 : (float)HitCount / ShotCount * 100f;
            headShotAccuracy = HitCount == 0 ? 0 : (float)HeadHitCount / HitCount * 100f;
            
            resultUI.SetResultValue(GameScore, RemainTime,
                shotAccuracy, headShotAccuracy, MaxDestroyTargetCombo);
        }
        AnalyticsManager.EndGame();
        Player.Controller.enabled = false;
    }

    /// <summary>
    /// 캐릭터 생성
    /// </summary>
    public void SpawnPlayer()
    {
        //var resource = ResourceManager.Instance.Load<Player>("Prefabs/Player/Player");
        var resource = ResourceManager.Instance.LoadAsset<Player>("Prefabs/Player/Player.prefab");
        Player = Instantiate(resource, new Vector3(0, 1, 3), Quaternion.identity);
    }
}