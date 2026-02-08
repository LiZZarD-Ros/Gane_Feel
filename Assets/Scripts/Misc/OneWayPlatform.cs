using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    [SerializeField] private float _disableColliderTime = 1.0f;
    
    private bool _playerOnPlatform = false;
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        DetectPPlayerInput();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            _playerOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            _playerOnPlatform = false;
        }
    }

    private void DetectPPlayerInput()
    {
        if (!_playerOnPlatform) return;

        if (PlayerController.Instance.MoveInput.y < 0f) 
        { 
           StartCoroutine(DisablePlatformColliderRoutine());
        }


    }

    private IEnumerator DisablePlatformColliderRoutine()
    {
        Collider2D[] playerColliders = PlayerController.Instance.GetComponents<Collider2D>();

        foreach (Collider2D playerCollider in playerColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, _collider, true);
        }


        yield return new WaitForSeconds(_disableColliderTime);

        foreach (Collider2D playerCollider in playerColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, _collider, false);
        }
    }


}
