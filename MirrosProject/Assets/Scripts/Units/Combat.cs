using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MirrorsProject.Units
{
    public class Combat : NetworkBehaviour
    {
        /// <summary>
        /// next time this unit can attack
        /// </summary>
        float nextAttack = 0f;

        NetworkAnimator networkAnimator;

        private void Awake()
        {
            networkAnimator = GetComponent<NetworkAnimator>();
        }
        private void Update()
        {
            Debug.Log(nextAttack);
            if (base.hasAuthority)
            {
                CheckAttack();
            }
        }

        /// <summary>
        /// checks if can attack
        /// </summary>
        private void CheckAttack()
        {
            if (Time.time < nextAttack)
            {
                return;
            }
            if (!Input.GetKeyDown(KeyCode.Mouse0))
            {
                return;
            }
            nextAttack = 0;
            nextAttack += Time.time + 1f;
            networkAnimator.SetTrigger("Attack");
        }
    }
}
