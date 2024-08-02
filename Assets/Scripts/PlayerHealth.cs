using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private TextMeshPro healthText;
    public NetworkVariable<int> health = new NetworkVariable<int>();
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        health.Value = 3;
        health.OnValueChanged += SyncHealth;
    }

    public void GetDamaged(ulong whoDamaged)
    {
        health.Value--;

        if(health.Value <= 0) //ONLY FOR THE SERVER
        {
            //DIE
            NetworkManager.ConnectedClients[whoDamaged].PlayerObject.GetComponent<PlayerInfo>().killCount.Value++;
            GameManager.Singleton.OnPlayerDied(this);
        }

        //Debug.Log(OwnerClientId + " health is now " + health.Value);
    }

    private void Update()
    {
        //Debug.Log(health.Value);
        
    }

    private void SyncHealth(int oldValue, int newValue)
    {
        string tempText = "";
        for(int i = 0; i < newValue; i++)
        {
            tempText += "I";
        }
        healthText.text = tempText;

        if(IsLocalPlayer && IsOwner)
        {
            Debug.Log("Health was " + oldValue + " and now is " + newValue);
        }
    }
}
