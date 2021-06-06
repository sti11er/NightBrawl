using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Image life_scale;
    public GameObject inscription_game_over;
    private bool no_game_over = true;
    public GameObject game_controler;

    //сериализация данных между пользователями

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("bullet1")){
            Destroy(col.gameObject);
            life_scale.fillAmount -= 0.2f;
        }
    }
    void Update()
    {
        if (life_scale.fillAmount < 0.1 && no_game_over){
            Instantiate(inscription_game_over, new Vector3(0, 0, 0), transform.rotation);
            Time.timeScale = 0;
            Destroy(game_controler.gameObject);
            no_game_over = false;
        }
    }
}
