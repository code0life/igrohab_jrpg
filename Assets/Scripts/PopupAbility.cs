using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupAbility : MonoBehaviour
{
    //#region Singleton

    //public static PopupAbility instance;
    //void Awake()
    //{
    //    instance = this;
    //}

    //#endregion

    public GameObject popPanel;
    public GameObject aButton;

    Ability popAbility;

    public GameObject aName;
    public GameObject Damage;
    public GameObject aDamage;
    public GameObject aCooldown;
    public GameObject aType;
    public GameObject Duration;
    public GameObject aDuration;

    public GameObject aDescription;

    // Start is called before the first frame update
    void Start()
    {
        //popAbility = aButton.GetComponent<AbilityButtonUI>().ability;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowPop()
    {
        SetPop();
        PopUp();

    }

    public void HidePop()
    {
        PopUp();
    }

    public void PopUp()
    {
        popPanel.SetActive(!popPanel.activeSelf);
    }


    public void SetPop()
    {
        popAbility = aButton.GetComponent<AbilityButtonUI>().ability;

        if (popAbility == null)
            return;

        Vector3 sizeButton = new Vector3(aButton.GetComponent<RectTransform>().rect.width, aButton.GetComponent<RectTransform>().rect.height, 0);
        float offset_x = aButton.transform.position.x;
        float offset_y = aButton.transform.position.y + sizeButton.y*2 - sizeButton.y / 2;

        Vector3 newPos = new Vector3(offset_x, offset_y, 0);
        popPanel.transform.position = newPos;

        aName.GetComponent<Text>().text = popAbility.name;
        aDescription.GetComponent<Text>().text = popAbility.description;
        aDamage.GetComponent<Text>().text = popAbility.damage.ToString();
        Damage.GetComponent<Text>().text = "Damage:";
        aCooldown.GetComponent<Text>().text = popAbility.cooldown.ToString();
        aType.GetComponent<Text>().text = popAbility.type.ToString(); ;
        aDuration.GetComponent<Text>().text = popAbility.duration.ToString();
        Duration.SetActive(true);

        if (popAbility.type == AbilityType.POISONING)
        {
            aType.GetComponent<Text>().color = Color.green / 2;
        }
        if (popAbility.type == AbilityType.PROTECTION)
        {
            aType.GetComponent<Text>().color = Color.yellow / 2;
            aDamage.GetComponent<Text>().text = "0";
        }
        if (popAbility.type == AbilityType.STUN)
        {
            aType.GetComponent<Text>().color = Color.red / 2;
        }
        if (popAbility.type == AbilityType.RECOVERY)
        {
            aType.GetComponent<Text>().color = Color.blue / 2;
            Damage.GetComponent<Text>().text = "Restores:";
            aDamage.GetComponent<Text>().text = "20";
        }
        if (popAbility.type == AbilityType.ATTACK)
        {
            aType.GetComponent<Text>().color = Color.black;
            Duration.SetActive(false);
        }

    }
}
