using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetKeyDownOnBoard : MonoBehaviour
{
    public TMP_InputField emailInput_Register, emailInput_Fogot;
    public TMP_InputField userInput_Login, userInput_Register;
    public TMP_InputField passwordInput_Login, passwordInput_Register;
    public TMP_InputField namePlayer;
    public TMP_InputField chatMessage, textShow;
    public Transform login, register, fogot;
    public GameObject keyBoard, keyBoardPlay;

    public bool isEmailFogot, isEmailRegister;
    public bool isUserLogin, isUserRegister;
    public bool isPassLogin, isPassRegister;
    public bool isNamePlayer, isUP;
    public bool isChatMess;
    [SerializeField] public bool loginOrPlay;
    public int status;

    void Start()
    {
        if (loginOrPlay)
        {
            // Đăng ký hàm xử lý cho sự kiện OnSelect của các input field
            emailInput_Register.onSelect.AddListener(delegate { OnInputFieldSelected(emailInput_Register); });
            emailInput_Fogot.onSelect.AddListener(delegate { OnInputFieldSelected(emailInput_Fogot); });
            userInput_Login.onSelect.AddListener(delegate { OnInputFieldSelected(userInput_Login); });
            userInput_Register.onSelect.AddListener(delegate { OnInputFieldSelected(userInput_Register); });
            passwordInput_Login.onSelect.AddListener(delegate { OnInputFieldSelected(passwordInput_Login); });
            passwordInput_Register.onSelect.AddListener(delegate { OnInputFieldSelected(passwordInput_Register); });
            namePlayer.onSelect.AddListener(delegate { OnInputFieldSelected(namePlayer); });
            // Đăng ký sự kiện onValueChanged cho mỗi TMP_InputField
            emailInput_Register.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(emailInput_Register); });
            emailInput_Fogot.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(emailInput_Fogot); });
            userInput_Login.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(userInput_Login); });
            userInput_Register.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(userInput_Register); });
            passwordInput_Login.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(passwordInput_Login); });
            passwordInput_Register.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(passwordInput_Register); });
            namePlayer.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(namePlayer); });
        }
        else
        {
            chatMessage.onSelect.AddListener(delegate { OnInputFieldSelected(chatMessage); });

            chatMessage.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(chatMessage); });
        }
    }
    void OnInputFieldValueChanged(TMP_InputField inputField)
    {
        // Kiểm tra nếu chiều dài văn bản vượt quá chiều dài của inputField
        if (inputField.preferredWidth > inputField.GetComponent<RectTransform>().rect.width)
        {
            // Di chuyển con trỏ đến cuối dòng
            Debug.Log("lon");
            inputField.caretPosition = inputField.text.Length;

            inputField.ForceLabelUpdate();
        }
    }
    void OnInputFieldSelected(TMP_InputField selectedInputField)
    {
        // Xử lý khi input field được chọn
        Debug.Log("Input field được chọn: " + selectedInputField.name);
        if (loginOrPlay) 
        {
            if(!keyBoard.activeSelf)
                keyBoard.SetActive(true);
            isEmailRegister = selectedInputField == emailInput_Register;
            isEmailFogot = selectedInputField == emailInput_Fogot;
            isUserLogin = selectedInputField == userInput_Login;
            isUserRegister = selectedInputField == userInput_Register;
            isPassLogin = selectedInputField == passwordInput_Login;
            isPassRegister = selectedInputField == passwordInput_Register;
            isNamePlayer = selectedInputField == namePlayer;
        } 
        else
        {
            if (!keyBoardPlay.activeSelf)
                keyBoardPlay.SetActive(true);
            isChatMess = selectedInputField == chatMessage;
        }
        if (isChatMess)
        {
            status = 4;
            textShow.text = chatMessage.text;
        }
        else if (isUserLogin || isPassLogin)
        {
            status = 1;
            login.position = new Vector3(0, 2, 90);
        }
        else if (isEmailFogot)
        {
            status = 2;
            fogot.position = new Vector3(0, 2, 90);
        }
        else
        {
            status = 3;
            register.position = new Vector3(0, 2, 90);
        }
    }

    public void getKeyOnBoard(string nameKey)
    {
        if (isChatMess)
        {
            if (nameKey == "DEL" && textShow.text.Length > 0)
            {
                textShow.text = textShow.text.Substring(0, textShow.text.Length - 1);
                chatMessage.text = textShow.text;
            }
            else
            {
                textShow.text += nameKey; 
                chatMessage.text = textShow.text;
            }
        }
        else if (isEmailFogot)
        {
            if (nameKey == "DEL" && emailInput_Fogot.text.Length > 0)
            {
                emailInput_Fogot.text = emailInput_Fogot.text.Substring(0, emailInput_Fogot.text.Length - 1);
            }
            else
            {
                emailInput_Fogot.text += nameKey;
            }
        }
        else if (isEmailRegister)
        {
            if (nameKey == "DEL" && emailInput_Register.text.Length > 0)
            {
                emailInput_Register.text = emailInput_Register.text.Substring(0, emailInput_Register.text.Length - 1);
            }
            else
            {
                emailInput_Register.text += nameKey;
            }
        }
        else if (isNamePlayer)
        {
            if (nameKey == "DEL" && namePlayer.text.Length > 0)
            {
                namePlayer.text = namePlayer.text.Substring(0, namePlayer.text.Length - 1);
            }
            else
            {
                namePlayer.text += nameKey;
            }
        }
        else if (isUserLogin)
        {
            if (nameKey == "DEL" && userInput_Login.text.Length > 0)
            {
                userInput_Login.text = userInput_Login.text.Substring(0, userInput_Login.text.Length - 1);
            }
            else
            {
                userInput_Login.text += nameKey;
            }
        }
        else if (isUserRegister)
        {
            if (nameKey == "DEL" && userInput_Register.text.Length > 0)
            {
                userInput_Register.text = userInput_Register.text.Substring(0, userInput_Register.text.Length - 1);
            }
            else
            {
                userInput_Register.text += nameKey;
            }
        }
        else if (isPassLogin)
        {
            if (nameKey == "DEL" && passwordInput_Login.text.Length > 0)
            {
                passwordInput_Login.text = passwordInput_Login.text.Substring(0, passwordInput_Login.text.Length - 1);
            }
            else
            {
                passwordInput_Login.text += nameKey;
            }
        }
        else if (isPassRegister)
        {
            if (nameKey == "DEL" && passwordInput_Register.text.Length > 0)
            {
                passwordInput_Register.text = passwordInput_Register.text.Substring(0, passwordInput_Register.text.Length - 1);
            }
            else
            {
                passwordInput_Register.text += nameKey;
            }
        }
    }
}
