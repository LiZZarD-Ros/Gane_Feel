using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public Action OnExplode;
    public Action OnBeep;
    
    [SerializeField] private GameObject _explodeVfx;
    [SerializeField] private GameObject _greenadeLight;
    [SerializeField] private float _launchForce = 15f;
    [SerializeField] private float _torqueAmount = 2f;
    [SerializeField] private float _explosionRadius = 3.5f;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private int _damageAmount = 3;
    [SerializeField] private float _lightBlinkTime = .15f;
    [SerializeField] private int _totalBlinks = 3;
    [SerializeField] private int _explodeTime = 3;

    private int _currentBlinks;
    private Rigidbody2D _rigidbody;
    private CinemachineImpulseSource _impulseSource;

    private void OnEnable()
    {
        OnExplode += Explosion;
        OnExplode += GrenadeScreenShake;
        OnExplode += DamageNearby;
        OnExplode += AudioManager.instance.Grenade_OnExplosion;
        OnBeep += AudioManager.instance.Grenade_OnBeep;
        OnBeep += BlinkLight;
    }

    private void OnDisable()
    {
        OnExplode -= Explosion;
        OnExplode -= GrenadeScreenShake;
        OnExplode -= DamageNearby;
        OnExplode -= AudioManager.instance.Grenade_OnExplosion;
        OnBeep -= AudioManager.instance.Grenade_OnBeep;
        OnBeep -= BlinkLight;

    }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        LaunchGrenade();
        StartCoroutine(CountdownExplodeRoutine());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            OnExplode?.Invoke();
        }
    }

    private void LaunchGrenade()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToMouse = (mousePos - (Vector2)transform.position).normalized;
        _rigidbody.AddForce(directionToMouse * _launchForce, ForceMode2D.Impulse);
        _rigidbody.AddTorque(_torqueAmount, ForceMode2D.Force);
    }

    private void Explosion()
    {
        Instantiate(_explodeVfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void GrenadeScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    private void DamageNearby()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayerMask);
        foreach (Collider2D hit in hits)
        {
            Health health = hit.GetComponent<Health>();
            health?.TakeDamage(_damageAmount);
        }
    }

    private IEnumerator CountdownExplodeRoutine()
    {
        while (_currentBlinks < _totalBlinks)
        {
            yield return new WaitForSeconds(_explodeTime / _totalBlinks);
            OnBeep?.Invoke();
            yield return new WaitForSeconds(_lightBlinkTime);
            _greenadeLight.SetActive(false);

        }

        OnExplode?.Invoke();
    }

    private void BlinkLight()
    {
        _greenadeLight.SetActive(true);
        _currentBlinks++;
    }

}
