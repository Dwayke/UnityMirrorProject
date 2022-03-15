using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MirrorsProject.Units
{
    public class Motor : NetworkBehaviour
    {
        #region VARS
        /// <summary>
        /// How quick it moves
        /// </summary>
        [Tooltip("How quick it moves")]
        [SerializeField] float moveRate = 3f;
        
        /// <summary>
        /// How quick it turns
        /// </summary>
        [Tooltip("How quick it turns")]
        [SerializeField] float turnRate = 10f;

        /// <summary>
        /// CharacterController Reference
        /// </summary>
        private CharacterController characterController = null;
        
        /// <summary>
        /// animator for this object
        /// </summary>
        Animator animator;
        #endregion

        #region ENGINE
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (base.hasAuthority)
            {
                Move();
            }
        }
        #endregion

        #region MEMBER METHODS
        public override void OnStartClient()
        {
            base.OnStartClient();
            characterController.enabled = base.hasAuthority;
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            animator = GetComponent<Animator>();
        }
        #endregion

        #region LOCAL METHODS
        private void Move()
        {
            float forward = Input.GetAxisRaw("Vertical");
            float rotation = Input.GetAxisRaw("Horizontal");

            Vector3 next = new Vector3(0f, 0f, forward * Time.deltaTime * moveRate);
            next += Physics.gravity * Time.deltaTime;

            transform.Rotate(new Vector3(0f, rotation * Time.deltaTime * turnRate, 0f));
            characterController.Move(transform.TransformDirection(next));

            animator.SetFloat("Forward", forward);
        }
        #endregion
    }
}
