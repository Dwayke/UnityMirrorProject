using Mirror;
using MirrorsProject.Clients;
using MirrorsProject.Clients.Leaderboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MirrorsProject.Units
{
    public class HealthSystem : NetworkBehaviour
    {
        #region VARS
        [SyncVar] float health = 100f;
        #endregion

        #region MEMBER METHODS
        public float Health
        {
            get { return health; }
            private set { health = value; }
        }
        public float MAX_HEALTH = 100f;
        [Server]
        public void DealDamage(float amount, NetworkConnection attackerConn)
        {
            Health -= amount;
            Health = Mathf.Max(0f, Health);

            if (health == 0f)
            {
                LeaderboardCanvas.Instance.KillSecured(attackerConn);
                NetworkConnection ownerConn = base.connectionToClient;
                NetworkServer.Destroy(gameObject);
                ClientInstance ci = ClientInstance.ReturnClientInstance(ownerConn);
                if (ci != null)
                {
                    Respawner r = ci.GetComponent<Respawner>();
                    r.NetworkSpawnPlayer();
                }
            }
        }
        #endregion
    }
}
