using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using MirrorsProject.Units;

namespace MirrorsProject.Clients
{
    public class Respawner : NetworkBehaviour
    {
        #region VARS
        /// <summary>
        /// Dispatched to the player when they're instantiated
        /// </summary>
        public static Action<GameObject> OnOwnerCharacterSpawned;

        /// <summary>
        /// Prefab for the player
        /// </summary>
        [Tooltip ("Prefab for the player")]
        [SerializeField] NetworkIdentity playerPrefab;

        /// <summary>
        /// currently spawned character for the local player
        /// </summary>
        public GameObject CurrentCharacter { get; private set; }
        #endregion

        #region MEMBER METHODS
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            CmdRequestSpawn();
        }

        public void InvokeCharacterSpawned(GameObject go)
        {
            CurrentCharacter = go;
            OnOwnerCharacterSpawned?.Invoke(go);
        }

        /// <summary>
        /// spawns a character for the player
        /// </summary>
        [Server]
        public void NetworkSpawnPlayer()
        {
            GameObject go = Instantiate(playerPrefab.gameObject, transform.position, Quaternion.identity);
            CurrentCharacter = go;
            NetworkServer.Spawn(go, base.connectionToClient);
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
        #endregion
    }
}
