using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using MirrorsProject.Units;

namespace MirrorsProject.Clients
{
    public class Namer : NetworkBehaviour
    {
        #region VARS
        private Respawner respawner;
        /// <summary>
        /// the current name for the character
        /// </summary>
        [SyncVar(hook = nameof(OnNameUpdated))]string serverCurrentName;

        public event Action<string> Relay_OnNameUpdated;

        public string GetServerCurrentName ()  { return serverCurrentName; }
        #endregion

        #region MEMBER METHODS
        /// <summary>
        /// set the player name for the local client
        /// </summary>
        /// <param name="name"></param>
        [Client]
        public void SetName(string name)
        {
            CmdSetName(name);
           
        }
        #endregion

        #region ENGINE
        private void Awake()
        {
            respawner = GetComponent<Respawner>();
            Respawner.OnOwnerCharacterSpawned += Respawner_OnOwnerCharacterSpawned;
        }
        private void OnDestroy()
        {
            Respawner.OnOwnerCharacterSpawned -= Respawner_OnOwnerCharacterSpawned;
        }
        #endregion

        #region LOCAL METHODS
        private void Respawner_OnOwnerCharacterSpawned(GameObject obj)
        {
            if (obj != null)
            {
                UpdatePlayerName(serverCurrentName);
            }
        }

        private void OnNameUpdated(string prev, string next)
        {
            UpdatePlayerName(next);
            Relay_OnNameUpdated?.Invoke(next);
        }

        /// <summary>
        /// sets the name for this character
        /// </summary>
        /// <param name="name"></param>
        [Command]
        private void CmdSetName(string name)
        {
            serverCurrentName = name;
            UpdatePlayerName(name);
        }

        private void UpdatePlayerName(string name)
        {
            if (respawner!=null&&respawner.CurrentCharacter != null)
            {
                PlayerName pn = respawner.CurrentCharacter.GetComponent<PlayerName>();
                pn.SetName(name);
            }
        }
        #endregion
    }
}
