using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Mirror;
using UnityEngine.UI;
using MirrorsProject.Clients;

namespace MirrorsProject.Units
{
    public class HealthCanvas : NetworkBehaviour
    {
        #region VARS
        [SerializeField] Image healthImage;
        HealthSystem healthSystem;
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
            UpdateHealthImage();
        }
        #endregion

        #region LOCAL METHODS
        private void ClientInstance_OnOwnerCharacterSpawned(GameObject obj)
        {
            if (obj != null)
            {
                healthSystem = obj.GetComponent<HealthSystem>();
            }
        }

        private void UpdateHealthImage()
        {
            if (healthImage == null)
            {
                return;
            }
            if (healthSystem)
            {
                float percentage = healthSystem.Health / healthSystem.MAX_HEALTH;
                healthImage.fillAmount = percentage;
            }
        }
        #endregion
    }
}