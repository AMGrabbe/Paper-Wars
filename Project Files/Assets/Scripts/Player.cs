using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //config parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] HealthBar healthBar;

    [Header("Projectile")]
    [SerializeField] float projectileFiringPeriod = 0.5f;
    int shootingDirection = 1;

    [Header("SoundFX")]
    [SerializeField] AudioClip hitSoundClip;
    [SerializeField] [Range(0,1)] float hitSoundVolume = 0.75f;

    Coroutine firingCoroutine;
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    void Start()
    {
        SetUpPlayerMovBound();
        healthBar.SetMaxHealth(health);
    }

    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {    
            firingCoroutine = StartCoroutine(FireConttinously());
        }
        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }

    }

    private IEnumerator FireConttinously()
    {
        while(true)
        {
            FireProjectile(shootingDirection);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
       
    }

    private void Move()
    {
        //move player character left or right (Horizonatal) and up and down (Vertical) on key input
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        //clamp transition values between viewport borders
        var newXPosition = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPosition = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPosition, newYPosition);
    }

    public override void ProcessDamage(DamageDealer damageDealer)
    {
        ProcessHit(damageDealer);
        healthBar.SetHealth(health);
    }

    public override void Die()
    { 
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        PlaySpecialEffectsOnDeath();
        
    }
    
    private void SetUpPlayerMovBound()
    {
        Camera gameCamera = Camera.main;
        //what is the world space value of the x axis corners
        xMin = gameCamera.ViewportToWorldPoint(new Vector3( 0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3( 1, 0, 0)).x - padding;
        // what is the world space value on y axis corners
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}