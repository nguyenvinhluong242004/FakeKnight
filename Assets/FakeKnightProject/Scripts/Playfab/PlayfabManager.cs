using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.IO;
using System;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PlayfabManager : MonoBehaviour
{
    public TMP_Text messageText;
    public TMP_InputField emailInput_Register, emailInput_Fogot;
    public TMP_InputField userInput_Login, userInput_Register;
    public TMP_InputField passwordInput_Login, passwordInput_Register;
    public TMP_InputField namePlayer;
    public bool hidePass;
    public GameObject onPass, offPass;


    [SerializeField] private DataPlayer dataPlayer;
    [SerializeField] private PlayfabObjectManager playfabObjectManager;
    [SerializeField] private ContentScrollView contentScrollView;
    [SerializeField] private LoadingGame loadingGame;

    void Start()
    {
        userInput_Login.text = PlayerPrefs.GetString("Username");
        passwordInput_Login.text = PlayerPrefs.GetString("Password");
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "5D9BF";
        }
        LoginStart();
    }
    void LoginStart()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSucess, OnError);
    }
    public void Login()
    {
        var request = new LoginWithPlayFabRequest
        {
            Username = userInput_Login.text,
            Password = passwordInput_Login.text
        };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSucess, OnError);
    }
    void OnLoginSucess(LoginResult result)
    {
        PlayerPrefs.SetString("Username", userInput_Login.text);
        PlayerPrefs.SetString("Password", passwordInput_Login.text);
        messageText.text = "Loggin in!";
        Debug.Log("Loggin acount sucessful!");
        FindObjectOfType<SceneControl>().LoadScene("Play");
    }
    public void Register()
    {
        if (passwordInput_Register.text.Length < 6)
        {
            messageText.text = "Password too short!";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput_Register.text,
            Username = userInput_Register.text,
            Password = passwordInput_Register.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSucess, OnError);
    }
    void OnRegisterSucess(RegisterPlayFabUserResult result)
    {
        playfabObjectManager.register.SetActive(false);
        playfabObjectManager.chooseFigure.SetActive(true);
        messageText.text = "Registered and loggin in!";
        Debug.Log("Register acount sucessful!");
    }
    public void setDataGame()
    {
        if (namePlayer.text.Length < 1)
            return;
        if (contentScrollView.item == 3)
            return;
        // update data first for user
        dataPlayer.name = namePlayer.text;
        dataPlayer.idPlayer = contentScrollView.item - 1;
        dataPlayer.lv = 1;
        dataPlayer.gold = 2000;
        dataPlayer.diamondPurple = 2000;
        dataPlayer.diamondRed = 2000;
        dataPlayer.isMusic = true;
        dataPlayer.isSound = true;
        dataPlayer.speed = 5f;
        int[] columnsPerRow = { 17, 12, 12, 7 };
        dataPlayer.items = new int[4, columnsPerRow.Max()];
        dataPlayer.types = new int[4, columnsPerRow.Max()];
        for (int i = 1; i < 4; i++)
            for (int j = 0; j < columnsPerRow[i]; j++)
                dataPlayer.types[i, j] = 1;
        dataPlayer.equipments = new int[8];
        for (int i = 0; i < 8; i++)
            dataPlayer.equipments[i] = -1;
        dataPlayer.gift = 0;
        dataPlayer.day = "";
        SaveDataGamePlayer();
        //
    }
    public void Fogot()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput_Fogot.text,
            TitleId = "5D9BF"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnResetSucess, OnError);
    }
    void OnResetSucess(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Reset Password sucessful!";
    }
    private void UpdateDisplayName(string displayname)
    {
        Debug.Log($"Updating Playfab account's Display name to: {displayname}");
        var request = new UpdateUserTitleDisplayNameRequest
        { 
            DisplayName = displayname 
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameSuccess, OnError);
    }
    private void OnDisplayNameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log($"You have updated the displayname of the playfab account!");
        FindObjectOfType<SceneControl>().LoadScene("Play");
    }
    public void SaveDataGamePlayer()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
                {
                    { "DataGamePlayer", JsonConvert.SerializeObject(dataPlayer.ReturnClass()) }
                }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Sucessful Data Send!");
        UpdateDisplayName(userInput_Register.text);
    }
    void OnSucess(LoginResult result)
    {
        Debug.Log("Done!");
        loadingGame.setLoadingGame(true);
    }
    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
}
