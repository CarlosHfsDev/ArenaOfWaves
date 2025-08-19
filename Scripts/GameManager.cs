using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Inimigos")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints; // pontos de spawn na cena
    public float spawnOffset = 8f; // usado se não tiver spawn points

    [Header("UI")]
    public Text waveText;
    public Text scoreText;
    private int wave = 1;
    private int score = 0;
    private int gerenemy;
    private float Interenemy;
     private EnemyController Enemy;

    void Start()
    {
        StartCoroutine(SpawnWaves());
        UpdateUI();
        gerenemy =3;
        Interenemy=0.5f;

         // Aqui usamos o player do topo
        GameObject EnemyObj = GameObject.Find("Enemy");
        if (EnemyObj != null)
        {
            Enemy = EnemyObj.GetComponent<EnemyController>();
        }
        else
        {
            Debug.LogError("Enemy não encontrado na cena!");
        }

    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            waveText.text = "Wave: " + wave;

            int enemyCount = wave * gerenemy; // cada wave gera mais inimigos
            for (int i = 0; i < enemyCount; i++)
            {
                Vector3 spawnPos = GetSpawnPosition();
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(0.5f); // intervalo entre inimigos
            }

            wave++;
            CheckSpecialWave(); // 🔥 verifica se é múltiplo de 5
            yield return new WaitForSeconds(3f); // pausa entre waves
        }
    }

    Vector3 GetSpawnPosition()
    {
        // Se tiver spawn points definidos no Inspector
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            return sp.position;
        }

        // Se NÃO tiver spawn points → cria nas bordas da tela
        Vector3 randomDir = Random.insideUnitCircle.normalized * spawnOffset;
        return randomDir;
    }

    void CheckSpecialWave()
    {
        if (wave % 5 == 0)        // múltiplos de 5 (5, 10, 15...)
        {
        
      Enemy.health *= 1.05f;     //aumenta vida do inimigoo
      Enemy.giveScore +=5 ;      //Aumenta 5 score a cada wave
      gerenemy+=2;               //gera mais dois inimigos em cada wave
      Interenemy -= 0.04f;       // deminui o intervalo de cada inimigo

        }
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (waveText != null) waveText.text = "Wave: " + wave;
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        StopAllCoroutines();
        waveText.text = "GAME OVER";
    }
}
