using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using MirrorsProject.Units;

namespace MirrorsProject.Clients
{
    public class ClientInstance : NetworkBehaviour
    {
        #region VARS
        /// <summary>
        ///singleton reference to the client instance, for the local player 
        /// </summary>
        public static ClientInstance instance;

        /// <summary>
        /// Dispatched to the player when they're instantiated
        /// </summary>
        public static event Action<GameObject> OnOwnerCharacterSpawned;

        /// <summary>
        /// Prefab for the player
        /// </summary>
        [Tooltip ("Prefab for the player")]
        [SerializeField] NetworkIdentity playerPrefab;

        /// <summary>
        /// currently spawned character for the local player
        /// </summary>
        GameObject currentCharacter = null;
        /// <summary>
        /// the current name for the character
        /// </summary>
        string currentName = string.Empty;
        #endregion

        #region MEMBER METHODS
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            instance = this;
            CmdRequestSpawn();
        }

        public static ClientInstance ReturnClientInstance(NetworkConnection conn = null)
        {
            if (NetworkServer.active && conn !=null)
            {
                NetworkIdentity localPlayer;
                if (NewNetworkManager.LocalPlayers.TryGetValue(conn,out localPlayer))
                {
                    return localPlayer.GetComponent<ClientInstance>();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return instance;
            }
        }
        
        public void InvokeCharacterSpawned(GameObject go)
        {
            currentCharacter = go;
            SetName(currentName);
            OnOwnerCharacterSpawned?.Invoke(go);
        }

        /// <summary>
        /// set the player name for the local client
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            currentName = name;
            if (currentCharacter != null)
            {
                PlayerName pn = currentCharacter.GetComponent<PlayerName>();
                pn.SetName(name);
            }
            else
            {
                return;
            }
        }
        #endregion

        #region LOCAL METHODS
        /// <summary>
        /// request a spawn for character
        /// </summary>
        [Command]
        private void CmdRequestSpawn()
        {
            NetworkSpawnPlayer();
        }
        /// <summary>
        /// spawns a character for the player
        /// </summary>
        [Server]
        private void NetworkSpawnPlayer()
        {
            GameObject go = Instantiate(playerPrefab.gameObject, transform.position, Quaternion.identity);
            NetworkServer.Spawn(go,base.connectionToClient);
        }
        #endregion
    }
}
