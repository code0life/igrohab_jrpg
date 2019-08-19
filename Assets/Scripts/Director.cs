using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Director : MonoBehaviour
{
    #region Singleton

    public static Director instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    [Header("Boss")]
    public GameObject boss;

    [Header("Boss Text")]
    public Text text_current;
    public Text text_history1;
    public Text text_history2;

    Animator anim_panel;
    Animator anim_boss;

    // Start is called before the first frame update
    void Start()
    {
        anim_boss = boss.GetComponent<Animator>();
        anim_panel = GetComponent<Animator>();
        FirstStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FirstStart()
    {
        //anim_panel.Play("start");
        anim_boss.Play("idle");
        ShowStartMessage( "Start" );
    }

    public void ShowStartMessage(string _text)
    {
        text_current.GetComponent<Text>().text = "Hello!";
    }

    public void SaveOldMessage(string _text)
    {
        text_history2.GetComponent<Text>().text = text_history1.GetComponent<Text>().text;
        text_history1.GetComponent<Text>().text = _text;
    }

    public void ShowMessage(Ability _ability, Unit _unit1, Unit _unit2)
    {
        SaveOldMessage(text_current.GetComponent<Text>().text);

        if (_ability.name == "Kick" )
        {
            text_current.GetComponent<Text>().text = _unit1.unit_name + " does not hit the paws of " + _unit2.unit_name + " hard. It is not painful.";
        }
        else if (_ability.name == "Hard Kick")
        {
            text_current.GetComponent<Text>().text = _unit1.unit_name + " hits the paws of " + _unit2.unit_name + " hard. Now it hurts.";
        }
        else if (_ability.name == "Healing")
        {
            text_current.GetComponent<Text>().text = _unit1.unit_name + " restores the lives of " + _unit2.unit_name + ". This is Love.";
        }
        else if (_ability.name == "Poisoning")
        {
            text_current.GetComponent<Text>().text = _unit1.unit_name + " throws foul food at " + _unit2.unit_name + ". Hey. Do not throw food!";
        }
        else if (_ability.name == "Stun")
        {
            text_current.GetComponent<Text>().text = _unit1.unit_name + " pitches " + _unit2.unit_name + ". It seems I saw stars in his eyes.";
        }
        else if (_ability.name == "Protection")
        {
            text_current.GetComponent<Text>().text = _unit1.unit_name + " promises to protect " + _unit2.unit_name + ". Best friend!";
        }
        else  
        {
            text_current.GetComponent<Text>().text = "Sorry, I digress, so what's going on?";
        }

    }

    public void ClickMessage()
    {
        if (boss.GetComponent<Button>().interactable == true)
        {
            string save_text = text_current.GetComponent<Text>().text;
            StartCoroutine(TextCoroutine(save_text));
            boss.GetComponent<Button>().interactable = false;
            text_current.GetComponent<Text>().text = "Sorry, I digress, so what's going on?";
        }

    }

    IEnumerator TextCoroutine(string _text)
    {
        yield return new WaitForSeconds(1.0f);
        text_current.GetComponent<Text>().text = _text;
        boss.GetComponent<Button>().interactable = true;
    }
}
