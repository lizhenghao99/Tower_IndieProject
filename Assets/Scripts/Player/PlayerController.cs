﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;
using Werewolf.StatusIndicators.Components;

public class PlayerController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] Waypoint waypoint;
    [SerializeField] float waypointXRotation;
    [SerializeField] float waypointHeight;
    [SerializeField] public Animator animator;
    [SerializeField] GameObject highlight;


    private bool isSelected = false;
    public bool isWalking { get; private set; } = false;
    public bool isCasting = false;
    private int layerMask;

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        layerMask = LayerMask.GetMask("Environment", "Player", "Enemy");
    }


    // Update is called once per frame
    void Update()
    {
        // anmiation
        animator.SetFloat("Velocity", agent.velocity.magnitude);
        if (agent.velocity.magnitude > Mathf.Epsilon && !isCasting)
        {
            spriteRenderer.flipX = agent.velocity.x < 0;
        }
        // mouse left
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                if (isSelected && hit.transform.tag == "Floor")
                {
                    Waypoint wp = FindObjectsOfType<Waypoint>()
                                    .Where(p => p.name == waypoint.name+"(Clone)")
                                    .FirstOrDefault();
                    if (wp != null)
                    {
                        wp.transform.position = hit.point 
                            + new Vector3(0,waypointHeight,0);
                    }
                    else
                    {
                        wp = Instantiate(
                            waypoint, 
                            hit.point + new Vector3(0,waypointHeight,0), 
                            Quaternion.Euler(waypointXRotation,0,0));
                        wp.destinationReached += OnDestinationReached;
                    }
                    agent.stoppingDistance = 0;
                    agent.SetDestination(hit.point);
                    isWalking = true;
                    isSelected = false;
                }
                else 
                {
                    if (hit.transform.name == gameObject.name)
                    {
                        isSelected = true;
                    }
                } 
            }
        }

        // mouse right
        if (Input.GetMouseButtonDown(1))
        {
            isSelected = false;
            if (!isWalking)
            {
            }
        }

        // select effect
        if (isSelected)
        {
            highlight.SetActive(true);
            
        }
        else
        {
            highlight.SetActive(false);
        }   
        
        if (isCasting)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    private void OnDestinationReached(object sender, EventArgs e)
    {
        isWalking = false;
    }
}
