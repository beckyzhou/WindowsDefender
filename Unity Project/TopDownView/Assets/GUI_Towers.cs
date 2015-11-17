﻿using UnityEngine;
using System.Collections;

/// <summary>
/// @Author Calvin Truong
/// @Version 1.0
/// 
/// @Description: When the button ("btn_Towers) is clicked, call the onClick() function
/// which will proceed to create an array of tower buttons based on the towers in the
/// towers array.
/// 
/// </summary>
public class GUI_Towers : MonoBehaviour {

    public TowerAI[] towers;
    public ButtonScript buttonTemplate;
    public GameObject panel;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Instantiates tower buttons based on the number of elements in the towers array.
    /// The tower buttons's text is based on the element's name.
    /// </summary>
    public void onClick()
    {
        // Hard-coded value
        int offsetX = 18;

        // Get the Panel's Position
        Vector3 panelPosition = panel.transform.position;

        // Get the Button's Rect Transformation
        Rect buttonRect = buttonTemplate.gameObject.GetComponent<RectTransform>().rect;

        // Get the Panel's Rect Transformation
        Rect panelRect = panel.gameObject.GetComponent<RectTransform>().rect;

        for (int i = 0; i < towers.Length; i++)
        {
            Vector3 newPosition = panelPosition;
            Vector3 spawnPosition = panelPosition;
            // We set the x position of the new button
            newPosition.x = 0 - panelRect.xMin + offsetX + buttonRect.width * i;
            spawnPosition.x = 0 - panelRect.xMin + offsetX;

            // Create the new button
            ButtonScript newButton = (ButtonScript) Instantiate(buttonTemplate, spawnPosition, transform.rotation);

            // Name of the object on Unity Editor
            newButton.name = towers[i].name;

            // Text of the object to display in the game
            newButton.gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = towers[i].name;

            // Set the parent of the object to this object (GUI_Towers)
            // If you don't then the object isn't part of the canvas / child of canvas
            newButton.transform.SetParent(panel.transform);

            // Set the position for the button to slide to
            newButton.MoveLocation = newPosition;

            // Assign the button to the game controller
            newButton.GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<BuildMode>();

            newButton.Tower = towers[i];
        }   
    }
}
