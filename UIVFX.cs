using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIVFX : MonoBehaviour
{
    public GameObject Text;
    public List<GameObject> DamageText = new List<GameObject>();
    public GameObject PlayerHealthLostVFX;
    public GameObject EnemyHealthLostVFX;
    public GameObject PlayerHealthBar;
    public GameObject EnemyHealthBar;
    private Animator animPlayer;
    private Animator animEnemy;

    void Start()
    {
        animPlayer = PlayerHealthBar.GetComponent<Animator>();
        animEnemy = EnemyHealthBar.GetComponent<Animator>();
    }

    public void SpawnDamageText(int damage, bool IsPlayer)
    {
        if (IsPlayer)
        {
            DamageText.Add(Instantiate(Text, PlayerHealthBar.transform));
            DamageText[DamageText.Count - 1].GetComponent<TextMeshProUGUI>().text = "" + damage;
            DamageText[DamageText.Count - 1].GetComponent<RectTransform>().Translate(new Vector3(Random.Range(-75, 75), Random.Range(-75, 75), Random.Range(-75, 75)));
            animPlayer.Play("Base Layer.HealthBarDamage_Player", 0, 0);
            for (int i = 0; i < PlayerHealthLostVFX.gameObject.transform.childCount; i++)
            {
                PlayerHealthLostVFX.transform.GetChild(i).gameObject.SetActive(false);
                PlayerHealthLostVFX.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            DamageText.Add(Instantiate(Text, EnemyHealthBar.transform.position, new Quaternion(), EnemyHealthBar.transform));
            DamageText[DamageText.Count - 1].transform.Rotate(new Vector3(0, -180, 0));
            DamageText[DamageText.Count - 1].GetComponent<TextMeshProUGUI>().text = "" + damage;
            DamageText[DamageText.Count - 1].GetComponent<RectTransform>().Translate(new Vector3(Random.Range(-75, 75), Random.Range(-75, 75), Random.Range(-75, 75)));
            animEnemy.Play("Base Layer.HealthBarDamage_Enemy", 0, 0);
            for (int i = 0; i < EnemyHealthLostVFX.gameObject.transform.childCount; i++)
            {
                EnemyHealthLostVFX.transform.GetChild(i).gameObject.SetActive(false);
                EnemyHealthLostVFX.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void ClearDamageTextList()
    {
        DamageText.Clear();
    }
}