using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class UIManager : MonoBehaviour
{

    public void HostButtonClick()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void ClientButtonClick()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void ServerButtonClick()
    {
        NetworkManager.Singleton.StartServer();
    }
}
