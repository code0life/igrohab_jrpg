
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

    Animations anim;
    Game game;
    //UnitUI unitUI;

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
        Debug.Log("Применяем скилл - " + ability.name + " в юнита " + unit_name);
        DamageСalculation(ability);
        CheckStatusAbility(ability);
        ability.Use();
        //CheckCountStatus();
        UpdateUnitStatus();
        CheckHealthUnit();

    }

    public void CheckHealthUnit()
    {
        UnitUI uUI = game.GetUnitUI(this);

        if (_current_hp <= 0.0f)
        {
            Debug.Log( "юнит " + unit_name + " МЕРТВ !");
            uUI.GetComponent<Animations>().PlayAnimation(AnimationType.DEAD);
        }
        else
        {
            Debug.Log("юнит " + unit_name + " ЖИВ !");
            if (GetStunStatus() == null)
            {
                uUI.GetComponent<Animations>().PlayAnimation(AnimationType.IDLE);
            }


        }
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        UnitUI uUI = game.GetUnitUI(this);
        if (uUI == null)
        {
            return;
        }
        uUI.health_bar.fillAmount = _current_hp / max_hp;
    }

    public void DamageСalculation(Ability ability)
    {
        DamageUnit(ability);
    }

    void DamageUnit(Ability ability)
    {
        UnitUI uUI = game.GetUnitUI(this);

        if (ability.type == AbilityType.PROTECTION)
        {
            Debug.Log("Накладывает щит - " + ability.name + " в юнита " + unit_name + "Дамаг не выщитывается");
        }
        else if (ability.type == AbilityType.STUN)
        {
            Debug.Log("Накладывает стан - " + ability.name + " в юнита " + unit_name + "Дамаг не выщитывается");
            uUI.GetComponent<Animations>().PlayAnimation(AnimationType.STUN);
        }
        else if (ability.type == AbilityType.POISONING)
        {
            Debug.Log("Накладывает отравление - " + ability.name + " в юнита " + unit_name + "Дамаг не выщитывается");
        }
        else
        {

            if (IsUnitStatuses())
            {
                //Debug.Log("У юнита " + unit_name + " есть статусы");

                Ability status_protection = GetProtectionStatus();
                //CheckActionTime(status_stun);
                if (status_protection != null)
                {
                    //Debug.Log("У юнита " + unit_name + " есть Щит, режем урон");
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
                //Debug.Log("У юнита " + unit_name + " нет статусов");

            }

        }
        //UpdateHealthBar();
    }

    public void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();

        //UnitUI uUI = game.GetUnitUI(this);
        //if (uUI != null)
        //{
        //    anim.PlayAnimation(uUI, AnimationType.IDLE);

        //}
        //anim = GetComponent<Animation>();
        //animation.Play("Evil_static");
        //unitUI = UnitUI.instance;

        //LoadAllAbilites();
        //SetUnitAbilites();
        SetAllUnitAbilites();
        SetUnitHP();
        //UpdateUnitStatus();
    }

    void LoadAllAbilites()
    {
        //abilities = new List<Ability>(Resources.LoadAll<Ability>("Abilites"));
        //Debug.Log(abilities.Count);
    }

    void SetAllUnitAbilites()
    {
        for (int i = 0; i<game.all_abilites.Count; ++i)
        {
            Ability ability = GameObject.Instantiate(game.all_abilites[i]);
            abilities.Add(ability);
        }
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
            //Debug.Log(status_stun.type);
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
        Debug.Log("AddUnitStatus - " + _status.name);
        int index_status = 0;
        bool find_index_status = false;

        int i = 0;
        foreach (Ability find in statuses)
        {
            if (find.name == _status.name)
            {
                index_status = i;
                find_index_status = true;
                break;
            }
            i++;
        }

        if (!find_index_status)
        {
            Debug.Log("AddUnitStatus NEW - " + _status.name);
            Ability status = GameObject.Instantiate(_status);
            statuses.Add(status);
        }
        else
        {
            Debug.Log("AddUnitStatus ADD - " + _status.name);
            //Debug.Log("index_status ADD - " + index_status);
            //Debug.Log("statuses[index_status].cooldown - " + statuses[index_status].cooldown);
            //Debug.Log("_status.cooldown - " + _status.cooldown);
            statuses[index_status].duration = _status.duration;
            //statuses[index_status].cooldown = _status.cooldown;
        }

        //Debug.Log( "Перейдём в обновлении статуса у игрока " + unit_name );
        //UpdateUnitStatus();
    }

    void UpdateAllUnitStatus()
    {
        game.DoneAllStatusNew();
    }

    void UpdateUnitStatus()
    {
        //if (IsUnitStatuses())
        //{
            //Debug.Log("==========UpdateUnitStatus " + unit_name);
            UnitUI uUI = game.GetUnitUI(this);
            if (uUI != null)
            {
                //Debug.Log("==========UpdateUnitStatus РЕЗУЛЬТАТ" + unit_name);
                uUI.UpdateUnitStatus();
            }
            else
            {
                //Debug.Log("==========UpdateUnitStatus ПУСТОТА" + unit_name);
                uUI.UpdateUnitStatus();
            }

        //}
        
        //for (int i = 0; i < game.all_abilites.Count; ++i)
        //{
        //    Ability ability = GameObject.Instantiate(game.all_abilites[i]);
        //    abilities.Add(ability);
        //}
    }

    void SetUnitAbilites()
    {
        Ability kick_ability = GetUnitKickAbility();

        if (kick_ability == null)
        {
            return;
        }
        else
        {
            abilities.Add(kick_ability);
        }


        SetTwoUnitAbilites();

    }

    void SetTwoUnitAbilites()
    {
        Ability random_ability_one = GetRndAbility();

        if (random_ability_one.name != "Kick")
        {
            Debug.Log( name + " скил 2 найден" );
            //abilities.Add(random_ability_one);
        }
        else
        {
            Debug.Log(name + " второй ошибка .рестарт");
            //abilities.Clear();
            SetTwoUnitAbilites();
            return;
        }

        Ability random_ability_two = GetRndAbility();

        if (random_ability_two != null && random_ability_two != random_ability_one && random_ability_two.name != "Kick")
        {
            Debug.Log(name + " скилл 3 найден");
            Debug.Log(name + " ЗАПИСЫВАЕМ 2 СКИЛЛА");
            abilities.Add(random_ability_one);
            abilities.Add(random_ability_two);
        }
        else
        {
            Debug.Log(name + " третий ошибка .рестарт");
            abilities.Clear();
            SetUnitAbilites();
        }

    }

    Ability GetRndAbility()
    {
        Ability rnd_ability = game.all_abilites[UnityEngine.Random.Range(0, game.all_abilites.Count)];
        return rnd_ability;
    }

    Ability GetUnitKickAbility()
    {
        for (int i = 0; i < game.all_abilites.Count; ++i)
        {
            if (game.all_abilites[i].name == "Kick")
            {
                Ability ability = GameObject.Instantiate(game.all_abilites[i]);
                return ability;
            }

        }

        return null;

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

    public void CheckCountStatus()
    {
        //Debug.Log("!!!!!!!!!!!!!!!!CheckCountStatus " + unit_name);

        if (GetStunStatus() != null)
        {
            //Debug.Log("Выполняем стан " + unit_name);
            Ability status1 = GetStunStatus();
            //if (status1.is_new)
            //{
                //status1.is_new = false;
                
            //}
           // else
            //{
                CheckActionTime(status1);
            //}
            
        }

        if (GetProtectionStatus() != null)
        {
            //Debug.Log("GetProtectionStatus " + unit_name);
            Ability status2 = GetProtectionStatus();
            if (status2.is_new)
            {
                status2.is_new = false;
            }
            else
            {
                CheckActionTime(status2);
            }
        }

        if (GetPoisonStatus() != null)
        {
            //Debug.Log("GetPoisonStatus " + unit_name);
            Ability status3 = GetPoisonStatus();
            if (status3.is_new)
            {
                status3.is_new = false;
            }
            else
            {
                CheckActionTime(status3);
            }
        }
    }

    public void StatusDamage()
    {
        Debug.Log("StatusDamage Пассивный урон юнита в начале хода " + unit_name + "");

        if (GetStunStatus() == null)
        {
            //Debug.Log("StatusDamage Стана нет у " + unit_name);
        }
        else
        {
            //Debug.Log("StatusDamage Снимаем стан у " + unit_name);
            //Ability status_stun = GetStunStatus();
            //CheckActionTime(status_stun);
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

                //Debug.Log("У юнита " + unit_name + " Подтверждаеца ОТРАВА");

                if (GetProtectionStatus() != null)
                {
                Ability status_protection = GetProtectionStatus();
                current_hp -= status_poison.damage / 2;


            }
                else
                {
                current_hp -= status_poison.damage;

            }

                //CheckActionTime(status_poison);

            }

            if (GetProtectionStatus() != null)
            {
                //Debug.Log("DamageСalculation - " + ability.damage / 2);
                Ability status_protection = GetProtectionStatus();
                //CheckActionTime(status_protection);

            }
        //}
        //if (IsUnitStatuses())
        //{
            //CheckCountStatus();
            //UpdateUnitStatus();
        //}

    }

    public void CheckActionTime(Ability _ability)
    {
        //Debug.Log("!!!!!!!!!!!!!!!!!!!!! CheckActionTime - " + _ability.name);
        //if (IsUnitStatuses())
        //{
        //    CheckCountStatus();
        //    UpdateUnitStatus();
        //}
        if (_ability.duration > 0)
        {
            _ability.duration--;
            //CheckCountStatus();
            //UpdateUnitStatus();
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
        //UpdateUnitStatus();
    }

    public void OnTurnStart()
    {
        //ПРОВЕРИТЬ ПАССИВНЫЙ УРОН В НАЧАЛЕ ХОДА ДРУЖЕСТВЕННОЙ ЕДЕНИЦЫ, в частности дженна от отравы
        Debug.Log(this.unit_name + " ходит");
        foreach (var ability in abilities)
        {
            //DamageСalculation(ability);
            //Debug.Log(this.unit_name + " юзает абилку " + ability.name);
            ability.OnTurnStart();
        }
        //CheckCountStatus();
    }

    public void OnTurnEnd()
    {
        UnitUI uUI = game.GetUnitUI(this);
        Debug.Log("----------------- Конец хода - " + this.unit_name);
        //foreach (var ability in abilities)
        //{
        //    //DamageСalculation(ability);
        //    //Debug.Log(this.unit_name + " юзает абилку " + ability.name);
        //    ability.OnTurnStart();
        //}
        CheckCountStatus();
        UpdateAllUnitStatus();
        UpdateUnitStatus();
        CheckHealthUnit();
    }

}
