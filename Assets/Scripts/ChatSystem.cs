using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class ChatSystem : NetworkBehaviour
{
    public bool myValue;
    public int myNumbers;
    public string myText;
    [SerializeField] private TextMeshProUGUI serverMessage;
    [SerializeField] private TMP_InputField inputField;

    public void SendToChat()
    {
        if(IsServer && !IsHost)
        {
            ShowServerMessageRpc(inputField.text, OwnerClientId);
        }
        else
        {
            RequestMessageSentRpc(inputField.text);
        }
        
    }

    [Rpc(SendTo.Server)]
    public void RequestMessageSentRpc(string msg, RpcParams param = default)
    {
        if(msg.Contains("badword"))
        {
            msg = "I am a sore loser";
        }

        ShowServerMessageRpc(msg, param.Receive.SenderClientId);
    }

    [Rpc(SendTo.Everyone)]
    public void ShowServerMessageRpc(string msg, ulong senderId)
    {
        serverMessage.text += "\n " + senderId + ":" + msg;
    }
}
