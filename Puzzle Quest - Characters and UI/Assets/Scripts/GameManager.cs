﻿using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	#region Member Variables

	/// <summary>
	/// Indicate if enemy is dead.
	/// </summary>
	private bool isEnemyDead;

	/// <summary>
	/// Indicate if it's the player's turn
	/// </summary>
	public bool isPlayerTurn;

	/// <summary>
	/// The player game object.
	/// </summary>
	public GameObject player;

	/// <summary>
	/// The enemy game object.
	/// </summary>
	public GameObject enemy;

	/// <summary>
	/// The chest.
	/// </summary>
	public GameObject chest;

	/// <summary>
	/// The user interface manager.
	/// </summary>
	public UIManager uiManager;

	/// <summary>
	/// The left camera.
	/// </summary>
	public Image coverImage1;

	/// <summary>
	/// The right camera.
	/// </summary>
	public Image coverImage2;

	/// <summary>
	/// The timer.
	/// </summary>
	public Timer timer;


	#endregion

	#region Private Methods

	// Use this for initialization
	void Start ()
	{
		isEnemyDead = false;
		EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth> ();
		enemyHealth.EnemyDefeated += new EventHandler (EnemyDefeatedHandler);
		timer.TimesUp += new EventHandler (TimesUpHandler);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isEnemyDead)
		{
			chest.SetActive (true);
		}
	}

	/// <summary>
	/// The coroutine when the enemy dies.
	/// </summary>
	/// <returns>.</returns>
	private IEnumerator EnemyDefeatedCoroutine()
	{
		isEnemyDead = true;
		chest.SetActive (true);

		yield return new WaitForSeconds (3.0f);
		uiManager.EnablePopUpWindow ();
	}

	/// <summary>
	/// Handler for enemy defeated event.
	/// Set the treasure box to visible.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
	private void EnemyDefeatedHandler(object sender, EventArgs e)
	{
		isEnemyDead = true;
		StartCoroutine (EnemyDefeatedCoroutine ());
	}

	/// <summary>
	/// Handler for when the time is up.
	/// Switch turn and restart the timer.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
	private void TimesUpHandler(object sender, EventArgs e)
	{
		coverImage1.gameObject.SetActive (!coverImage1.gameObject.activeSelf);
		coverImage2.gameObject.SetActive (!coverImage2.gameObject.activeSelf);
		StartCoroutine (TimesUpCoroutine ());
	}

	/// <summary>
	/// Times up coroutine.
	/// </summary>
	/// <returns>The up coroutine.</returns>
	private IEnumerator TimesUpCoroutine()
	{
		if (!isPlayerTurn)
		{
			uiManager.DisplayTurn (!isPlayerTurn);
		}

		yield return new WaitForSeconds (5.0f);

		if (isPlayerTurn)
		{
			uiManager.DisplayTurn (!isPlayerTurn);
		}
		yield return new WaitForSeconds (1.5f);
		isPlayerTurn = !isPlayerTurn;
	}

	#endregion
}