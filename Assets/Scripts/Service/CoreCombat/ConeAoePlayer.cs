﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Werewolf.StatusIndicators.Components;

public class ConeAoePlayer : Singleton<ConeAoePlayer>
{
    private ConeAoe cardPlaying;
    private SplatManager splat;
    private LayerMask layerMask;
    private Transform playerTransform;

    public void Awake()
    {
        layerMask = LayerMask.GetMask("Enemy");
    }

    public void Ready()
    {
        Refresh();
        splat.SelectSpellIndicator(cardPlaying.owner + "ConeSelector");
        Cone currentSpellIndicator = (Cone) splat.CurrentSpellIndicator;
        currentSpellIndicator.Scale = (cardPlaying.range + 2) * 2;
        currentSpellIndicator.Angle = cardPlaying.angle;
        splat.CancelRangeIndicator();
    }

    public void Play()
    {
        Refresh();
        var rotation = Quaternion.LookRotation(
                (Vector3.ProjectOnPlane(splat.Get3DMousePosition()
                - playerTransform.position, new Vector3(0, 1, 0)).normalized));
        
        Instantiate(
            cardPlaying.vfx,
            playerTransform.position
                + rotation * cardPlaying.vfxOffset,
            rotation);
            

        var enemies = GetEnemies(
            playerTransform.position,
            cardPlaying.range);

        foreach (Collider e in enemies)
        {
            var direction = Vector3.ProjectOnPlane(
                e.gameObject.transform.position
                - playerTransform.position, new Vector3(0,1,0)).normalized;

            var force = direction * cardPlaying.force;
            StartCoroutine(
                hitAfterDelay(
                e.gameObject,
                cardPlaying.damage,
                force,
                cardPlaying.delay));
        }
    }

    private Collider[] GetEnemies(Vector3 hitPoint, float attackRange)
    {
        return Physics.OverlapSphere(hitPoint, attackRange, layerMask)
            .Where(
                e => Vector3.Angle(
                        Vector3.ProjectOnPlane(
                            e.gameObject.transform.position - hitPoint,
                            new Vector3(0,1,0)).normalized, 
                        Vector3.ProjectOnPlane(
                            splat.Get3DMousePosition() - hitPoint,
                            new Vector3(0,1,0)).normalized)
                     < cardPlaying.angle)
            .ToArray();
    }

    IEnumerator hitAfterDelay(GameObject e, int damage, Vector3 force, float delay)
    {
        yield return new WaitForSeconds(delay);
        e.GetComponent<Health>().TakeDamage(damage);
        e.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        yield return null;
    }

    private void Refresh()
    {
        cardPlaying = (ConeAoe)CardPlayer.Instance.cardPlaying;
        splat = CardPlayer.Instance.splat;
        playerTransform = FindObjectsOfType<PlayerController>()
           .Where(player => player.gameObject.name == cardPlaying.owner.ToString())
           .FirstOrDefault().gameObject.transform;
    }
}
