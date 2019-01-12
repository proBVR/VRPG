using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : Character
{
    [SerializeField]
    protected float interval, searchRange, attackRange, mvSpeed;
    protected bool cooling = false, attacking=false;
    protected EnemyManager manager;
    protected int actNum = 0;
    protected Animator animator;
    protected Rigidbody rb;

    [SerializeField]
    protected int dropExp, dropItem;

    private UI headUI;

    // Use this for initialization
    protected void Start()
    {
        manager = transform.parent.GetComponent<EnemyManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        headUI = new UI(this);
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!manager.ExistPlayer()) return;

        headUI.UpdateUI();

        if (attacking) return;
        //cooling = true;
        var distance = Vector3.Distance(Player.instance.transform.position, transform.position);

        if (distance < attackRange)
        {
            if (!cooling)
            {
                Action(actNum);
                attacking = true;
            }
        }
        if (distance > searchRange) Idle();
        else Move();
    }

    protected void FinAtk()
    {
        attacking = false;
        cooling = true;
        Scheduler.instance.AddEvent(interval, FinCool);
    }

    private void FinCool()
    {
        cooling = false;
    }

    protected abstract void Idle();

    protected override void Death()
    {
        Player.instance.AddExp(dropExp);
        Player.instance.inventory.AddInventory(GameManager.instance.itemList[dropItem].GetName());
    }

    private class UI
    {
        private Transform ui;
        private Enemy enemy;
        private Slider hpBar;

        public UI(Enemy enemy)
        {
            this.enemy = enemy;
            foreach(Transform child in enemy.transform)
                if(child.GetComponent<Canvas>() != null)
                    ui = child;
            foreach(Transform child in ui)
            {
                if (child.GetComponent<TextMeshProUGUI>() != null)
                    child.GetComponent<TextMeshProUGUI>().text = enemy.status.name + " Lv." + enemy.level;
                if (child.GetComponent<Slider>() != null)
                    hpBar = child.GetComponent<Slider>();
            }
        }

        public void UpdateUI()
        {
            var dir = Player.instance.transform.position - ui.position;
            dir.y = 0;
            ui.forward = dir;
            hpBar.value = enemy.status.Hp / enemy.status.MaxHp;
        }
    }
}