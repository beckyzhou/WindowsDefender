﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public LinkedList<Vector2> movementPoints;

    public float movementSpeed;
    public int drainDamage;
    public float drainSpd;

    public int health;    
    public bool isVisible;
    public bool isGround;
    public int armour;
    public double resistance;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (movementPoints != null && movementPoints.Count > 0)
        {
            Vector2 first = movementPoints.First.Value;

            float dist = Vector2.Distance(transform.position, first);

            if (dist <= 0.1)
                movementPoints.RemoveFirst();
            else
                transform.position = Vector2.MoveTowards(transform.position, first, movementSpeed * Time.deltaTime);
        }
	}

    //Method to drain enemy's health
    void DrainHealth()
    {
        health -= drainDamage;
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "PROJECTILE")
        {
            projectileAI projectile = obj.gameObject.GetComponent<projectileAI>();

            if (projectile.target == this.gameObject)
            {
                int damage;

                if((damage = projectile.damage - armour) > 0)
                    health -= damage;

                drainDamage = projectile.drainDamage;
                drainSpd += projectile.drainSpd;
                if (drainSpd != 0) InvokeRepeating("DrainHealth", 3, drainSpd);

                Destroy(obj.gameObject);
            }

        }
    }
}