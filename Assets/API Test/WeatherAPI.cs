using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class WeatherAPI : MonoBehaviour
{
    public string cityName;
    private const string API_URL = "https://api.openweathermap.org/data/2.5/weather/?units=metric&";
    private const string APP_ID = "appid=b111af389eccbd055b9c30c296444e2f"; //REPLACE BY YOUR OWN KEY

    public WeatherInfo retrievedInfo;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FetchInfoByCityName());
    }

    IEnumerator FetchInfoByCityName()
    {
        string apiCall = $"{API_URL}q={cityName}&{APP_ID}";
        //string apiCall = API_URL + "q=" + cityName + "&" + APP_ID;
        UnityWebRequest request = UnityWebRequest.Get(apiCall);

        yield return request.SendWebRequest();
        retrievedInfo = JsonUtility.FromJson<WeatherInfo>(request.downloadHandler.text);
        
    }
}
