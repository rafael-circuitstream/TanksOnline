using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkBullet : NetworkBehaviour
{
    //[SerializeField] private float shootingSpeed;
    [SerializeField] private NetworkObject particle;
    [SerializeField] private Rigidbody rb;
    private ulong realOwnerId;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (IsServer)
        {
            NetworkManager.SpawnManager.InstantiateAndSpawn(particle, position: transform.position, rotation: transform.rotation);
        }

    }
    public void ShootBullet(float shootingSpeed, ulong shooterId)
    {
        rb.AddForce(rb.transform.forward * shootingSpeed, ForceMode.VelocityChange);
        Invoke("DestroyWithDelay", 3f);
        realOwnerId = shooterId;
    }

    void DestroyWithDelay()
    {
        NetworkObject.Despawn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(IsServer && IsSpawned)
        {
            PlayerHealth temporaryHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if(temporaryHealth && realOwnerId != temporaryHealth.OwnerClientId)
            {
                Debug.Log(realOwnerId + " hitting tank" + temporaryHealth.OwnerClientId);
                temporaryHealth.GetDamaged();
            }
            NetworkObject.Despawn();
        }
    }
}
