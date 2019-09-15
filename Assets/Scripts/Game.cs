using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    Interface intWindow;

    [Header("Lists GameObjects")]
    List<GameObject> goods = new List<GameObject>();
    List<GameObject> evils = new List<GameObject>();

    [Header("Lists Units")]
    public List<Unit> good_units = new List<Unit>();
    public List<Unit> evil_units = new List<Unit>();

    [Header("List All Ability")]
    public List<Ability> all_abilites = new List<Ability>();

    [Header("Unit UI")]
    public AbilityUI ability_ui;

    public GameObject evil_units_ui;
    public GameObject good_units_ui;

    public GameObject mini_status_panel;
    public GameObject mini_status;

    [Header("GameObject Director")]
    public GameObject boss;

    bool is_turn_end = true;
    bool is_battle_end {
        get {
            return good_units.Count == 0 || evil_units.Count == 0;
        }
    }
    void Awake()
    {
        LoadAllAbilites();
    }
    void Start()
    {
        intWindow = GameObject.Find("Interface").GetComponent<Interface>();
        is_turn_end = true;
        FindUnitAddList();
        InitUnitsUI(evil_units, evil_units_ui);
        InitUnitsUI(good_units, good_units_ui);

    }

    void FindUnitAddList()
    {
        goods.AddRange(GameObject.FindGameObjectsWithTag("hero"));
        evils.AddRange(GameObject.FindGameObjectsWithTag("evil"));

        for (int i = 0; i < goods.Count; i++)
        {
            good_units.Add(goods[i].gameObject.GetComponent<Unit>());
        }
        for (int i = 0; i < evils.Count; i++)
        {
            evil_units.Add(evils[i].gameObject.GetComponent<Unit>());
        }

    }

    void LoadAllAbilites()
    {
        all_abilites = new List<Ability>(Resources.LoadAll<Ability>("Abilites"));
    }

    void MarkUnit(GameObject units_ui, Ability ability, System.Func<float, bool> check)
    {
        for (int i = 0; i < units_ui.transform.childCount; ++i)
        {
            var unit_ui = units_ui.transform.GetChild(i).gameObject;
            if (!unit_ui.activeSelf)
                continue;

            var unit_ui_button = unit_ui.GetComponent<Button>();
            unit_ui_button.interactable = ability == null || check(ability.damage);
        }
    }

    void InitUnitsUI(List<Unit> units, GameObject units_ui)
    {
        int i;
        for (i = 0; i < units.Count; ++i)
        {
            GameObject unit_ui;
            if (i < units_ui.transform.childCount)
            {
                unit_ui = units_ui.transform.GetChild(i).gameObject;
            }
            else
                unit_ui = GameObject.Instantiate(units_ui.transform.GetChild(0).gameObject, units_ui.transform);
            unit_ui.GetComponent<UnitUI>().unit = units[i];
            var unit = units[i];
            unit_ui.GetComponent<UnitUI>().UpdateUnitStatus();
            unit_ui.GetComponent<Animations>().PlayAnimation(AnimationType.IDLE);
            unit_ui.GetComponent<Button>().onClick.AddListener(delegate () {
                ability_ui.target = unit;
            });
        }

        for (; i < units_ui.transform.childCount; ++i)
            units_ui.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void DoneAllStatusNew()
    {
        for (int i = 0; i < good_units.Count; i++)
        {
            for (int j = 0; j < good_units[i].statuses.Count; j++)
            {
                bool is_new_ability = good_units[i].statuses[j].is_new;
                if (is_new_ability)
                {
                    good_units[i].statuses[j].is_new = false;

                }
            }

        }

        for (int i = 0; i < evil_units.Count; i++)
        {
            for (int j = 0; j < evil_units[i].statuses.Count; j++)
            {
                bool is_new_ability = evil_units[i].statuses[j].is_new;
                if (is_new_ability)
                {
                    evil_units[i].statuses[j].is_new = false;
                }
            }

        }
    }

    public UnitUI GetUnitUI(Unit _unit)
    {
        for (int i = 0; i < evil_units_ui.transform.childCount; i++)
        {
            UnitUI unitUI = evil_units_ui.transform.GetChild(i).GetComponent<UnitUI>();
            if (unitUI.unit == _unit)
            {
                return unitUI;
            }

        }

        for (int i = 0; i < good_units_ui.transform.childCount; i++)
        {
            UnitUI unitUI = good_units_ui.transform.GetChild(i).GetComponent<UnitUI>();
            if (unitUI.unit == _unit)
            {
                return unitUI;
            }

        }

        return null;
    }

    public void Update()
    {
        if (is_turn_end && !is_battle_end)
            StartCoroutine(Turn());
    }

    void OnAbilitySelected(Ability selected)
    {
        MarkUnit(evil_units_ui, selected, (damage) => damage > 0);
        MarkUnit(good_units_ui, selected, (damage) => damage < 0);

    }

    public IEnumerator Turn()
    {
        is_turn_end = false;
        foreach (var unit in good_units)
        {
            if (unit.GetStunStatus() != null)
            {
                unit.OnTurnStart();
                if (unit.IsUnitStatuses())
                {
                    unit.StatusDamage();
                    yield return new WaitForSeconds(1.0f);

                }
            }
            else
            {
                unit.OnTurnStart();
                boss.GetComponent<Button>().interactable = true;
                ability_ui.gameObject.SetActive(true);

                yield return ability_ui.WaitInput(unit, good_units, evil_units, OnAbilitySelected);
                if (unit.IsUnitStatuses())
                {
                    unit.StatusDamage();
                    yield return new WaitForSeconds(1.0f);
                }
                ability_ui.gameObject.SetActive(false);
                boss.GetComponent<Button>().interactable = false;
            }
            RemoveDead();
            if (GameIsEnd())
            {
                StopCoroutine(Turn());
                intWindow.ShowWindow();

            }
            yield return new WaitForSeconds(1.0f);
            unit.OnTurnEnd();

        }

        RemoveDead();
        if (GameIsEnd())
        {
            StopCoroutine(Turn());
            intWindow.ShowWindow();

        }
        foreach (var unit in evil_units)
        {

            unit.OnTurnStart();
            if (unit.IsUnitStatuses())
            {
                if (unit.GetStunStatus() == null)
                {
                    AI.MakeAction(unit, evil_units, good_units);
                }
                else
                {
                    unit.StatusDamage();

                }
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                //Debug.Log(unit.unit_name + " Нет статусов");
                AI.MakeAction(unit, evil_units, good_units);
            }
            if (unit.current_hp > 0.0f)
                yield return new WaitForSeconds(1.0f);
            unit.OnTurnEnd();
        }
        RemoveDead();
        if (GameIsEnd())
        {
            StopCoroutine(Turn());
            intWindow.ShowWindow();

        }
        is_turn_end = true;
        yield return null;

    }

    void RemoveDead()
    {
        good_units.RemoveAll(unit => unit.current_hp <= 0.0f);
        evil_units.RemoveAll(unit => unit.current_hp <= 0.0f);

    }

    bool GameIsEnd()
    {
        if (good_units.Count == 0 || evil_units.Count == 0)
        {
            return true;
        }
        return false;
    }
}

public enum AbilityType
{
    ATTACK,
    PROTECTION,
    RECOVERY,
    STUN,
    POISONING
}