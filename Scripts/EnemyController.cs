using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movimento e Vida")]
    public float moveSpeed = 2f;
    public float health = 20f;

    [Header("Score")]
    public int giveScore = 10;

    [Header("Drop Kit")]
    public GameObject dropKitPrefab; // arraste o prefab do item aqui
    public float dropChance = 0.1f; // 10%

    private Transform player;
    private PlayerController playerct;
    private GameManager gameManager;

    void Start()
    {
        // Busca player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerct = playerObj.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("Player não encontrado na cena!");
        }

        // Busca GameManager
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
            Debug.LogError("GameManager não encontrado na cena!");
    }

    void Update()
    {
        // Movimento em direção ao player
        if (player != null)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            // Adiciona score
            if (gameManager != null)
                gameManager.AddScore(giveScore);

            // Incrementa kills do player
            if (playerct != null)
                playerct.AddKill();

            // Drop do kit
            DropKit();

            // Destrói inimigo
            Destroy(gameObject);
        }
    }

    void DropKit()
    {
        if (dropKitPrefab == null) return;

        float rand = Random.Range(0f, 1f); // sorteia entre 0 e 1
        if (rand <= dropChance)
        {
            Instantiate(dropKitPrefab, transform.position, Quaternion.identity);
            Debug.Log("DropKit spawnado!");
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerController pc = col.gameObject.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TakeDamage(10);
            }
        }
    }
}
