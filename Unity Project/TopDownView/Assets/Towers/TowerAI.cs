using UnityEngine;
using System.Collections;
using System.Reflection;

public class TowerAI : Building {

    public projectileAI projectileSource;

    public int towerDamage;
    public int cost;

    public float attackSpd;
    public bool attacksGround;
    public bool attacksAir;
    public float attackRange;

    public int drainDamage;
    public float drainSpd;
    public float drainDuration;

    public bool revealsInvisible;

    private float timer;
    protected AudioSource aSource;

    public int[] Levels { get; set; }

    // Use this for initialization
    public TowerAI () {
        attackSpd = 0.5f;
        attackRange = 1.0f;
        attacksGround = true;

        attacksAir = false;
		revealsInvisible = false;
        towerDamage = 50;
        drainDamage = 0;
        drainSpd = 0;
        drainDuration = 0;

        timer = Time.time + attackSpd;

        Levels = new int[3];
        Levels[0] = 1;
        Levels[1] = 1;
        Levels[2] = 1;
    }

    void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    void Update () {
        ExecuteTowerAI();
    }

    /// <summary>
    /// The tower does the following steps:
    /// 1. Check if the target is able to attack ((timer > attackSpd) - Attack Speed)
    /// 2. Check if they are enemies nearby (detectEnemies() - AttackRange)
    /// 3. Shoot the target if applicable (shootTarget(target.gameObject) - Spawn Projectile)
    /// 4. Decrement timer by Attack Speed
    /// </summary>
    protected void ExecuteTowerAI()
    {
        if (operating)
        {
            EnemyAI target = null;

            if (Time.time >= timer)
            {
                timer = Time.time + attackSpd;
                target = detectEnemies();

                if (target != null)
                    shootTarget(target.gameObject);
            }
        }
    }

    /// <summary>
    /// Creates a projectile object that will fly towards the target
    /// </summary>
    /// <param name="target"></param>
    public virtual projectileAI shootTarget(GameObject target)
    {
        projectileAI temp = (projectileAI) projectileAI.Instantiate(projectileSource, transform.position, transform.rotation);
        temp.transform.parent = transform;
        temp.damage = towerDamage;
        temp.drainDamage = drainDamage;
        temp.drainSpd = drainSpd;
        temp.drainDuration = drainDuration;
        temp.target = target;
        aSource.PlayOneShot(aSource.clip, 0.9f);
        return temp;
    }

    /// <summary>
    /// Scans for the nearest enemy within attack range, and returns the closest enemy as the target
    /// </summary>
    /// <returns></returns>
    public virtual EnemyAI detectEnemies()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("MONSTER");
        EnemyAI target = null;

        float distance = attackRange;

        if (targets.Length > 0)
        {
            // Get the nearest target and shoot at it
            foreach (GameObject obj in targets)
            {
                EnemyAI enemy = obj.GetComponent<EnemyAI>();

                if (enemy == null)
                    continue;

                if (!((attacksGround && enemy.isGround) || (attacksAir && !enemy.isGround)) || !enemy.isVisible)
                    continue;

                float targetDist = Vector2.Distance(transform.position, obj.transform.position);

                if (targetDist < distance)
                {
                    distance = targetDist;
                    target = enemy;
                }
            }
        }

        return target;
    }

	/// <summary>
	/// Upgrades a property of the tower.
	/// </summary>
	/// <param name="propertyName">Name of property of tower.</param>
	/// <param name="newVal">New value of property.</param>
	public void upgradeTower(string propertyName, object newVal)
	{
        switch (propertyName)
        {
            case "towerDamage": towerDamage = (int)newVal; Levels[0]++; break;
            case "attackSpd": attackSpd = (float)newVal; Levels[1]++; break;
            case "attackRange": attackRange = (float)newVal; Levels[2]++; break;
        }
    }

    /// <summary>
    /// Returns Information of tower based on propertyName specified
    /// </summary>
    /// <param name="propertyName"></param>
    public object getTowerInfo(string propertyName)
    {
        switch (propertyName)
        {
            case "towerDamage": return towerDamage;
            case "attackSpd": return attackSpd;
            case "attackRange": return attackRange;
        }

        return null;
    }

    /// <summary>
    /// Created by Joel
    /// </summary>
    /// <returns></returns>
    public string ToolTip()
    {
        string toolTipContents = "";
        toolTipContents += name.Substring(0, name.Length - 5);
        toolTipContents += "\nCost: " + cost;
        toolTipContents += "\nDamage: " + towerDamage;
        toolTipContents += "\nAttack Speed: " + attackSpd;
        toolTipContents += "\nAttack Range: " + attackRange;
        if (attacksGround && attacksAir)
        {
            toolTipContents += "\nAttacks Air & Ground Enemies";
        }
        else if (attacksGround && !attacksAir)
        {
            toolTipContents += "\nAttacks Ground Enemies";
        }
        else
        {
            toolTipContents += "\nAttacks Air Enemies";
        }
        return toolTipContents;
    }

    /// <summary>
    /// When Tower is clicked, display upgrade options onto the panel
    /// </summary>
    public void OnMouseDown()
    {
        GUI_PanelInterface panel = GameObject.FindGameObjectWithTag("GUI_PANEL").GetComponent<GUI_PanelInterface>();

        panel.showTowerInfo(this);
    }
}
