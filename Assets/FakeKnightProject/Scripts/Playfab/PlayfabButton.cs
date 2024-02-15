using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayfabButton : MonoBehaviour
{
    [SerializeField] private PlayfabManager playfabManager;
    [SerializeField] private PlayfabObjectManager playfabObjectManager;

    public void Login()
    {
        playfabManager.Login();
    }
    public void Register()
    {
        playfabManager.Register();
    }
    public void Fogot()
    {
        playfabManager.Fogot();
    }
    public void setDataAndStartRegister()
    {
        playfabManager.setDataGame();
    }    
    public void ButtonLogin()
    {
        playfabObjectManager.register.SetActive(false);
        playfabObjectManager.fogot.SetActive(false);
        playfabObjectManager.login.SetActive(true);
    }
    public void ButtonRegister()
    {
        playfabObjectManager.fogot.SetActive(false);
        playfabObjectManager.login.SetActive(false);
        playfabObjectManager.register.SetActive(true);
    }
    public void ButtonFogot()
    {
        playfabObjectManager.register.SetActive(false);
        playfabObjectManager.login.SetActive(false);
        playfabObjectManager.fogot.SetActive(true);
    }
    public void setHidePass()
    {
        if (playfabManager.hidePass)
        {
            playfabManager.passwordInput_Login.contentType = TMP_InputField.ContentType.Standard;
            playfabManager.onPass.SetActive(false);
            playfabManager.offPass.SetActive(true);
        }
        else
        {
            playfabManager.passwordInput_Login.contentType = TMP_InputField.ContentType.Password;
            playfabManager.onPass.SetActive(true);
            playfabManager.offPass.SetActive(false);
        }
        playfabManager.passwordInput_Login.ForceLabelUpdate();
        playfabManager.hidePass = !playfabManager.hidePass;
    }    
}
