using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SignState
{
    SignIn,
    SignUp,
}

public class PopupAuth : PopupUI, IPointerEnterHandler
{
    [SerializeField] private Button closeButton;
    
    [SerializeField] private Text titleText;
    
    [SerializeField] private InputField idInputField;
    [SerializeField] private InputField passwordInputField;
    
    [SerializeField] private Text nameText;
    [SerializeField] private InputField nameInputField;
    
    [SerializeField] private Text signInText;
    [SerializeField] private Button signInButton;
    [SerializeField] private Button signUpButton;
    
    private List<InputField> inputFieldList;
    private int curInputIdx;

    private SignState curSignState;

    private void Awake()
    {
        curSignState = SignState.SignIn;
        
        InitInputField();
        curInputIdx = 0;

        closeButton.onClick.AddListener(ChangeSignState);
        
        signInButton.onClick.AddListener(SignIn);
        signUpButton.onClick.AddListener(ChangeSignState);
        
#if UNITY_EDITOR
        idInputField.text = "Admin";
        passwordInputField.text = "Admin123!";
#endif
    }

    private void Update()
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            CloseUI();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            var maxIdx = inputFieldList.Count;
            curInputIdx = (curInputIdx + 1) % maxIdx;
            inputFieldList[curInputIdx].Select();
        }
    }

    private void InitInputField()
    {
        // ID inputField 초기화
        idInputField.characterLimit = 20;
        idInputField.onEndEdit.AddListener(ValidateString.ValidateID);
        
        // 비밀번호 inputField 초기화
        passwordInputField.characterLimit = 30;
        passwordInputField.onEndEdit.AddListener(ValidateString.ValidatePassword);
        
        nameText.gameObject.SetActive(false);
        nameInputField.gameObject.SetActive(false);

        inputFieldList = new List<InputField>
        {
            idInputField,
            passwordInputField
        };
    }
    
    private async void SignIn()
    {
        try
        {
            await UserManager.Instance.SignInWithUsernamePasswordAsync(idInputField.text, passwordInputField.text);
            idInputField.text = "";
            passwordInputField.text = ""; 
        }
        catch (AuthenticationException)
        {
            var ui = UIManager.Instance.OpenPopupUI<PopupNotice>();
            ui.SetContentText("로그인 실패", "아이디 또는 비밀번호를 확인해주세요", "취소", "확인");
        }
        catch (RequestFailedException ex)
        {
            var ui = UIManager.Instance.OpenPopupUI<PopupNotice>();
            ui.SetContentText("로그인 실패", "네트워크 오류: 서버 접속 실패", "취소", "확인");
        }
        catch (Exception e)
        {
            var ui = UIManager.Instance.OpenPopupUI<PopupNotice>();
            ui.SetContentText("로그인 실패", e.Message, "취소", "확인");
        }
    }

    private async void SignUp()
    {
        try
        {
            await UserManager.Instance.SignUpWithUsernamePasswordAsync(idInputField.text, passwordInputField.text, nameInputField.text);
            idInputField.text = null;
            passwordInputField.text = null;
            nameInputField.text = null;
            Debug.Log("회원가입 성공");
        }
        catch(Exception e)
        {
            var ui = UIManager.Instance.OpenPopupUI<PopupNotice>();
            ui.SetContentText("회원가입 실패", e.Message, "취소", "확인");
            Debug.Log("회원가입 실패");
        }
    }

    private void ChangeSignState()
    {
        if (curSignState == SignState.SignIn)
        {
            curSignState = SignState.SignUp;

            closeButton.gameObject.SetActive(true);
            
            titleText.text = "회원가입";
            
            idInputField.text = null;
            passwordInputField.text = null;
            
            nameText.gameObject.SetActive(true);
            nameInputField.gameObject.SetActive(true);
            
            signInText.text = "회원가입";
            signInButton.onClick.RemoveAllListeners();
            signInButton.onClick.AddListener(SignUp);
            
            signUpButton.gameObject.SetActive(false);
            
            inputFieldList.Add(nameInputField);
        }
        else
        {
            curSignState = SignState.SignIn;
            
            closeButton.gameObject.SetActive(false);
            
            titleText.text = "로그인";
            
            nameText.gameObject.SetActive(false);
            nameInputField.gameObject.SetActive(false);
            
            signInText.text = "로그인";
            signInButton.onClick.RemoveAllListeners();
            signInButton.onClick.AddListener(SignIn);
            
            signUpButton.gameObject.SetActive(true);

            inputFieldList.Remove(nameInputField);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }
}
