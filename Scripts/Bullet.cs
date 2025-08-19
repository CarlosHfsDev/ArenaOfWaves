using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    private Vector2 direction;
   
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
       this.destroybullet();
        transform.position += (Vector3)direction * speed * Time.deltaTime;
       
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void destroybullet(){
         if(!GetComponent<Renderer>().isVisible){
            Destroy(this.gameObject);
        }
    }

}
