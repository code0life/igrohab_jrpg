
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class Unit : MonoBehaviour
{
    public string unit_name;
    public float max_hp;

    public List<Ability> abilities = new List<Ability>();
    public List<Ability> statuses = new List<Ability>();

    Game game;
    UnitUI unitUI;

    float _current_hp;
    public float current_hp {
    get {
        return _current_hp;
    }
    set {
        _current_hp = value;
        if(_current_hp < 0.0f)
        _current_hp = 0.0f;
        if(_current_hp > max_hp)
        _current_hp = max_hp;
        }
    }
    List<Ability> available_abilities = new List<Ability>();

    public void ApplyAbility(Ability ability)
    {
        Debug.Log("ApplyAbility - " + ability.name + " в юнита " + unit_name);
        DamageСalculation(ability);
        CheckStatusAbility(ability);
        ability.Use();
        //CheckUnitStatuses();
    }

    public void DamageСalculation(Ability ability)
    {
        //Debug.Log("DamageСalculation - " + ability.name + " в юнита " + unit_name);
        //if (IsUnitStatuses())
        //{
        //    Debug.Log("У юнита " + unit_name + " есть статусы");
        //    DamageUnit(ability);
        //}
        //else
        //{
        DamageUnit(ability);
        //    Debug.Log("У юнита " + unit_name + " нет статусов");

        //}
        //Debug.Log("DamageСalculation - " + ability.damage);
        //current_hp -= ability.damage;
    }

    void DamageUnit(Ability ability)
    {

        if (ability.type == AbilityType.PROTECTION)
        {
            Debug.Log("Накладывает щит - " + ability.name + " в юнита " + unit_name + "Дамаг не выщитывается");
        }
        else if (ability.type == AbilityType.STUN)
        {
            Debug.Log("Накладывает стан - " + ability.name + " в юнита " + unit_name + "Дамаг не выщитывается");
        }
        else if (ability.type == AbilityType.POISONING)
        {
            Debug.Log("Накладывает отравление - " + ability.name + " в юнита " + unit_name + "Дамаг не выщитывается");
        }
        else
        {
            if (IsUnitStatuses())
            {
                Debug.Log("У юнита " + unit_name + " есть статусы");

                Ability status_protection = GetProtectionStatus();
                //CheckActionTime(status_stun);
                if (status_protection != null)
                {
                    Debug.Log("У юнита " + unit_name + " есть Щит, режем урон");
                    if (ability.type == AbilityType.RECOVERY)
                    {
                        Debug.Log("ПРименяется хилка, не режем урон - " + ability.name + " в юнита " + unit_name);
                        current_hp -= ability.damage;
                    }
                    else
                    {
                        current_hp -= ability.damage / 2;
                    }
                }
                else
                {
                    current_hp -= ability.damage;
                }

            }
            else
            {
                current_hp -= ability.damage;
                Debug.Log("У юнита " + unit_name + " нет статусов");

            }

        }
    }

    public void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        unitUI = GameObject.Find("Unit").GetComponent<UnitUI>();

        //LoadAllAbilites();
        SetUnitAbilites();
        SetUnitHP();
    }

    void LoadAllAbilites()
    {
        //abilities = new List<Ability>(Resources.LoadAll<Ability>("Abilites"));
        //Debug.Log(abilities.Count);
    }

    Ability GetPoisonStatus()
    {
        for (int i = 0; i < statuses.Count; ++i)
        {
            Ability status_poison = statuses[i];
            //Debug.Log(status_poison.type);
            if (status_poison.type == AbilityType.POISONING)
            {
                return status_poison;
            }
        }

        return null;
    }

    Ability GetProtectionStatus()
    {
        for (int i = 0; i < statuses.Count; ++i)
        {
            Ability status_protection = statuses[i];
            //Debug.Log(status_poison.type);
            if (status_protection.type == AbilityType.PROTECTION)
            {
                return status_protection;
            }
        }

        return null;
    }

    public Ability GetStunStatus()
    {
        for (int i = 0; i < statuses.Count; ++i)
        {
            Ability status_stun = statuses[i];
            Debug.Log(status_stun.type);
            if (status_stun.type == AbilityType.STUN)
            {
                return status_stun;
            }
        }

        return null;
    }

    public bool IsUnitStatuses()
    {
        if (statuses.Count != 0)
        {
            return true;
        }
        return false;
    }

    void CheckUnitStatuses()
    {
        //if (_ability.is_status)
        //{
        //    AddUnitStatus(_ability);
        //}
    }

    void CheckStatusAbility(Ability _ability)
    {
        if (_ability.is_status)
        {
            AddUnitStatus(_ability);
        }
    }

    void AddUnitStatus(Ability _status)
    {
        Ability status = GameObject.Instantiate(_status);
        Ability find_status = null;

        foreach (Ability find in statuses)
        {
            if (find.name == _status.name)
            {
                find_status = find;
                break;
            }
                
        }

        if (find_status == null)
        {
            statuses.Add(status);
        }
        else
        {
            find_status.cooldown = _status.cooldown;
        }
        unitUI.UpdateUnitStatus();
        //unitUI.UpdateUnitStatus();
    }

    void SetUnitAbilites()
    {
        for (int i = 0; i < game.all_abilites.Count; ++i)
        {
            Ability ability = GameObject.Instantiate(game.all_abilites[i]);
            abilities.Add(ability);
        }
    }

    void SetUnitHP()
    {
        current_hp = max_hp;
    }

    Ability GetRandomAbility()
    {
        available_abilities.Clear();
        foreach(var ability in abilities)
        {
            if(ability.is_ready)
            available_abilities.Add(ability);
        }
        return available_abilities[UnityEngine.Random.Range(0, available_abilities.Count)];
    }

    public void StatusDamage()
    {
        Debug.Log("StatusDamage Пассивный урон юнита в начале хода " + unit_name + "");

        if (GetStunStatus() == null)
        {
            Debug.Log("StatusDamage Стана нет у " + unit_name);
        }
        else
        {
            Debug.Log("StatusDamage Снимаем стан у " + unit_name);
            Ability status_stun = GetStunStatus();
            CheckActionTime(status_stun);
        }
        //    Debug.Log("У юнита " + unit_name + " нет СТАНА. ОШИБКА");

        //    //current_hp -= ability.damage;
        //}
        //else
        //{
        //Ability status_stun = GetStunStatus();
        //CheckActionTime(status_stun);

        //Debug.Log("У юнита " + unit_name + " Подтверждаеца СТАН");
        if (GetPoisonStatus() != null)
            {
                Ability status_poison = GetPoisonStatus();

                Debug.Log("У юнита " + unit_name + " Подтверждаеца ОТРАВА");

                if (GetProtectionStatus() != null)
                {
                    Debug.Log("У юнита " + unit_name + " Подтверждаеца ЩИТ");
                Debug.Log("У current_hp " + current_hp);
                Debug.Log("У status_poison.damage " + status_poison.damage);
                Ability status_protection = GetProtectionStatus();
                    current_hp -= status_poison.damage / 2;
                //CheckActionTime(status_protection);
                Debug.Log("У current_hp " + current_hp);

            }
                else
                {
                    Debug.Log("У юнита " + unit_name + " Щита нет. ПОлный дамаг отравой.");
                Debug.Log("У current_hp " + current_hp);
                Debug.Log("У status_poison.damage " + status_poison.damage);
                current_hp -= status_poison.damage;
                Debug.Log("У current_hp " + current_hp);
            }

                CheckActionTime(status_poison);

            }

            if (GetProtectionStatus() != null)
            {
                Debug.Log("У юнита " + unit_name + " Подтверждаеца ЩИТ. Уменьшаем его на 1.");
                //Debug.Log("DamageСalculation - " + ability.damage / 2);
                Ability status_protection = GetProtectionStatus();
                CheckActionTime(status_protection);

            }
        //}
    }

    public void CheckActionTime(Ability _ability)
    {
        Debug.Log("CheckActionTime - " + _ability.name);
        if (_ability.duration > 0)
        {
            _ability.duration--;
            //Debug.Log(_ability.duration + " осталось тиков");
            if (_ability.duration <= 0)
            {
                statuses.Remove(_ability);
                //Destroy(_ability);


            }
        }
        else
        {
            statuses.Remove(_ability);
            //Destroy(_ability);
        }
    }

    public void OnTurnStart()
    {
        Debug.Log(this.unit_name + " ходит");
        foreach (var ability in abilities)
        {
            //DamageСalculation(ability);
            //Debug.Log(this.unit_name + " юзает абилку " + ability.name);
            ability.OnTurnStart();
        }
    }

}
