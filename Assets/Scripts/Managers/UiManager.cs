using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
	public static UiManager instance;
    public GameObject[] weaponsAndSpellsHud;

	public Image healthImage;
    public Image manaImage;

    public TextMeshProUGUI goldTxt;
    public TextMeshProUGUI ammoTxt;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI chestTxt;

    public GameObject damagetxt;
    public GameObject inventoryUi;

	void Awake()
	{
        if(instance == null)
		    instance = this;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayerHealthBar(float currentHealth, float maxHealth)
    {
        float calculateLife = currentHealth / maxHealth;
    	healthImage.fillAmount = Mathf.MoveTowards(healthImage.fillAmount, calculateLife, Time.deltaTime);
    }

    public void PlayerHealthDamage(float currentHealth, float damage)
    {
        currentHealth -= damage;
    }

    public void ManaUi(float manaCost, float maxMana)
    {
        manaImage.fillAmount -= manaCost / maxMana;
    }

    public void ManaFillup(float currentMana, float maxMana)
    {
        if(currentMana < maxMana)
        {
            manaImage.fillAmount = Mathf.MoveTowards(manaImage.fillAmount, 1, Time.deltaTime * 0.01f);
            currentMana = Mathf.MoveTowards(currentMana/maxMana, 1f, Time.deltaTime * 0.01f) * maxMana;
        }
    }

    public void GoldUiUpdate(float goldAmount)
    {
        goldTxt.text = "X " + goldAmount.ToString();
    }

    public void AmmoUiUpdate(int ammoAmount)
    {
        ammoTxt.text = "X " + ammoAmount.ToString();
    }

    public void LevelUiUpdate(int lvl)
    {
        levelTxt.text = lvl.ToString();
    }

    public void DamageText(Transform hit, int _ammount)
    {
        GameObject damage = Instantiate(damagetxt, hit.transform.position, Quaternion.identity);
        TextMeshPro tmp = damage.GetComponentInChildren<TextMeshPro>();
        tmp.SetText(_ammount.ToString());
    }

    public void WeaponsAndSpellsHud(int type, Sprite image)
    {
        weaponsAndSpellsHud[type].transform.GetChild(0).GetComponentInChildren<Image>().sprite = image;
        
    }

    public void WeaponsAndSpellsText(int type, string amount)
    {
        weaponsAndSpellsHud[type].GetComponentInChildren<TextMeshProUGUI>().text = amount;
    }

    public void ChestName(string _name)
    {
        chestTxt.text = _name;
    }
}
