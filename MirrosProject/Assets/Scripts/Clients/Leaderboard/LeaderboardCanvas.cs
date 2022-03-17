using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Mirror;
using UnityEngine.UI;

namespace MirrorsProject.Clients.Leaderboard
{
    public class LeaderboardCanvas : NetworkBehaviour
    {
        #region VARS
        [SerializeField] Transform content;
        [SerializeField] PlayerScore prefab;

        List<PlayerScore> addedPlayerScores = new List<PlayerScore>();

        public static LeaderboardCanvas Instance { get; private set; }
        #endregion

        public override void OnStartServer()
        {
            base.OnStartServer();
            NewNetworkManager.Relay_OnServerAddPlayer += NewNetworkManager_Relay_OnServerAddPlayer;
            NewNetworkManager.Relay_OnServerDisconnect += NewNetworkManager_Relay_OnServerDisconnect;
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            NewNetworkManager.Relay_OnServerAddPlayer -= NewNetworkManager_Relay_OnServerAddPlayer;
            NewNetworkManager.Relay_OnServerDisconnect -= NewNetworkManager_Relay_OnServerDisconnect;
        }

        [Server]
        public void KillSecured(NetworkConnection attackerConn)
        {
            ClientInstance ci = ClientInstance.ReturnClientInstance(attackerConn);
            if (ci!=null)
            {
                AddScore(ci.netId, 1);
            }
        }

        private void AddScore(uint netId, int value)
        {
            int index = addedPlayerScores.FindIndex(x => x.NetId == netId);
            if (index != -1)
            {
                addedPlayerScores[index].AddScore(value);
                if (base.isServer)
                {
                    RpcAddScore(netId, value);
                }
            }
        }

        [ClientRpc]
        private void RpcAddScore(uint netId, int value)
        {
            if (base.isServer)
            {
                return;
            }
            AddScore(netId, value);
        }

        private void NewNetworkManager_Relay_OnServerAddPlayer(NetworkConnection conn)
        {
            if (NewNetworkManager.LocalPlayers.TryGetValue(conn,out NetworkIdentity newPlayerNetIdent))
            {
                foreach (PlayerScore item in addedPlayerScores)
                {
                    if (NetworkIdentity.spawned.TryGetValue(item.NetId, out NetworkIdentity existingPlayerNetId))
                    {
                        NetworkConnection existingPlayerConn = existingPlayerNetId.connectionToClient;
                        ClientInstance ci = ClientInstance.ReturnClientInstance(existingPlayerConn);

                        int score = item.GetScore();
                        Namer n = ci.GetComponent<Namer>();
                        string name = n.GetServerCurrentName();

                        TargetPlayerConnected(conn, item.NetId, name, score);
                    }

                }

                RPCPlayerConnected(newPlayerNetIdent.netId);
                AddPlayer(newPlayerNetIdent.netId, "New Player", 0);
            }
            
        }

        private void NewNetworkManager_Relay_OnServerDisconnect(NetworkConnection conn)
        {
            if (NewNetworkManager.LocalPlayers.TryGetValue(conn, out NetworkIdentity ni))
            {
                RPCPlayerDisconnected(ni.netId);
                RemovePlayer(ni.netId);
            }
        }

        private void PlayerJoined(NetworkConnection conn)
        {

        }

        [TargetRpc]
        private void TargetPlayerConnected(NetworkConnection conn, uint netId, string name, int score)
        {
            if (base.isServer)
            {
                return;
            }
            AddPlayer(netId, name, score);
        }

        private void AddPlayer(uint netId, string name, int score)
        {
            PlayerScore ps = Instantiate(prefab, content);
            ps.SetNetId(netId);
            ps.SetPlayerName(name);
            ps.AddScore(score);

            addedPlayerScores.Add(ps);
        }

        private void RemovePlayer(uint netId)
        {
            int index = addedPlayerScores.FindIndex(x => x.NetId == netId);
            if (index != -1)
            {
                Destroy(addedPlayerScores[index].gameObject);
                addedPlayerScores.RemoveAt(index);
            }
        }

        [ClientRpc]
        private void RPCPlayerConnected(uint conn)
        {
            if (base.isServer)
            {
                return;
            }
            Debug.Log(conn + " Has Joined.");
            AddPlayer(conn,"New Player",0);
        }

        [ClientRpc]
        private void RPCPlayerDisconnected(uint conn)
        {
            if (base.isServer)
            {
                return;
            }
            Debug.Log(conn + " Has Left.");
            RemovePlayer(conn);
        }

        private void ClearPlayerScores()
        {
            for (int i = 0; i < addedPlayerScores.Count; i++)
            {
                Destroy(addedPlayerScores[i].gameObject);
            }

            addedPlayerScores.Clear();
        }

        private void NewNetworkManager_Relay_OnClientStop()
        {
            ClearPlayerScores();
        }

        private void NewNetworkManager_Relay_OnServerStop()
        {
            ClearPlayerScores();
        }

        #region ENGINE
        private void Awake()
        {
            Instance = this;
            NewNetworkManager.Relay_OnClientStop += NewNetworkManager_Relay_OnClientStop;
            NewNetworkManager.Relay_OnServerStop += NewNetworkManager_Relay_OnServerStop;
        }

        private void OnDestroy()
        {
            NewNetworkManager.Relay_OnClientStop -= NewNetworkManager_Relay_OnClientStop;
            NewNetworkManager.Relay_OnServerStop -= NewNetworkManager_Relay_OnServerStop;
        }

        #endregion
    }
}