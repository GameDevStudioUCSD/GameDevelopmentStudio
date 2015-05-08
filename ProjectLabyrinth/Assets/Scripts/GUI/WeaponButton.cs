﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponButton : MonoBehaviour {

	private Image image;
	public Color disabledColor;
	public Color enabledColor;
	public Color cooldownColor;

	public bool active { get; set; }
	public bool cooldown { get; set; }

	public Weapon weapon { get; set; }
	public Weapon[] weaponList;

	private int cooldownCount = 0;

	// Use this for initialization
	void Start () {
		image = (Image) gameObject.GetComponent<Image>();
		active = false;
		cooldown = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(cooldownCount > 0) {
			cooldownCount--;
			if(cooldownCount == 0) {
				cooldown = false;
				deactivate ();
			}
		}
	}

	public void setWeapon(int num) {
		gameObject.SetActive (true);
		for(int i = 0; i < weaponList.Length; i++) {
			weaponList[i].gameObject.SetActive(false);
		}

		weaponList [num].gameObject.SetActive (true);
		weapon = weaponList [num];
	}

	public void removeWeapon() {
		gameObject.SetActive (false);
		for(int i = 0; i < weaponList.Length; i++) {
			weaponList[i].gameObject.SetActive (false);
		}
	}

	void deactivate() {
		active = false;
		image.color = disabledColor;

		if(weapon != null)
			weapon.deactivate ();
	}

	void activate() {
		active = true;
		image.color = enabledColor;

		if(weapon != null)
			weapon.activate ();
	}

	public void setCooldown() {
		active = false;
		cooldown = true;
		cooldownCount = weapon.cooldown;
		image.color = cooldownColor;

		if(weapon != null)
			weapon.deactivate ();
	}

	public void onClick() {
		if(active) {
			deactivate ();
		}

		else if(!active && !cooldown) {
			activate();
		}
	}
}