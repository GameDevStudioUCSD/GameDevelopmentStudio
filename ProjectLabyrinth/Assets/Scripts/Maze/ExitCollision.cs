﻿using UnityEngine;
using System.Collections;


public class ExitCollision : AbstractGUI {

    public GameObject loadResult;
    private bool hasReached;
	private PlayerCharacter player;
	public GameObject skPrefab;

    Rect win = new Rect(frameX + frameWidth/2, frameHeight / 2, 100, 100);

	
	void OnTriggerEnter (Collider other)
	{
		HitDetector hitDetector = (HitDetector)other.gameObject.GetComponent("HitDetector");
		if (hitDetector) {
            Instantiate(loadResult, new Vector3(0, 0, 0), Quaternion.identity);
			this.player = (PlayerCharacter) hitDetector.GetComponentInParent<PlayerCharacter>();
			this.hasReached = true;
			skPrefab.GetComponent<ScoreKeeper>().stats[0].win = true;
<<<<<<< HEAD
            Instantiate(loadResult, new Vector3(0, 0, 0), Quaternion.identity);
=======
>>>>>>> bc3897313e3ea4ac4a946f5481c7226f4f28fe39
			player.data.win = true;
		}
	}
    

}