using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability Data", order = 1)]
public class Ability : ScriptableObject
{
    new public string name;
    public string description;

    public float damage;
    public uint cooldown;

    public Sprite icon;

    public bool is_status;
    public AbilityType type;
    public uint duration;

    [NonSerialized]
    public uint recharge_timer;

    public bool is_ready {
    get {
        return recharge_timer == 0;
        }
    }

    public bool prepare {get; set;}

    public float Use()
    {
        prepare = false;
        recharge_timer = cooldown;
        return damage;
    }

    public void OnTurnStart()
    {
    if(recharge_timer > 0)
        recharge_timer--;
    }

}
