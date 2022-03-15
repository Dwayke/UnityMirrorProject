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
        [SerializeField] Image healthImage;
        HealthSystem healthSystem;



        private void Awake()
        {
            ClientInstance.OnOwnerCharacterSpawned += ClientInstance_OnOwnerCharacterSpawned;
        }

        private void OnDestroy()
        {
            ClientInstance.OnOwnerCharacterSpawned -= ClientInstance_OnOwnerCharacterSpawned;
        }

        private void ClientInstance_OnOwnerCharacterSpawned(GameObject obj)
        {
            if (obj != null)
            {
                healthSystem = obj.GetComponent<HealthSystem>();
            }
        }

        private void Update()
        {
            UpdateHealthImage();
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
    }
}