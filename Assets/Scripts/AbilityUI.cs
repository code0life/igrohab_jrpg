using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;

public class AbilityUI : MonoBehaviour
{
  [NonSerialized]
  public int index = -1;

  Ability selected;

  [NonSerialized]
  public Unit target;

  void SetupButton(int i, Ability ability, GameObject button)
  {
    button.GetComponentInChildren<Text>().text = ability.name;

    var ability_button_ui = button.GetComponent<AbilityButtonUI>();
    ability_button_ui.ability = ability;

    var button_component = button.GetComponent<Button>();
    button_component.onClick.RemoveAllListeners();
    button_component.onClick.AddListener(() => {
      if(selected != ability)
      {
        if(selected != null)
          selected.prepare = false;
        selected = ability;
        selected.prepare = true;
      }
      else
      {
        selected.prepare = false;
        selected = null;
      }
    });
  }

  public IEnumerator WaitInput(Unit unit, List<Unit> ally, List<Unit> enemy, Action<Ability> on_selected)
  {
    selected = null;

    if(unit.abilities.Count == 0)
      yield break;

    if(ally.Count == 0 || enemy.Count == 0)
      yield break;

    Show(unit.abilities);

    while(selected == null || target == null)
    {
      on_selected(null);

      while(selected == null)
        yield return null;

      on_selected(selected);

      target = null;
      while(target == null
            || (selected.damage < 0.0f && !ally.Contains(target))
            //|| (selected.damage == 0.0f && ally.Contains(target))
            || (selected.damage > 0.0f && !enemy.Contains(target)))
      {
        if(selected == null)
          break;
        yield return null;
      }
    }

    target.ApplyAbility(selected, unit);
    on_selected(null);
  }

  public void Show(List<Ability> abilities)
  {
    int i;
    for(i = 0; i < abilities.Count; ++i)
    {
      if(i < transform.childCount)
        SetupButton(i, abilities[i], transform.GetChild(i).gameObject);
      else
        SetupButton(i, abilities[i], GameObject.Instantiate(transform.GetChild(0).gameObject, transform));
    }

    for(;i < transform.childCount; ++i)
      transform.GetChild(i).gameObject.SetActive(false);
  }

    public void SetStatusUnitUI(List<Ability> abilities)
    {
        //int i;
        //for (i = 0; i < abilities.Count; ++i)
        //{
        //    if (i < transform.childCount)
        //        SetupButton(i, abilities[i], transform.GetChild(i).gameObject);
        //    else
        //        SetupButton(i, abilities[i], GameObject.Instantiate(transform.GetChild(0).gameObject, transform));
        //}

        //for (; i < transform.childCount; ++i)
        //    transform.GetChild(i).gameObject.SetActive(false);
    }

}
