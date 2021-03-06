﻿using UnityEngine;
using System.Collections;

abstract public class TutorialMovement : Movement {
	
	public float SPEED;

	private Square[,] walls;
	protected bool canTurn = false;
	protected int direction;
	public int stunTime;
	private int stunned = 0;
	protected bool playerDetected;
	protected int detectionRange;
	protected int farDetectRange;
	protected bool attacking;

	public bool canMove { get; set; }
	
	public int attackRange;
	public int closeDetectRange;

	public GameObject player;
	
	// Use this for initialization
	void Start () {
		canMove = false;
		
		direction = 3; // Quaternion.identity
		playerDetected = false;
		isClose = false;
		attacking = false;
		detectionRange = closeDetectRange;
		farDetectRange = closeDetectRange + 5;
		
       
    }
	
	// Update is called once per frame
	void Update()
	{
		// Stunned is a countdown - once the countdown is up, continue moving towards the player
		if (Network.isServer && this.canMove)
		{
			planMovement();
		}
		
	}
	private void planMovement()
	{
		if (stunned == 0)
		{
			// Move if not attacking
			if (!attacking)
			{
				// Move towards the player if in sight
				approachPlayer();

				if( Network.isServer )
					maneuver();
			}
			
			// Attack if not moving
			else
			{
				doAttack();
			}
		}
		else
		{
			stunned -= 1;
		}
	}
	abstract public void maneuver ();
	abstract public void doClose (Transform player);
	abstract public void doAttack();
	abstract public bool canAttack();

	protected void approachPlayer() {
		Transform playerTransform = player.transform;
		float distance = Vector3.Distance (new Vector3(playerTransform.position.x, 0, playerTransform.position.z), 
		                                   new Vector3(transform.position.x, 0, transform.position.z));
		if (distance >= attackRange && distance <= detectionRange) {
			isClose = false;
			detectionRange = farDetectRange;
			transform.LookAt (new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
			playerDetected = true;
		} else if(distance < attackRange) {
			isClose = true;
			doClose (playerTransform);
		}
	}
	
	public override void setAttacking(bool state) {
		if(this.isClose && this.canAttack()) {
			attacking = state;
		}
	}
	
	// Stops the monster from moving.
	public override void stun() {
		stunned = stunTime;
	}
}
