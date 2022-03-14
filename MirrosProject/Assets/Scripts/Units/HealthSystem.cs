using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MirrorsProject.Units
{
    public class HealthSystem : NetworkBehaviour
    {
        [SyncVar] float health = 100f;
        public float Health
        {
            get { return health; }
            private set { health = value; }
        }
        public float MAX_HEALTH = 100f;
        [Server]
        public void DealDamage(float amount)
        {
            Health -= amount;
            Health = Mathf.Max(0f, Health);
        }
    }
}
