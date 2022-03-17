using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MirrorsProject.Clients
{
    public class OwnerSpawnedAnnouncer : NetworkBehaviour
    {
        #region MEMBER METHODS
        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            AnnounceSpawned();
        }
        #endregion

        #region LOCAL METHODS
        private void AnnounceSpawned()
        {
            ClientInstance ci = ClientInstance.ReturnClientInstance();
            Respawner r = ci.GetComponent<Respawner>();
            r.InvokeCharacterSpawned(gameObject);
        }
        #endregion
    }
}
