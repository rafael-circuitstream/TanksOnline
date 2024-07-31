using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<int> health = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        health.Value = 3;
        health.OnValueChanged += SyncHealth;
    }

    public void GetDamaged()
    {
        health.Value--;
        //Debug.Log(OwnerClientId + " health is now " + health.Value);
    }

    private void Update()
    {
        //Debug.Log(health.Value);
        
    }

    private void SyncHealth(int oldValue, int newValue)
    {
        if(IsLocalPlayer && IsOwner)
        {

            Debug.Log("Health was " + oldValue + " and now is " + newValue);
        }
    }
}
