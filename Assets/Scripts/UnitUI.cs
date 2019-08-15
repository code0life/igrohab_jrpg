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
  public GameObject status_count;
  public GameObject status_element;

  Animation anim;

    //public List<Ability> statuses = new List<Ability>();


    public float prev_hp = -1;

    public void Start()
    {
        anim = GetComponent<Animation>();
        anim.Play("Evil_static");
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

    if(unit.current_hp <= 0.0f)
      gameObject.SetActive(false);

    name.text = unit.unit_name;
    hp.text = $"HP: {unit.current_hp}/{unit.max_hp}";
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

                //end_status_ui = status_panel.transform.GetChild(j).gameObject;
                //Log(end_status_ui.gameObject.GetComponent<UnitUI>().unit.unit_name);
                //end_status_ui.GetComponent<UnitUI>()._unit.unit_name;
                //if (end_status_ui.GetComponent<UnitUI>()._unit == null)
                //{
                //    Debug.Log(end_status_ui);
                //    Destroy(end_status_ui);
                //}
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
                //Debug.Log( "Есть на кого повесить - " + unit.unit_name );
 
                //Debug.Log( "++++++++++++++++ ");

                //if (unit_status_ui.gameObject.GetComponent<UnitUI>()._unit.statuses == AbilityType.POISONING)
                //{
                //    unit_status_ui.GetComponent<Image>().color = Color.green;
                //    //Debug.Log("Красим в зелёный " + unit.statuses[i].name + " объекта " + unit_status_ui.name);
                //}
                //else if (unit.statuses[i].type == AbilityType.PROTECTION)
                //{
                //    unit_status_ui.GetComponent<Image>().color = Color.blue;
                //    //Debug.Log("Красим в синий " + unit.statuses[i].name);
                //}
                //else if (unit.statuses[i].type == AbilityType.STUN)
                //{
                //    unit_status_ui.GetComponent<Image>().color = Color.red;
                //    //Debug.Log("Красим в красный " + unit.statuses[i].name);
                //}

                //Debug.Log("В " + unit.unit_name + _unit.statuses.Count + " статусов ");

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
            //else
            //{
            //    unit_status_ui = GameObject.Instantiate(status_element, status_panel.transform);
            //    unit_status_ui = status_panel.transform.GetChild(i).gameObject;
            //    unit_status_ui.SetActive(true);
            //    status_count.GetComponent<Text>().text = unit.statuses[i].duration.ToString();

            //    if (_unit.statuses[i].type == AbilityType.POISONING)
            //    {
            //        unit_status_ui.GetComponent<Image>().color = Color.green;
            //    }
            //    else if (_unit.statuses[i].type == AbilityType.PROTECTION)
            //    {
            //        unit_status_ui.GetComponent<Image>().color = Color.blue;
            //        //Debug.Log("Красим в синий " + unit.statuses[i].name);
            //    }
            //    else if (_unit.statuses[i].type == AbilityType.STUN)
            //    {
            //        unit_status_ui.GetComponent<Image>().color = Color.red;
            //        //Debug.Log("Красим в красный " + unit.statuses[i].name);
            //    }

                //UpdateUnitStatus();
                //if (unit.statuses[i].type == AbilityType.POISONING)
                //{
                //    unit_status_ui.GetComponent<Image>().color = Color.green;
                //    Debug.Log("2Красим в зелёный " + unit.statuses[i].name);
                //}
                //else if (unit.statuses[i].type == AbilityType.PROTECTION)
                //{
                //    unit_status_ui.GetComponent<Image>().color = Color.blue;
                //    Debug.Log("2Красим в синий " + unit.statuses[i].name);
                //}
                //else if (unit.statuses[i].type == AbilityType.STUN)
                //{
                //    unit_status_ui.GetComponent<Image>().color = Color.red;
                //    Debug.Log("2Красим в красный " + unit.statuses[i].name + " объекта " + unit_status_ui.name);
                //}
                ////Debug.Log("Обновляем счетчик у " + unit.unit_name + " на " + unit.statuses[i].duration.ToString());
                ////    //unit_ui.GetComponent<UnitUI>().unit.statuses[j] = 
                //Debug.Log("Обновляем счетчик у " + unit.unit_name + " скилла " + unit.statuses[i].name + " на " + unit.statuses[i].duration.ToString() + " в место " + status_count.GetComponent<Text>().text);
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
