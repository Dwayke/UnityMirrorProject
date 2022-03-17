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
        #region VAR
        [SerializeField] TMP_InputField input;
        string lastValue = string.Empty;
        #endregion

        #region ENGINE
        private void Update()
        {
            CheckSetName();
        }
        #endregion

        #region LOCAL METHODS
        private void CheckSetName()
        {
            if (!NetworkClient.active)
            {
                return;
            }
            if (input.text == lastValue)
            {
                return;
            }
            ClientInstance ci = ClientInstance.ReturnClientInstance();
            if (ci == null)
            {
                lastValue = string.Empty;
                return;
            }
            Namer n = ci.GetComponent<Namer>();
            lastValue = input.text;
            n.SetName(input.text);
        }
        #endregion
    }
}
