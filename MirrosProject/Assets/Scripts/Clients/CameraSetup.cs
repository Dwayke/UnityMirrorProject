using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MirrorsProject.Clients
{
    public class CameraSetup : NetworkBehaviour
    {
        #region VARS
        /// <summary>
        /// object for the camera within the child of this transform
        /// </summary>
        [Tooltip ("object for the camera within the child of this transform")]
        [SerializeField] Transform cameraObject;
        #endregion

        #region MEMBER METHODS
        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            cameraObject.gameObject.SetActive(true);
        }
        #endregion
    }
}
