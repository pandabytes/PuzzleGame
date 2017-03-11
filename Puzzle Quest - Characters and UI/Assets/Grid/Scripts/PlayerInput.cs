﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	private GridManager gridManager;
	public LayerMask Tiles;
	private GameObject activeTile;
    public GameObject indicator;
	public GameManager gameManager;
	public Timer timer;
    private GameObject go;

	void Awake()
	{
		gridManager = GetComponent<GridManager> ();
		timer.TimesUp += new EventHandler (TimesUpHandler);
	}

	// Update is called once per frame
	void Update ()
	{
		// Make it unclickable when cover images are active
		if (Input.GetKeyDown (KeyCode.Mouse0) && !gameManager.coverImage1.IsActive ())
		{
			if (activeTile == null)
				SelectTile ();
			else
				AttemptMove ();
		}
        /*
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            activeTile = null;
            Destroy(go);
        }
        */
	}

	// Tries to select a tile if the players left-clicks and no other tile is selected.
	void SelectTile()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 50f, Tiles);
        if (hit)
        {
            activeTile = hit.collider.gameObject;
            go = Instantiate(indicator, new Vector2(activeTile.transform.position.x, activeTile.transform.position.y), Quaternion.identity) as GameObject;
        }
	}

	//tries to select and move a tile if the player left-clicks and another tile has already been selected
	void AttemptMove()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit2D hit = Physics2D.GetRayIntersection (ray, 50f, Tiles);
		if(hit)
		{
            if (Vector2.Distance(activeTile.transform.position, hit.collider.gameObject.transform.position) <= 1.25f)
            {
                TileControl activeControl = activeTile.GetComponent<TileControl>();
                TileControl hitControl = hit.collider.gameObject.GetComponent<TileControl>();

                GridManager.XY activeXY = activeControl.MyXY;
                GridManager.XY hitXY = hitControl.MyXY;

                activeControl.Move(hitXY);
                hitControl.Move(activeXY);

                gridManager.SwitchTiles(hitXY, activeXY);

            /*   
                if (gridManager.SwitchBack())
                {
                    activeControl = gridManager.GetTileControl(hitXY);
                    hitControl = gridManager.GetTileControl(activeXY);
                    activeXY = activeControl.MyXY;
                    hitXY = hitControl.MyXY;

                    activeControl.Move(hitXY);
                    hitControl.Move(activeXY);

                    gridManager.SwitchTiles(hitXY, activeXY);

                }
              */ 
            }
                activeTile = null;
                Destroy(go);
    	}        
	}

	/// <summary>
	/// Handler for when the time expires.
	/// Deselect a tile once the players run out of time.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
	private void TimesUpHandler(object sender, EventArgs e)
	{
		if (activeTile != null)
		{
			activeTile = null;
			Destroy (go);
		}
	}
}
