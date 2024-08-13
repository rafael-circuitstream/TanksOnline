using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LoginRegisterScreen : MonoBehaviour
{
    public UserInformationDisplay loggedUserDisplay;
    public UserInformation loggedUser;
    public GameObject loginScreen;

    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField scoreInput;

    public static string api_url = "https://api-tanks-test-default-rtdb.firebaseio.com/AllPlayers/";
    public void LoginButton()
    {
        StartCoroutine(LoginVerification());
    }

    IEnumerator LoginVerification()
    {
        UnityWebRequest request = UnityWebRequest.Get(api_url + usernameInput.text + ".json");
        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success && request.downloadHandler.text != "null")
        {
            UserInformation userFound = JsonUtility.FromJson<UserInformation>(request.downloadHandler.text);
            if(userFound.password == passwordInput.text)
            {
                Debug.Log("Login Success");
                loggedUser = userFound;
                loginScreen.SetActive(false);
                loggedUserDisplay.UpdateUsername(loggedUser.username);
            }
            else
            {
                Debug.Log("PASSWORD INCORRECT, TRY AGAIN");
            }
        }
        else
        {
            Debug.Log("USERNAME NOT FOUND, TRY AGAIN");
        }
    }
    public void RegisterButton()
    {
        StartCoroutine(RegisterAccount());
    }

    IEnumerator RegisterAccount()
    {
        UserInformation userInfo = new UserInformation();
        userInfo.username = usernameInput.text;
        userInfo.password = passwordInput.text;
        string userJson = JsonUtility.ToJson(userInfo);
        UnityWebRequest request = UnityWebRequest.Put(api_url + userInfo.username + ".json", userJson);

        
        yield return request.SendWebRequest();
        
        if(request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Registered Account with ID "+ request.downloadHandler.text);
            loggedUser = userInfo;
            loginScreen.SetActive(false);
            loggedUserDisplay.UpdateUsername(loggedUser.username);
        }
    }

    public void SubmitScore()
    {
        if(loggedUser.highestScore < int.Parse(scoreInput.text))
        {
            StartCoroutine(ScoreUpdate(int.Parse(scoreInput.text)));
        }
        
    }

    IEnumerator ScoreUpdate(int newScore)
    {
        UnityWebRequest request = UnityWebRequest.Put(api_url + loggedUser.username + "/highestScore.json", newScore.ToString());
        
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("New score updated to " + newScore.ToString());
            loggedUser.highestScore = newScore;
            StartCoroutine(UpdateLoggedUser());

        }
    }

    IEnumerator UpdateLoggedUser()
    {
        UnityWebRequest request = UnityWebRequest.Get(api_url + loggedUser.username + ".json");
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.Success)
        {
            loggedUser = JsonUtility.FromJson<UserInformation>(request.downloadHandler.text);
        }
    }
}
