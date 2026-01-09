using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;

    private Vector2 _mousePos;
    
    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0)) {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        Bullet newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
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
