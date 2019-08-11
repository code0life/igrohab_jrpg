using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class AbilityButtonUI : MonoBehaviour
{
  [NonSerialized]
  public Ability ability;

  Button button;
  Image image;

  public GameObject recharge_info;
  public Text recharge_text;

  void Start()
  {
    button = gameObject.GetComponent<Button>();
    image = gameObject.GetComponent<Image>();
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
  }
}
