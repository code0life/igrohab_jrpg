using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
  public AbilityUI ability_ui;
  public List<Unit> good_units = new List<Unit>();
  public List<Unit> evil_units = new List<Unit>();

  public List<Ability> all_abilites = new List<Ability>();

  public GameObject evil_units_ui;
  public GameObject good_units_ui;

  public GameObject mini_status_panel;
  public GameObject mini_status;

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
        InitUnitsUI(evil_units, evil_units_ui);
        InitUnitsUI(good_units, good_units_ui);
        
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
                unit_ui = units_ui.transform.GetChild(i).gameObject;
            else
                unit_ui = GameObject.Instantiate(units_ui.transform.GetChild(0).gameObject, units_ui.transform);
            unit_ui.GetComponent<UnitUI>().unit = units[i];
            var unit = units[i];

            int j;
            for (j = 0; j < units[i].statuses.Count; ++j) 
            {

            }
            unit_ui.GetComponent<Button>().onClick.AddListener(delegate () {
                ability_ui.target = unit;
            });
        }

        for (; i < units_ui.transform.childCount; ++i)
            units_ui.transform.GetChild(i).gameObject.SetActive(false);
    }

    //int j;
    //for (j = 0; j < units[i].statuses.Count; ++j)
    //{
    //    GameObject unit_status_ui;
    //    if (i < mini_status_panel.transform.childCount)
    //    {
    //        unit_status_ui = mini_status_panel.transform.GetChild(j).gameObject;
    //        Debug.Log("В " + units[i].unit_name + " создаём присваиваем ");
    //    }
    //    else
    //    {
    //        Debug.Log("В " + units[i].unit_name + " создаём статус ");
    //        //unit_status_ui
    //        //Debug.Log("В " + units[i].unit_name + " создаём статус " + unit_status_ui.name);
    //        //unit_status_ui = GameObject.Instantiate(unit_ui.GetComponent<UnitUI>().unit.statuses[0].transform.GetChild(0).gameObject, unit_ui.GetComponent<UnitUI>().unit.transform);
    //        //unit_ui.GetComponent<UnitUI>().unit = units[i];
    //        //unit_ui.GetComponent<UnitUI>().unit.statuses[j] = 
    //    }

    //}

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
    foreach(var unit in good_units)
    {
        if (unit.GetStunStatus() != null)
        {
            Debug.Log(unit.unit_name + " ИГра говорит что гирок под станом, идёт подсчет статусов и пропуск хода");
            unit.OnTurnStart();
            if (unit.IsUnitStatuses())
            {
                Debug.Log(unit.unit_name + " Есть статусы у игрока, анализ пассивного урона");
                unit.StatusDamage();
                yield return new WaitForSeconds(1.0f);
            }
        }
        else
        {
                //Debug.Log(unit.unit_name + " ИГра говорит тчо юнит СО СТАНОМ, ход не вычисляеца, но рассчитывается дамаг по статусам");
                unit.OnTurnStart();
                ability_ui.gameObject.SetActive(true);
                if (unit.IsUnitStatuses())
                {
                    Debug.Log(unit.unit_name + " Есть статусы у игрока, анализ пассивного урона");
                    unit.StatusDamage();
                    yield return new WaitForSeconds(1.0f);
                }
                yield return ability_ui.WaitInput(unit, good_units, evil_units, OnAbilitySelected);
                ability_ui.gameObject.SetActive(false);
            }

      yield return new WaitForSeconds(1.0f);

    }
    RemoveDead();
    foreach(var unit in evil_units)
    {

        unit.OnTurnStart();
        if (unit.IsUnitStatuses())
        {
            Debug.Log(unit.unit_name + " Есть статусы, анализ пассивного урона");
            unit.StatusDamage();
            yield return new WaitForSeconds(1.0f);
        }
      
        if (unit.current_hp > 0.0f)
        if (unit.GetStunStatus() == null)
        {
            Debug.Log(unit.unit_name + " ИГра говорит тчо юнит без стана, ход вычисляеца");
            AI.MakeAction(unit, evil_units, good_units);

        }
        else
        {
            Debug.Log(unit.unit_name + " ИГра говорит тчо юнит СО СТАНОМ, ход не вычисляеца, но рассчитывается дамаг по статусам");
            unit.StatusDamage();

        }
            //unit.OnTurnStart();
            //return;
            //AI.MakeAction(unit, evil_units, good_units);

            yield return new WaitForSeconds(1.0f);
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