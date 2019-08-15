
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

    Animation anim;
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
        Debug.Log("��������� ����� - " + ability.name + " � ����� " + unit_name);
        Damage�alculation(ability);
        CheckStatusAbility(ability);
        ability.Use();
        //CheckCountStatus();
        UpdateUnitStatus();
    }

    public void Damage�alculation(Ability ability)
    {
        //Debug.Log("Damage�alculation - " + ability.name + " � ����� " + unit_name);
        //if (IsUnitStatuses())
        //{
        //    Debug.Log("� ����� " + unit_name + " ���� �������");
        //    DamageUnit(ability);
        //}
        //else
        //{
        DamageUnit(ability);
        //    Debug.Log("� ����� " + unit_name + " ��� ��������");

        //}
        //Debug.Log("Damage�alculation - " + ability.damage);
        //current_hp -= ability.damage;
    }

    void DamageUnit(Ability ability)
    {

        if (ability.type == AbilityType.PROTECTION)
        {
            Debug.Log("����������� ��� - " + ability.name + " � ����� " + unit_name + "����� �� ������������");
        }
        else if (ability.type == AbilityType.STUN)
        {
            Debug.Log("����������� ���� - " + ability.name + " � ����� " + unit_name + "����� �� ������������");
        }
        else if (ability.type == AbilityType.POISONING)
        {
            Debug.Log("����������� ���������� - " + ability.name + " � ����� " + unit_name + "����� �� ������������");
        }
        else
        {
            if (IsUnitStatuses())
            {
                //Debug.Log("� ����� " + unit_name + " ���� �������");

                Ability status_protection = GetProtectionStatus();
                //CheckActionTime(status_stun);
                if (status_protection != null)
                {
                    //Debug.Log("� ����� " + unit_name + " ���� ���, ����� ����");
                    if (ability.type == AbilityType.RECOVERY)
                    {
                        //Debug.Log("����������� �����, �� ����� ���� - " + ability.name + " � ����� " + unit_name);
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
                //Debug.Log("� ����� " + unit_name + " ��� ��������");

            }

        }
    }

    public void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        anim = GetComponent<Animation>();
        //animation.Play("Evil_static");
        //unitUI = UnitUI.instance;

        //LoadAllAbilites();
        SetUnitAbilites();
        SetUnitHP();
        //UpdateUnitStatus();
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

        //Debug.Log( "������� � ���������� ������� � ������ " + unit_name );
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
                //Debug.Log("==========UpdateUnitStatus ���������" + unit_name);
                uUI.UpdateUnitStatus();
            }
            else
            {
                //Debug.Log("==========UpdateUnitStatus �������" + unit_name);
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

    public void CheckCountStatus()
    {
        //Debug.Log("!!!!!!!!!!!!!!!!CheckCountStatus " + unit_name);

        if (GetStunStatus() != null)
        {
            //Debug.Log("��������� ���� " + unit_name);
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
        Debug.Log("StatusDamage ��������� ���� ����� � ������ ���� " + unit_name + "");

        if (GetStunStatus() == null)
        {
            //Debug.Log("StatusDamage ����� ��� � " + unit_name);
        }
        else
        {
            //Debug.Log("StatusDamage ������� ���� � " + unit_name);
            //Ability status_stun = GetStunStatus();
            //CheckActionTime(status_stun);
        }
        //    Debug.Log("� ����� " + unit_name + " ��� �����. ������");

        //    //current_hp -= ability.damage;
        //}
        //else
        //{
        //Ability status_stun = GetStunStatus();
        //CheckActionTime(status_stun);

        //Debug.Log("� ����� " + unit_name + " ������������� ����");
        if (GetPoisonStatus() != null)
            {
                Ability status_poison = GetPoisonStatus();

                //Debug.Log("� ����� " + unit_name + " ������������� ������");

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
                //Debug.Log("Damage�alculation - " + ability.damage / 2);
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
        //��������� ��������� ���� � ������ ���� ������������� �������, � ��������� ������ �� ������
        Debug.Log(this.unit_name + " �����");
        foreach (var ability in abilities)
        {
            //Damage�alculation(ability);
            //Debug.Log(this.unit_name + " ����� ������ " + ability.name);
            ability.OnTurnStart();
        }
        //CheckCountStatus();
    }

    public void OnTurnEnd()
    {
        
        Debug.Log("----------------- ����� ���� - " + this.unit_name);
        //foreach (var ability in abilities)
        //{
        //    //Damage�alculation(ability);
        //    //Debug.Log(this.unit_name + " ����� ������ " + ability.name);
        //    ability.OnTurnStart();
        //}
        CheckCountStatus();
        UpdateAllUnitStatus();
        UpdateUnitStatus();
    }

}
