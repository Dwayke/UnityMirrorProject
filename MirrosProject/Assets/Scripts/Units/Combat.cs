using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MirrorsProject.Units
{
    public class Combat : NetworkBehaviour
    {
        #region VARS
        /// <summary>
        /// Visual Effects for the projectile
        /// </summary>
        [Tooltip("Visual Effects for the projectile")]
        [SerializeField] GameObject projectileVFX;

        /// <summary>
        /// Visual Effects for the projectile
        /// </summary>
        [Tooltip("Visual Effects for the Hits")]
        [SerializeField] GameObject HitVFX;

        /// <summary>
        /// next time this unit can attack
        /// </summary>
        float nextAttack = 0f;

        NetworkAnimator networkAnimator;
        Collider[] colliders;
        HealthSystem healthSystem;
        #endregion

        #region ENGINE
        private void Awake()
        {
            networkAnimator = GetComponent<NetworkAnimator>();
            colliders = GetComponentsInChildren<Collider>();
            healthSystem = GetComponent<HealthSystem>();
        }
        private void Update()
        {
            if (base.hasAuthority)
            {
                CheckAttack();
            }
        }
        #endregion

        #region LOCAL METHODS
        /// <summary>
        /// checks if can attack
        /// </summary>
        private void CheckAttack()
        {
            if (!Input.GetKeyDown(KeyCode.Mouse0))
            {
                return;
            }
            if (!FireTimeMet())
            {
                return;
            }

            StartCoroutine(SpawnProjectiles(transform.position, transform.forward));
            networkAnimator.SetTrigger("Attack");
            CmdAttack(transform.position, transform.forward);
        }

        /// <summary>
        /// Fire Cooldown
        /// </summary>
        /// <param name="resetTime"></param>
        /// <returns></returns>
        private bool FireTimeMet(bool resetTime=true)
        {
            bool result = (Time.time >=  nextAttack);
            if (resetTime)
            {
                nextAttack = Time.time + .25f;
            }
            return result;
        }

        [Command]
        private void CmdAttack(Vector3 pos,Vector3 dir)
        {
            if (!FireTimeMet())
            {
                return;
            }
            dir = dir.normalized;
            float maxPositionOffset = 1f;
            if (Vector3.Distance(pos,transform.position)>maxPositionOffset)
            {
                Vector3 posDirection = pos - transform.position;
                pos = transform.position + (maxPositionOffset * posDirection);
            }
            if (base.isClient)
            {
                StartCoroutine(SpawnProjectiles(pos, dir));
            }
            RpcAttack(pos, dir);
        }

        [ClientRpc]
        private void RpcAttack(Vector3 pos,Vector3 dir)
        {
            if (base.hasAuthority||base.isServer)
            {
                return;
            }
            StartCoroutine(SpawnProjectiles(pos, dir));
        }

        private void TraceForHits(Vector3 pos, Vector3 dir)
        {
            SetColliders(false);
            Ray ray = new Ray(pos, dir);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit, 100f))
            {
                if (base.isClient)
                {
                    Instantiate(HitVFX, hit.point, Quaternion.identity);
                }
                if (base.isServer)
                {
                    HealthSystem target = hit.collider.transform.root.GetComponent<HealthSystem>();
                    target.DealDamage(20f, base.connectionToClient);
                }
            }
            SetColliders(true);
        }

        private void SetColliders(bool enabled)
        {
            foreach (var collider in colliders)
            {
                collider.enabled = enabled;
            }
        }

        private IEnumerator SpawnProjectiles(Vector3 pos,Vector3 dir)
        {
            pos += new Vector3(0f, 0.5f, 0f);
            TraceForHits(pos, dir);
            if (base.isClient)
            {

                GameObject go = Instantiate(projectileVFX,pos,new Quaternion(0f,0f,0f,0f));
                float moveRate = 100f;

                WaitForEndOfFrame wait = new WaitForEndOfFrame();
                while (go!=null)
                {
                    go.transform.position += (dir * moveRate * Time.deltaTime);
                    yield return wait;
                }
            }
        }
        #endregion
    }
}
