using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Transform tip;
    private Transform player;

    [Header("Spell")]
    [SerializeField] private float castRate;
    [SerializeField] private float maxRange;
    [SerializeField] private LayerMask shootableMask;
    [SerializeField] private LayerMask environmentMask;
    private Coroutine spellCastCoroutine;

    [Header("Tracer")]
    [SerializeField] private float spellTracerDuration;
    [SerializeField] private LineRenderer spellTracer;

    private void Start() {

        player = FindObjectOfType<PlayerController>().transform;

    }

    public void CastSpell(Gradient gradient) {

        if (spellCastCoroutine != null) return; // already casting spell

        spellTracer.colorGradient = gradient; // set color gradient
        spellCastCoroutine = StartCoroutine(HandleCastSpell());

    }

    private IEnumerator HandleCastSpell() {

        RaycastHit2D shootableHit = Physics2D.Raycast(tip.position, player.right, maxRange, shootableMask); // for checking if a shootable is hit
        RaycastHit2D obstacleHit = Physics2D.Raycast(tip.position, player.right, maxRange, environmentMask); // for checking if an obstacle is in the way

        if (obstacleHit && (Vector2.Distance(tip.position, obstacleHit.point) <= Vector2.Distance(tip.position, shootableHit.point) || !shootableHit)) { // obstacle in the way or shot didn't hit shootable, but hit obstacle

            // impact effect
            // Instantiate(impactEffect, obstacleHit.point, Quaternion.identity);

            // set bullet tracer points
            spellTracer.SetPosition(0, tip.position);
            spellTracer.SetPosition(1, obstacleHit.point);

        } else if (shootableHit) {

            //bool? deathCaused = false; // to prevent impact effect when something dies (for better looking gfx)

            //if (entityType == EntityType.Player)
            //    deathCaused = shootableHit.transform.GetComponent<PhantomHealthManager>()?.TakeDamage(gunData.GetDamage() * multiplier); // damage enemy if player is shooter
            //else if (entityType == EntityType.Phantom)
            //    deathCaused = shootableHit.transform.GetComponent<PlayerHealthManager>()?.TakeDamage(gunData.GetDamage() * multiplier);  // damage player if enemy is shooter

            //// impact effect
            //if (deathCaused != null && !(bool) deathCaused)
            //    Instantiate(impactEffect, shootableHit.point, Quaternion.identity);

            // set bullet tracer points
            spellTracer.SetPosition(0, tip.position);
            spellTracer.SetPosition(1, shootableHit.point);

        } else { // miss

            // set bullet tracer points
            spellTracer.SetPosition(0, tip.position);
            spellTracer.SetPosition(1, tip.position + player.right * maxRange);
            // bulletTracer.SetPosition(1, muzzle.position + muzzle.right * 100f); // illusion for infinite length tracer when missed

        }

        // display bullet tracer
        spellTracer.enabled = true;
        yield return new WaitForSeconds(spellTracerDuration);
        spellTracer.enabled = false;

        yield return new WaitForSeconds(1f / castRate); // use fire rate to prevent shooting
        spellCastCoroutine = null;

    }
}
