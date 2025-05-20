using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RoomManager : NetworkBehaviour
{
    private const int MAX_PLAYER_COUNT = 4;
    public static RoomManager Instance { get; private set; }

    public List<ulong> playerIds = new List<ulong>();
    public const int MaxPlayers = 4;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (playerIds.Count >= MaxPlayers)
        {
            // 최대 인원 초과 시 강제 퇴장
            NetworkManager.Singleton.DisconnectClient(clientId);
            return;
        }
        playerIds.Add(clientId);
    }

    private void OnClientDisconnected(ulong clientId)
    {
        playerIds.Remove(clientId);
    }

    public bool IsLobbyFull() => playerIds.Count >= MaxPlayers;
}
