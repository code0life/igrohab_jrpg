
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class Unit : MonoBehaviour
{
    [Header("Unit")]
    public string unit_name;
    public float max_hp;

    [Header("Abilities")]
    public List<Ability> abilities = new List<Ability>();

    [Header("Statuses")]
    public List<Ability> statuses = new List<Ability>();

    Animations anim;
    Game game;
    Director boss;

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

    public void ApplyAbility(Ability ability, Unit _unit)
    {
        Damage—alculation(ability);
        CheckStatusAbility(ability);
        ability.Use();
        UpdateUnitStatus();
        CheckHealthUnit();
        boss.ShowMessage(ability, _unit, this );
    }

    public void CheckHealthUnit()
    {
        UnitUI uUI = game.GetUnitUI(this);

        if (_current_hp <= 0.0f)
        {
            uUI.GetComponent<Animations>().PlayAnimation(AnimationType.DEAD);
        }
        else
        {
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

    public void Damage—alculation(Ability ability)
    {
        DamageUnit(ability);
    }

    void DamageUnit(Ability ability)
    {
        UnitUI uUI = game.GetUnitUI(this);

        if (ability.type == AbilityType.PROTECTION)
        {
        }
        else if (ability.type == AbilityType.STUN)
        {
            uUI.GetComponent<Animations>().PlayAnimation(AnimationType.STUN);
        }
        else if (ability.type == AbilityType.POISONING)
        {
        }
        else
        {

            if (IsUnitStatuses())
            {
                Ability status_protection = GetProtectionStatus();

                if (status_protection != null)
                {
                    if (ability.type == AbilityType.RECOVERY)
                    {
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
            }

        }
    }

    public void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        boss = GameObject.Find("Director").GetComponent<Director>();

        //SetAllUnitAbilites();
        SetUnitAbilites();
        SetUnitHP();
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

    void CheckStatusAbility(Ability _ability)
    {
        if (_ability.is_status)
        {
            AddUnitStatus(_ability);
        }
    }

    void AddUnitStatus(Ability _status)
    {
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
            Ability status = GameObject.Instantiate(_status);
            statuses.Add(status);
        }
        else
        {
            statuses[index_status].duration = _status.duration;
        }
    }

    void UpdateAllUnitStatus()
    {
        game.DoneAllStatusNew();
    }

    void UpdateUnitStatus()
    {
        UnitUI uUI = game.GetUnitUI(this);

        if (uUI != null)
        {
            uUI.UpdateUnitStatus();
        }
        else
        {
            uUI.UpdateUnitStatus();
        }
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
        }
        else
        {
            SetTwoUnitAbilites();
            return;
        }

        Ability random_ability_two = GetRndAbility();

        if (random_ability_two != null && random_ability_two != random_ability_one && random_ability_two.name != "Kick")
        {
            abilities.Add(random_ability_one);
            abilities.Add(random_ability_two);
        }
        else
        {
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

        if (GetStunStatus() != null)
        {
            Ability status1 = GetStunStatus();
            CheckActionTime(status1);
            
        }

        if (GetProtectionStatus() != null)
        {
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
        if (GetPoisonStatus() != null)
        {
            Ability status_poison = GetPoisonStatus();

            if (GetProtectionStatus() != null)
            {

                Ability status_protection = GetProtectionStatus();
                current_hp -= status_poison.damage / 2;

            }
            else
            {
                current_hp -= status_poison.damage;
            }

        }

        if (GetProtectionStatus() != null)
        {
            Ability status_protection = GetProtectionStatus();
        }

    }

    public void CheckActionTime(Ability _ability)
    {

        if (_ability.duration > 0)
        {
            _ability.duration--;
            if (_ability.duration <= 0)
            {
                statuses.Remove(_ability);

            }
        }
        else
        {
            statuses.Remove(_ability);
        }
    }

    public void OnTurnStart()
    {
        foreach (var ability in abilities)
        {
            ability.OnTurnStart();
        }
    }

    public void OnTurnEnd()
    {
        UnitUI uUI = game.GetUnitUI(this);

        CheckCountStatus();
        UpdateAllUnitStatus();
        UpdateUnitStatus();
        CheckHealthUnit();
    }

}
