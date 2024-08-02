using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class GameManager : NetworkBehaviour
{
    public NetworkVariable<float> gameTimer = new NetworkVariable<float>();

    [SerializeField] private UIManager uiManager;
    [SerializeField] private Transform[] spawnPoints;
    public static GameManager Singleton { get; private set; }

    [SerializeField] private List<PlayerInfo> connectedPlayers; //SERVER ONLY
    private bool isCounting;
    private void Awake()
    {
        Singleton = this;
    }

    private void Update()
    {
        if(isCounting && NetworkManager.IsServer)
        {
            gameTimer.Value -= Time.deltaTime;
            if(gameTimer.Value <= 0)
            {
                EndGameRpc();
                isCounting = false;
            }
        }
    }

    public void StartMatch()
    {
        if(NetworkManager.IsServer)
        {
            gameTimer.Value = 15;
            isCounting = true;
            foreach(PlayerInfo info in connectedPlayers)
            {
                info.killCount.Value = 0;
            }
        }
    }

    private void EndGameRpc()
    {
        foreach (PlayerInfo info in connectedPlayers)
        {
            info.GetComponent<PlayerMovement>().enabled = false;
        }
    }

    public void OnLocalPlayerJoined(PlayerInfo player)
    {

        player.playerNickname.Value = uiManager.GetLocalNickname();
        OnPlayerJoined(player);
    }


    public void OnPlayerJoined(PlayerInfo playerObject)
    {
        connectedPlayers.Add(playerObject);
        SpawnPlayer(playerObject.NetworkObject);
        playerObject.killCount.OnValueChanged += ScoreboardUpdate;
    }

    private void ScoreboardUpdate(int oldValue, int newValue)
    {
        string tempScoreboard = "";
        foreach (PlayerInfo connectedPlayer in connectedPlayers)
        {
            tempScoreboard += connectedPlayer.playerNickname.Value.ToString() + ": " + connectedPlayer.killCount.Value.ToString() + "\n";
        }


        uiManager.SetScoreboardText(tempScoreboard);
    }

    public void OnPlayerDied(PlayerHealth playerHealth)
    {
        playerHealth.health.Value = 3;
        SpawnPlayer(playerHealth.NetworkObject);

    }

    private void SpawnPlayer(NetworkObject playerObject)
    {
        if (NetworkManager.IsServer)
        {
            Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            playerObject.transform.position = randomSpawn.position;
            playerObject.transform.rotation = randomSpawn.rotation;
        }
    }



}
