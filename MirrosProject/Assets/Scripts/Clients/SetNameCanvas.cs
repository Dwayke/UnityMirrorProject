using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Mirror;

namespace MirrorsProject.Clients
{
    public class SetNameCanvas : NetworkBehaviour
    {
        [SerializeField] TMP_InputField input;
        string lastValue = string.Empty;

        private void Update()
        {
            CheckSetName();
        }

        private void CheckSetName()
        {
            if (!NetworkClient.active)
            {
                return;
            }
            ClientInstance ci = ClientInstance.ReturnClientInstance();
            if (ci == null)
            {
                lastValue = string.Empty;
                return;
            }
            if (input.text != lastValue)
            {
                lastValue = input.text;
                ci.SetName(input.text);
            }
        }
    }
}
