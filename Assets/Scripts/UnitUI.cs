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

    [Header( "Unit" ) ]
    new public Text name;
    public Text hp;
    public Text popup;
    public Text phrase;
    public float prev_hp = -1;

    [Header("Unit Statuses")]

    public GameObject status_panel;
    public GameObject status_count;
    public GameObject status_element;

    Animations anim;

    [Header("Unit Health Bar")]
    public Image health_bar;

    public void Start()
    {
    }

    void Update()
    {


    if (unit == null)
      return;

    if(prev_hp != unit.current_hp)
    {
      ShowDamage(prev_hp - unit.current_hp);
      prev_hp = unit.current_hp;
    }

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

        for (i = 0; i < _unit.statuses.Count; ++i)
        {
            GameObject unit_status_ui;

            if (i < status_panel.transform.childCount)
            {

                for (int si = 0; si < _unit.statuses.Count; si++)
                {

                    unit_status_ui = status_panel.transform.GetChild(si).gameObject;
                    unit_status_ui.SetActive(true);
                    unit_status_ui.GetComponentInChildren<Text>().text = unit_status_ui.GetComponentInParent<UnitUI>()._unit.statuses[si].duration.ToString();

                    if (unit_status_ui.GetComponentInParent<UnitUI>()._unit.statuses[si].type == AbilityType.POISONING)
                    {
                        unit_status_ui.GetComponent<Image>().color = Color.green;
                    }
                    if (unit_status_ui.GetComponentInParent<UnitUI>()._unit.statuses[si].type == AbilityType.STUN)
                    {
                        unit_status_ui.GetComponent<Image>().color = Color.red;
                    }
                    if (unit_status_ui.GetComponentInParent<UnitUI>()._unit.statuses[si].type == AbilityType.PROTECTION)
                    {
                        unit_status_ui.GetComponent<Image>().color = Color.yellow;
                    }
                }
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

    public void ClickUnit()
    {
        if (GetComponent<Button>().interactable == true)
        {
            string _text = null;
            StartCoroutine(PhraseCoroutine(_text));
            GetComponent<Button>().interactable = false;
            phrase.GetComponent<Text>().text = "MEOW!";
        }

    }

    IEnumerator PhraseCoroutine(string _text)
    {
        yield return new WaitForSeconds(0.6f);
        phrase.GetComponent<Text>().text = _text;
        GetComponent<Button>().interactable = true;
    }
}
