using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;

    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunFireCD = .5f;


    private Vector2 _mousePos;
    private float _lastFireTime = 0f;

    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += ResetLastFireTime;
    }

    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= _lastFireTime) {
            OnShoot?.Invoke();
            
        }
    }

    private void ShootProjectile()
    {
        
        Bullet newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        newBullet.Init(_bulletSpawnPoint.position, _mousePos);
    }

    private void ResetLastFireTime()
    {
      _lastFireTime = Time.time + _gunFireCD;
    }

    private void RotateGun()
    {
       _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 direction = _mousePos - (Vector2)PlayerController.Instance.transform.position;
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0,0,angle);
 
    }

}
