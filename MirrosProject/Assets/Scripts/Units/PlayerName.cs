using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

namespace MirrorsProject.Units
{
    public class PlayerName : NetworkBehaviour
    {
        #region VARS
        [SyncVar(hook = nameof(OnNameUpdated))] string syncName = string.Empty;
        [SerializeField] TextMeshProUGUI text;
        #endregion

        #region MEMBER METHODS
        /// <summary>
        /// sets the playe name for owner
        /// </summary>
        /// <param name="name"></param>
        [Client]
        public void SetName(string name)
        {
            if (!base.hasAuthority)
            {
                return;
            }
            OnNameUpdated(string.Empty, name);
            CmdSetName(name);
        }
        #endregion

        #region LOCAL METHODS
        /// <summary>
        /// sets the name for this character
        /// </summary>
        /// <param name="name"></param>
        [Command]
        private void CmdSetName(string name)
        {
            syncName = name;
        }
        /// <summary>
        /// SyncVar hook for playerName
        /// </summary>
        private void OnNameUpdated(string prev, string next)
        {
            text.text = next;
        }
        #endregion
    }
}
