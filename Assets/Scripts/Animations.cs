using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    //Game game;
    Unit unit;

    public AnimationType current_animation;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimation(AnimationType _type)
    {

        //Debug.Log("PlayAnimation - " + _type);

        if (_type == AnimationType.IDLE)
        {
            PlayIdleAnimation();
            current_animation = AnimationType.IDLE;
        }
        if (_type == AnimationType.DEAD)
        {
            PlayDeadAnimation();
            current_animation = AnimationType.DEAD;
        }

        if (_type == AnimationType.STUN)
        {
            PlayStunAnimation();
            current_animation = AnimationType.STUN;
        }

    }

    string IsTypeUnit()
    {
        string type_unit = GetComponent<UnitUI>().unit.tag;
        return type_unit;

    }

    Animator GetAnimationComponent()
    {
        Animator anim = GetComponent<Animator>();
        return anim;

    }

    public void PlayIdleAnimation()
    {

        string type_unit = IsTypeUnit();
        Animator anim = GetAnimationComponent();

        if (type_unit == "hero")
        {
            anim.Play("hero_static");
        }
        else
        {
            anim.Play("evil_static");
        }

    }

    public void PlayDeadAnimation()
    {

        string type_unit = IsTypeUnit();
        Animator anim = GetAnimationComponent();

        if (type_unit == "hero")
        {
            anim.Play("hero_dead");
        }
        else
        {
            anim.Play("evil_dead");
        }

    }

    public void PlayStunAnimation()
    {

        string type_unit = IsTypeUnit();
        Animator anim = GetAnimationComponent();

        if (type_unit == "hero")
        {
            anim.Play("hero_stan");
        }
        else
        {
            anim.Play("evil_stan");
        }

    }

}

public enum AnimationType
{
    IDLE, ATTACK, DAMAGE, DEAD, STUN,
}
