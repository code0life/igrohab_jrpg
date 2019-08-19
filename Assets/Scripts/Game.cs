using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
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
    for(int i = 0; i < units_ui.transform.childCount; ++i)
    {
      var unit_ui = units_ui.transform.GetChild(i).gameObject;
            //Debug.Log(unit_ui.name);
      if(!unit_ui.activeSelf)
        continue;

      var unit_ui_button = unit_ui.GetComponent<Button>();
      unit_ui_button.interactable = ability == null || check(ability.damage);
    }
  }

    void InitUnitsUI(List<Unit> units, GameObject units_ui)
    {
        //Debug.Log("Init");
        int i;
        for (i = 0; i < units.Count; ++i)
        {
            GameObject unit_ui;
            if (i < units_ui.transform.childCount)
            {
                unit_ui = units_ui.transform.GetChild(i).gameObject;
                //anim.PlayAnimation(unit_ui.GetComponent<UnitUI>().unitUI, AnimationType.IDLE);
            }
            else
                unit_ui = GameObject.Instantiate(units_ui.transform.GetChild(0).gameObject, units_ui.transform);
            unit_ui.GetComponent<UnitUI>().unit = units[i];
            var unit = units[i];
            unit_ui.GetComponent<UnitUI>().UpdateUnitStatus();
            //UnitUI uUI = GetUnitUI(units[i]);
            //Debug.Log("uUI - " + unit_ui.GetComponent<UnitUI>().unit.tag);
            unit_ui.GetComponent<Animations>().PlayAnimation(AnimationType.IDLE);
            //anim.PlayAnimation(unit_ui.GetComponent<UnitUI>().unit, AnimationType.IDLE);
            //unit_ui.gameObject.
            ///GameObject unit_status_ui;

            unit_ui.GetComponent<Button>().onClick.AddListener(delegate () {
                ability_ui.target = unit;
            });
        }

        for (; i < units_ui.transform.childCount; ++i)
            units_ui.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void DoneAllStatusNew()
    {
        //Debug.Log("evil_units_ui.transform.childCount - " + evil_units_ui.transform.childCount);
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
        //Debug.Log("evil_units_ui.transform.childCount - " + evil_units_ui.transform.childCount);
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
    if(is_turn_end && !is_battle_end)
      StartCoroutine(Turn());
  }

  void OnAbilitySelected(Ability selected)
  {
        //if (selected != null)
        //{
        //    Debug.Log("OnAbilitySelected - " + selected.name);
        //    Debug.Log("OnAbilitySelected - " + selected.damage);
        //}
        //MarkUnit(evil_units_ui, selected, (damage) => damage == 0);
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
            //Debug.Log(unit.unit_name + " ИГра говорит что гирок под станом, идёт подсчет статусов и пропуск хода");
            unit.OnTurnStart();
            //boss.GetComponent<Button>().interactable = true;
            if (unit.IsUnitStatuses())
            {
                //Debug.Log(unit.unit_name + " Есть статусы у игрока, анализ пассивного урона");
                unit.StatusDamage();
                yield return new WaitForSeconds(1.0f);
            }
        }
        else
        {
            //Debug.Log(unit.unit_name + " ИГра говорит тчо юнит СО СТАНОМ, ход не вычисляеца, но рассчитывается дамаг по статусам");
            unit.OnTurnStart();
            boss.GetComponent<Button>().interactable = true;
            ability_ui.gameObject.SetActive(true);

            //if (unit.IsUnitStatuses())
            //{
            //    Debug.Log(unit.unit_name + " Есть статусы у игрока, анализ пассивного урона");
            //    unit.StatusDamage();
            //    yield return new WaitForSeconds(1.0f);
            //}
            yield return ability_ui.WaitInput(unit, good_units, evil_units, OnAbilitySelected);
            //boss.GetComponent<Button>().interactable = false;
            if (unit.IsUnitStatuses())
            {
                //Debug.Log(unit.unit_name + " Есть статусы у игрока, анализ пассивного урона");
                unit.StatusDamage();
                yield return new WaitForSeconds(1.0f);
            }
            ability_ui.gameObject.SetActive(false);
                boss.GetComponent<Button>().interactable = false;
            }
        //unit.CheckCountStatus();
        yield return new WaitForSeconds(1.0f);
        unit.OnTurnEnd();
    }

    RemoveDead();
    foreach(var unit in evil_units)
    {

        unit.OnTurnStart();
        if (unit.IsUnitStatuses())
        {
            Debug.Log(unit.unit_name + " Есть статусы, анализ пассивного урона");
            if (unit.GetStunStatus() == null)
            {
                Debug.Log(unit.unit_name + " ИГра говорит тчо юнит без стана, ход вычисляеца");
                AI.MakeAction(unit, evil_units, good_units);
                //unit.CheckCountStatus();
            }
            else
            {
                Debug.Log(unit.unit_name + " ИГра говорит тчо юнит СО СТАНОМ, ход не вычисляеца, но рассчитывается дамаг по статусам");
                unit.StatusDamage();

            }
                //unit.CheckCountStatus();
                yield return new WaitForSeconds(1.0f);
                //boss.GetComponent<Button>().interactable = true;
            }
        else
        {
            Debug.Log(unit.unit_name + " Нет статусов");
            AI.MakeAction(unit, evil_units, good_units);
        }
        if (unit.current_hp > 0.0f)
        //if (unit.GetStunStatus() == null)
        //{
        //    Debug.Log(unit.unit_name + " ИГра говорит тчо юнит без стана, ход вычисляеца");
        //    AI.MakeAction(unit, evil_units, good_units);
        //    //unit.CheckCountStatus();
        //}
        //else
        //{
        //    Debug.Log(unit.unit_name + " ИГра говорит тчо юнит СО СТАНОМ, ход не вычисляеца, но рассчитывается дамаг по статусам");
        //    unit.StatusDamage();

        //}
            //unit.OnTurnStart();
            //return;
            //AI.MakeAction(unit, evil_units, good_units);
            //unit.CheckCountStatus();
            yield return new WaitForSeconds(1.0f);
        unit.OnTurnEnd();
    }
    RemoveDead();
    is_turn_end = true;
    yield return null;

    }

  void RemoveDead()
  {
    good_units.RemoveAll(unit => unit.current_hp <= 0.0f);
    evil_units.RemoveAll(unit => unit.current_hp <= 0.0f);
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