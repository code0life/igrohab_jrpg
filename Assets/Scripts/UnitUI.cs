using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;

public class UnitUI : MonoBehaviour
{
    Unit _unit;
    public Unit unit {
        set {
            _unit = value;
            prev_hp = _unit.current_hp;
        }
        get {
            return _unit;
        }
    }

    //public UnitUI unitUI
    //{
    //    set
    //    {
    //    }
    //    get
    //    {
    //        return this;
    //    }
    //}

    [Header( "Unit" ) ]
    new public Text name;
    public Text hp;
    public Text popup;
    public float prev_hp = -1;

    [Header("Unit Statuses")]

    public GameObject status_panel;
    public GameObject status_count;
    public GameObject status_element;

    Animations anim;

    [Header("Unit Health Bar")]
    public Image health_bar;

    //public List<Ability> statuses = new List<Ability>();




    public void Start()
    {
        //anim = GetComponent<Animator>();
        //anim.PlayAnimation(this, AnimationType.IDLE);
        //anim.Play("evil_static");
    }

    void Update()
  {

    //UpdateUnitStatus();
    if (unit == null)
      return;

    if(prev_hp != unit.current_hp)
    {
      ShowDamage(prev_hp - unit.current_hp);
      prev_hp = unit.current_hp;
    }

    //if(unit.current_hp <= 0.0f)
    //  gameObject.SetActive(false);

    name.text = unit.unit_name;
    hp.text = $"{unit.current_hp}/{unit.max_hp}";
  }

    public void SetDeadState()
    {
        if(unit.current_hp <= 0.0f)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void UpdateUnitStatus()
    {

        int j;

        GameObject end_status_ui;

        for (j = 0; j < status_panel.transform.childCount; j++)
        {
            //Debug.Log(status_panel.transform.childCount + " СТОЛЬКО ДЕТЕЙ");
            if (status_panel.transform.childCount > _unit.statuses.Count)
            {
                end_status_ui = status_panel.transform.GetChild(j).gameObject;
                end_status_ui.SetActive(false);

            }
            else
            {
                end_status_ui = GameObject.Instantiate(status_element, status_panel.transform);
                end_status_ui = status_panel.transform.GetChild(j).gameObject;
                end_status_ui.SetActive(true);
            }
        }

        int i;
        //Debug.Log(" ============= UpdateUnitStatus - " + unit.unit_name);

        for (i = 0; i < _unit.statuses.Count; ++i)
        {
            //Debug.Log("Тип статуса - " + _unit.statuses[i].type);

            GameObject unit_status_ui;
            //Debug.Log( i );

            if (i < status_panel.transform.childCount)
            {

                for (int si = 0; si < _unit.statuses.Count; si++)
                {
                    //Debug.Log("_unit.statuses[i].type - " + _unit.statuses[si].type);

                    unit_status_ui = status_panel.transform.GetChild(si).gameObject;
                    unit_status_ui.SetActive(true);
                    unit_status_ui.GetComponentInChildren<Text>().text = unit_status_ui.GetComponentInParent<UnitUI>()._unit.statuses[si].duration.ToString();

                    //Debug.Log("unit_status_ui.GetComponentInParent<UnitUI>()._unit.statuses[si].type - " + unit_status_ui.GetComponentInParent<UnitUI>()._unit.statuses[si].type);
                    if (unit_status_ui.GetComponentInParent<UnitUI>()._unit.statuses[si].type == AbilityType.POISONING)
                    {
                        unit_status_ui.GetComponent<Image>().color = Color.green;
                        //Debug.Log("Красим в зелёный " + _unit.statuses[si].type);
                    }
                    if (unit_status_ui.GetComponentInParent<UnitUI>()._unit.statuses[si].type == AbilityType.STUN)
                    {
                        unit_status_ui.GetComponent<Image>().color = Color.red;
                        //Debug.Log("Красим в красный " + _unit.statuses[si].type);
                    }
                    if (unit_status_ui.GetComponentInParent<UnitUI>()._unit.statuses[si].type == AbilityType.PROTECTION)
                    {
                        unit_status_ui.GetComponent<Image>().color = Color.yellow;
                        //Debug.Log("Красим в желтый " + _unit.statuses[si].type);
                    }
                }
                   // Debug.Log("Обновляем счетчик у " + unit.unit_name + " скилла " + unit.statuses[i].name + " на " + unit.statuses[i].duration.ToString() + " в место " + status_count.GetComponent<Text>().text);
            }

        }

    }

    void ShowDamage(float damage)
    {
        var new_popup = Text.Instantiate(popup, popup.transform.parent);
        new_popup.text = $"{-damage}";
        if(damage > 0)
            new_popup.color = Color.red;
        else
            new_popup.color = Color.green;
        new_popup.gameObject.AddComponent<PopupText>();
        new_popup.gameObject.SetActive(true);
    }
}
