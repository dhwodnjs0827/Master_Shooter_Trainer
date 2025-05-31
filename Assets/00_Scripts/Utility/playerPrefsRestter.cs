using UnityEngine;

public class PlayerPrefsResetter : MonoBehaviour
{
    /// <summary>
    /// 아무 오브젝트에 아 컴포넌트를 붙인다
    /// 오른쪽위 점 세개를 누른다
    /// 초기화 버튼 클릭
    /// PlayerPrefs초기화
    /// </summary>
    [ContextMenu("초기화 - PlayerPrefs DeleteAll")]
    void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs가 전부 삭제되었습니다.");
    }
}
