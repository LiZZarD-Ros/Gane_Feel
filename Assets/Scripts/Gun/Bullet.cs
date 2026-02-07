using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _bulletVFX;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockBackThrust = 20f;

    private Vector2 _fireDirection;

    private Rigidbody2D _rigidBody;

    private Gun _gun;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Init(Gun gun, Vector2 bulletSpawnPos, Vector2 mousePos)
    {
        _gun = gun;
        transform.position = bulletSpawnPos;
        _fireDirection = (mousePos - bulletSpawnPos).normalized; 
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    
    {
        IHitable iHitable = other.gameObject.GetComponent<IHitable>();
        iHitable?.TakeHit();

        IDamageable iDamage = other.gameObject.GetComponent<IDamageable>();
        iDamage?.TakeDamage(_damageAmount, _knockBackThrust);

        Instantiate(_bulletVFX, transform.position, transform.rotation);
    

        _gun.ReleaseBulletFromPool(this);




    }
}