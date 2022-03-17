using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

namespace MirrorsProject.Clients
{
    public class WorldCameraSetup : MonoBehaviour
    {
        #region VARS
        /// <summary>
        /// Offset from the character
        /// </summary>
        [Tooltip ("Offset from the character")]
        [SerializeField] Vector3 positionOffset = new Vector3(2f, 1f, -5f);
        [SerializeField] Vector3 rotationOffset = new Vector3(-5f,-5f,0f);
        /// <summary>
        /// target to follow
        /// </summary>
        Transform target;
        #endregion

        #region ENGINE
        private void Awake()
        {
            Respawner.OnOwnerCharacterSpawned += ClientInstance_OnOwnerCharacterSpawned;
        }

        private void OnDestroy()
        {
            Respawner.OnOwnerCharacterSpawned -= ClientInstance_OnOwnerCharacterSpawned;
        }

        private void Update()
        {
            if (target==null)
            {
                return;
            }
            transform.rotation = target.rotation *Quaternion.Euler(rotationOffset);
            transform.position = target.position + positionOffset;
        }
        #endregion

        #region LOCAL METHODS
        private void ClientInstance_OnOwnerCharacterSpawned(GameObject go)
        {
            if (go!=null)
            {
                target = go.transform;

            }
        }
        #endregion
    }
}
