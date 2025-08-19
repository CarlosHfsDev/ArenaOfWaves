using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Variaveis")]
    public Text VidaPlayerText;   // UI de Vida
    public Text UltText;
    public Text UltTextActive;
    private PlayerController player; // Referência ao Player
     public float ultPercent;

    void Start()
    {
        // Aqui usamos o player do topo
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("Player não encontrado na cena!");
        }
    }

    void ultUI()
    {
        // Calcula a porcentagem do carregamento da ult
        ultPercent = (float)player.kills / player.killsToActivateULT * 100f;
        ultPercent = Mathf.Clamp(ultPercent, 0f, 100f); // garante que não passe de 100%
        UltText.text = "ULT: " + ultPercent.ToString("F0") + "%";
        Debug.Log("Sua Ult esta em : " +ultPercent.ToString("F0"));
    
        if(ultPercent >= 100f){

       UltTextActive.gameObject.SetActive(true);

        }else{
             UltTextActive.gameObject.SetActive(false);
        }
    

        // Atualiza o texto
       

    }

    void Update()
    {

    
         ultUI();
        if (player != null)
        {
            VidaPlayerText.text = "Life: "+ player.currentHealth.ToString();
             UltText.text = "ULT: " + ultPercent.ToString("F0") + "%";
        }else{
            Debug.LogError("Player não encontrado na cena!");
        }
    }
}
