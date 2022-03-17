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
        #endregion

        #region MEMBER METHODS
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            instance = this;
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
        #endregion
    }
}
