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

  new public Text name;
  public Text hp;
  public Text popup;

  public GameObject status_panel;
  //public GameObject status_panel;
  //public List<Ability> statuses = new List<Ability>();


  public float prev_hp = -1;

  void Update()
  {
    if(unit == null)
      return;

    if(prev_hp != unit.current_hp)
    {
      ShowDamage(prev_hp - unit.current_hp);
      prev_hp = unit.current_hp;
    }

    if(unit.current_hp <= 0.0f)
      gameObject.SetActive(false);

    name.text = unit.unit_name;
    hp.text = $"HP: {unit.current_hp}/{unit.max_hp}";
  }

    public void UpdateUnitStatus()
    {
        int i;
        Debug.Log("this.statuses.Count - " + _unit.statuses.Count);
        for (i = 0; i < _unit.statuses.Count; ++i)
        {
            Debug.Log("Тип статуса - " + _unit.statuses[i].type);
            //GameObject unit_status_ui;
            //if (i < mini_status_panel.transform.childCount)
            //{
            //    unit_status_ui = mini_status_panel.transform.GetChild(j).gameObject;
            //    Debug.Log("В " + units[i].unit_name + " создаём присваиваем ");
            //}
            //else
            //{
            //    Debug.Log("В " + units[i].unit_name + " создаём статус ");
            //    //unit_status_ui
            //    //Debug.Log("В " + units[i].unit_name + " создаём статус " + unit_status_ui.name);
            //    //unit_status_ui = GameObject.Instantiate(unit_ui.GetComponent<UnitUI>().unit.statuses[0].transform.GetChild(0).gameObject, unit_ui.GetComponent<UnitUI>().unit.transform);
            //    //unit_ui.GetComponent<UnitUI>().unit = units[i];
            //    //unit_ui.GetComponent<UnitUI>().unit.statuses[j] = 
            //}

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
