using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class ScoreboardManager : MonoBehaviour
{
    public UserInformationDisplay scoreEntry;
    public UserCollection AllPlayers;

    public void DisplayScoreboard()
    {
        StartCoroutine(InitializeScoreboard());
    }

    IEnumerator InitializeScoreboard()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://api-tanks-test-default-rtdb.firebaseio.com/.json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            AllPlayers = JsonConvert.DeserializeObject<UserCollection>(request.downloadHandler.text);
            //sort the array found to higher scores first

            foreach(UserInformation info in AllPlayers.AllPlayers)
            {
                UserInformationDisplay display = Instantiate(scoreEntry); //set parent of this clone to be in the correct UI
                display.UpdateScoreDisplay(info.highestScore);
                display.UpdateUsername(info.username);
            }
        }
    }
}

[System.Serializable]
public class UserCollection
{
    public List<UserInformation> AllPlayers;

    public UserCollection()
    {
        AllPlayers = new List<UserInformation>();
    }
}

