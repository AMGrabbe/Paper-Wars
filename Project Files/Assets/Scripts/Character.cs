using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header ("Status")]
    public int health = 100;

    [Header ("Effects")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip deathSoundClip;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.75f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0,1)] float shootSoundVolume = 0.25f;

    [Header("Shooting")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float bulletStartingPosion = 1.5f;

    public void PlaySpecialEffectsOnDeath()
    {
        GameObject explosion = Instantiate(deathVFX,
                                   new Vector2(transform.position.x, 
                                               transform.position.y), 
                                   Quaternion.identity) as GameObject;
        Destroy(explosion, durationOfExplosion);

        AudioSource.PlayClipAtPoint(deathSoundClip, 
                                    Camera.main.transform.position, 
                                    deathSoundVolume);
    }

    public void FireProjectile( int direction)
    {
        //Quaternion.identity for no rotation
        GameObject bullet = Instantiate(
                            bulletPrefab, 
                            new Vector2(transform.position.x, transform.position.y + bulletStartingPosion), 
                            Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(
                                                      0, 
                                                      projectileSpeed * direction);

        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessDamage(damageDealer);
    }
    
    public virtual void ProcessDamage(DamageDealer damageDealer)
    {
        ProcessHit(damageDealer);
    }

    protected void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        if(health < 0)
        {
            Die();            
        } 
        else
        {
            StartCoroutine(Disapear()); 
        }
    }


    private IEnumerator Disapear()
    {
        var sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color (1, 1, 1, 0);
        yield return new WaitForSeconds(0.05f);
        sprite.color = new Color (1, 1, 1, 1);
    }

    public int GetHealth() { return health;}
    public virtual void Die() {}
}
