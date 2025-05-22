using System.Collections;
using System.Collections.Generic;
using DataDeclaration;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MainUI
{
    #region UI Object

    [Header("스코어 관련")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private Transform scorePosition;
    
    [SerializeField] private TextMeshProUGUI remainTimeText;
    [SerializeField] private TextMeshProUGUI curStageText;
    [SerializeField]
    private TextMeshProUGUI comboText
    ;
    [SerializeField] private Image[] stageIndex;
    [Space][SerializeField] private Image equipImage;
    [SerializeField] private TextMeshProUGUI ammoText;
    [Space][SerializeField] private Image spdGauge;
    [SerializeField] private TextMeshProUGUI spdText;
    [SerializeField] private Image hdlGauge;
    [SerializeField] private TextMeshProUGUI hdlText;
    [SerializeField] private Image rclGauge;
    [SerializeField] private TextMeshProUGUI rclText;
    [SerializeField] private Image stpGauge;
    [SerializeField] private TextMeshProUGUI stpText;
    
    [Header("도전과제")]
    [SerializeField] private Transform achivmentPoint;
    [SerializeField] private GameObject achivemnetPrefab;

    #endregion

    private Color32 originalColor;
    private Color32 currentColor;

    public override MainUIType UIType { get; protected set; }

    private void Awake()
    {
        UIType = MainUIType.HUD;

        originalColor = new Color32(221, 234, 249, 255);
        currentColor = new Color32(252, 192, 1, 255);
    }

    private void Start()
    {
        RoomManager.Instance.RoomChangedAction += UpdateStageInfo;
        UpdateStageInfo();
        UpdateStatValue();

        var weapon = GameManager.Instance.selectedWeapon;
        var image = ResourceManager.Instance.Load<Sprite>($"Sprites/{weapon.ID}");
        SetEquipImage(image);
    }

    private void Update()
    {
        if (StageManager.Instance.Player)
        {
            UpdateRealTimeChanges();
        }
    }

    public override void SetActiveUI(MainUIType activeUIType)
    {
        gameObject.SetActive(UIType == activeUIType);
    }

    /// <summary>
    /// 실시간의로 변경되는 UI 갱신 메서드
    /// </summary>
    private void UpdateRealTimeChanges()
    {
        scoreText.text = $"<size=36>Score</size>\n<size=80>{StageManager.Instance.GameScore:D6}</size>";
        remainTimeText.text = $"<size=120>{StageManager.Instance.RemainTime:N2}</size>";
        ammoText.text = $"{StageManager.Instance.Player.Weapon.CurAmmo} / {StageManager.Instance.Player.Weapon.MaxAmmo}";
        comboText.text = $"<size=36>Combo</size>\n<size=80>{StageManager.Instance.DestroyTargetCombo}</size>";
    }

    /// <summary>
    /// Stage 정보 UI 갱신 메서드
    /// </summary>
    private void UpdateStageInfo()
    {
        curStageText.text = $"Stage {RoomManager.Instance.CurStageIndex}";

        for (var i = 1; i <= stageIndex.Length; i++)
        {
            if (RoomManager.Instance.CurRoomIndex == 0)
            {
                stageIndex[i - 1].color = originalColor;
            }
            else
            {
                stageIndex[i - 1].color = currentColor;
                stageIndex[i - 1].color = (i == RoomManager.Instance.CurRoomIndex) ? currentColor : originalColor;
            }
        }
    }

    /// <summary>
    /// 점수 올라갈때 Score 띄우는 메서드
    /// </summary>
    public void ShowScoreEffect(bool isHeadShot, int headShotScore, int comboScore, int quickShotScore, int rangeScore)
    {
        if (scorePrefab == null || scorePosition == null) return;

        StartCoroutine(SpawnScoreTexts());

        IEnumerator SpawnScoreTexts()
        {
            List<(string label, int score)> scoreItems = new();

            string hitLabel = isHeadShot ? "Head Shot" : "Body Shot";
            scoreItems.Add((hitLabel, headShotScore));

            if (comboScore > 0)
                scoreItems.Add(("Combo Bonus", comboScore));

            if (quickShotScore > 0)
                scoreItems.Add(("Quick Shot Bonus", quickShotScore));

            if (rangeScore > 0)
                scoreItems.Add(("Range Bonus", rangeScore));

            foreach (var item in scoreItems)
            {
                CreateScoreText(item.label, item.score);
                yield return new WaitForSeconds(0.2f);
            }
        }

        void CreateScoreText(string label, int score)
        {
            var go = ObjectPoolManager.Instance.GetObject(scorePrefab, Vector3.zero, Quaternion.identity, 2f);
            go.transform.SetParent(scorePosition, false);

            go.transform.localRotation = Quaternion.identity;

            var text = go.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = $"{label} +{score:0000}";
            }
        }
    }







    /// <summary>
    /// 캐릭터 스탯 UI 갱신 메서드
    /// </summary>
    public void UpdateStatValue()
    {
        var stat = StageManager.Instance.Player.Stat;

        rclGauge.fillAmount = stat.RCL / Constants.MAX_STAT;
        rclText.text = stat.RCL.ToString("N0");

        hdlGauge.fillAmount = stat.HDL / Constants.MAX_STAT;
        hdlText.text = stat.HDL.ToString("N0");

        stpGauge.fillAmount = stat.STP / Constants.MAX_STAT;
        stpText.text = stat.STP.ToString("N0");

        spdGauge.fillAmount = stat.SPD / Constants.MAX_STAT;
        spdText.text = stat.SPD.ToString("N0");
    }

    /// <summary>
    /// 캐릭터가 장착 중인 무기 이미지 UI 갱신 메서드
    /// </summary>
    public void SetEquipImage(Sprite weaponImage)
    {
        equipImage.sprite = weaponImage;
    }
}