using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GradientButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler, IPointerUpHandler
{
    public Image targetImage;
    public Material normalMaterial; // 색깔 없음
    public Material gradientMaterial; // 그라데이션 색깔
    public TextMeshProUGUI text;
    private bool isPressed = false;
    
    public void OnPointerEnter(PointerEventData eventData) // 버튼위에 포인터
    {
        targetImage.material = gradientMaterial;
        text.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData) // 버튼위에 커서가 벗어 났을 때 
    {
        targetImage.material = normalMaterial;
        if (isPressed)
        {
            text.color = Color.gray;
            return;
        }
        else
        {
            text.color = Color.white;
        }
    }

    public void OnPointerDown(PointerEventData eventData) // 클릭시 
    {
        //targetImage.material = normalMaterial;
        text.color = Color.gray;
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData) // 클릭을 뗐을 때 
    {
        isPressed = false;
        targetImage.material = normalMaterial;
        text.color = Color.white;
    }
}