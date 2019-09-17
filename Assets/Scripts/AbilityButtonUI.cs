using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
//RequireComponent(typeof(GameObject))]

public class AbilityButtonUI : MonoBehaviour
{
  [NonSerialized]
  public Ability ability;

  Button button;
  Image image;
  public GameObject colorAbility;

  public GameObject recharge_info;
  public Text recharge_text;
  //public Text recharge_text;

  void Start()
  {
    button = gameObject.GetComponent<Button>();
    image = gameObject.GetComponent<Image>();

        if (ability == null)
            return;
  }

    void SetColorButtom()
    {
        if (ability == null)
            return;
        if (ability.type == AbilityType.POISONING)
        {
            colorAbility.GetComponent<Image>().color = Color.green;
        }
        if (ability.type == AbilityType.PROTECTION)
        {
            colorAbility.GetComponent<Image>().color = Color.yellow;
        }
        if (ability.type == AbilityType.STUN)
        {
            colorAbility.GetComponent<Image>().color = Color.red;
        }
        if (ability.type == AbilityType.RECOVERY)
        {
            colorAbility.GetComponent<Image>().color = Color.blue;
        }

        if (ability.type == AbilityType.RECOVERY || ability.type == AbilityType.POISONING || ability.type == AbilityType.PROTECTION || ability.type == AbilityType.STUN)
        {
            colorAbility.SetActive(true);
        }
        else
        {
            colorAbility.SetActive(false);
        }

    }

  void Update()
  {
    if(ability == null)
      return;

    button.interactable = ability.is_ready;
    if(ability.prepare)
      image.color = Color.green;
    else
      image.color = Color.white;

    recharge_info.SetActive(ability.recharge_timer > 0);
    recharge_text.text = ability.recharge_timer.ToString();
        SetColorButtom();

  }
}
