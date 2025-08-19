using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 5f;
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Tiro")]
    public GameObject bulletPrefab;
    public float bulletSpawnDistance = 0.6f; // distância do player que o tiro nasce

    [Header("ULT")]
    public int kills = 0;
    public int killsToActivateULT = 50; // número de inimigos para liberar a ult
    public float ultRadius = 5f;
    public GameManager gameManager; // arrastar no Inspector

    private Camera cam;

    void Start()
    {
        currentHealth = maxHealth;
        cam = Camera.main;
    }

    void Update()
    {
        Move();
        Aim();

        if (Input.GetMouseButtonDown(0)) // clique esquerdo para atirar
        {
            Shoot();
        }

        // Ativar ULT
        if (Input.GetKeyDown(KeyCode.X) && kills >= killsToActivateULT)
        {
            ULT();
            kills = 0; // reset kills após usar
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, v, 0).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    void Aim()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); // -90 para alinhar sprite
    }

    void Shoot()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = (mousePos - transform.position).normalized;

        Vector3 spawnPos = transform.position + (Vector3)shootDir * bulletSpawnDistance;

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(shootDir);
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            if (gameManager != null)
                gameManager.GameOver();
            Destroy(gameObject);
        }
    }

    // Função para incrementar kills (chamar quando o player matar um inimigo)
    public void AddKill()
    {
        kills++;
    }

    // Função da ULT
    void ULT()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= ultRadius)
            {
                if (gameManager != null)
                    gameManager.AddScore(enemy.giveScore);

                Destroy(enemy.gameObject);
            }
        }
        Debug.Log("ULT ativada! Todos os inimigos próximos mortos.");
    }


     void OnCollisionEnter2D(Collision2D col)
{
    if (col.gameObject.CompareTag("BoxLife"))
    {
        if(currentHealth <=50 ){
             currentHealth += 40; // aumenta a vida

        }else{
        
        currentHealth = 100;
        }

               Destroy(col.gameObject); // destrói a caixa
    }
}


}
