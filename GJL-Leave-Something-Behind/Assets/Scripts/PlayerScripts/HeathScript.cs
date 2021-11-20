/////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Filename:        HealthScript.cs
/// Author:          Jack Kellett
/// Date Created:    20/11/2021
/// Brief:           To catalog how much health something has and to allow an object to be damaged or healed.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (SerializeFields).
	[SerializeField]
	private int maxHealth = 100;

	[SerializeField]
	private int highestUpgradedHealth = 500;
	#endregion

	#region Private Variable Declarations.

	private bool isDead = false;
	private int currentHealth = 100;
	private float currentHealthPercentage = 100.0f;
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start() {
		maxHealth = GetMaxHealthPlayerPref(maxHealth);
		currentHealth = maxHealth;
		currentHealthPercentage = CalculateHealthPercentage();
	}

	// Update is called once per frame
	void Update() {
		if (currentHealth <= 0) {
			isDead = true;
			Debug.Log("LOG::Player killed!!");
		}
	}

	/// <summary>
	/// Checks the current max health player pref and gets it for the game during start.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	private int GetMaxHealthPlayerPref(int value) {
		if (!PlayerPrefs.HasKey("maxHealth")) {
			PlayerPrefs.SetInt("maxHealth", value);
		} else {
			value = PlayerPrefs.GetInt("maxHealth");
		}

		return value;
	}

	/// <summary>
	/// Updates the max health player pref to the current maxHealth value.
	/// </summary>
	private void UpdateHealthPlayerPref() {
		PlayerPrefs.SetInt("maxHealth", maxHealth);
	}

	private float CalculateHealthPercentage() {
		float percentage = ((float)currentHealth / (float)maxHealth) * 100.0f;
		Debug.Log("Current Health Percentage: " + percentage);
		return percentage;
	}
	#endregion

	#region Public Access Functions (Getters and Setters).

	public void HealToMax() {
		currentHealth = maxHealth;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
		currentHealthPercentage = CalculateHealthPercentage();
		Debug.Log("LOG::Player healed to max!!");
	}

	public void HealByInt(int health) {
		currentHealth += health;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
		currentHealthPercentage = CalculateHealthPercentage();
		Debug.Log("LOG::Player healed by " + health + " points!!");
	}

	public void IncreaseMaxHealth(int increment) {
		maxHealth += increment;
		maxHealth = Mathf.Clamp(maxHealth, 0, highestUpgradedHealth);
		UpdateHealthPlayerPref();
		HealToMax();
		currentHealthPercentage = CalculateHealthPercentage();
		Debug.Log("LOG::Max health increased by " + increment + " points!!");
	}

	public void DamageByInt(int health) {
		currentHealth -= health;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
		currentHealthPercentage = CalculateHealthPercentage();
		Debug.Log("LOG::Player damaged by " + health + " points!!");
	}

	public float GetCurrentHealthPercentage() {
		return currentHealthPercentage;
	}

	public bool GetDeathState() {
		return isDead;
	}
	#endregion
}