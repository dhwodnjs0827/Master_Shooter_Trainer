using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;

public class RankDisplayUI : MonoBehaviour
{
    [SerializeField] private Text userName;
    [SerializeField] private Text scoreText;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Text rankText;
    [SerializeField] private Image characterImage;

    public void SetUI(int index, DocumentSnapshot data)
    {
        userName.text = data.GetValue<string>("UserName");
        scoreText.text = data.GetValue<int>("BestScore").ToString();
        weaponImage.sprite = ResourceManager.Instance.Load<Sprite>($"Sprites/{data.GetValue<string>("Weapon")}");
        rankText.text = index.ToString();
        characterImage.sprite = ResourceManager.Instance.Load<Sprite>($"Sprites/{data.GetValue<string>("Character")}");
    }
}
