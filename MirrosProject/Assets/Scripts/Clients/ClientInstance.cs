using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

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
            Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaa");
            OnOwnerCharacterSpawned?.Invoke(go);
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
