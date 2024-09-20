using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float ForceAmount = 10;
    public AudioClip Jump;
    public AudioClip Death;

    private Rigidbody2D _rigidBody2D;
    private AudioSource _audioSource;

    private float _currentTiltAngle = 0f;
    private float _tiltVelocity = 0f;
    public float TiltSmoothTime = 0.2f;
    public float FallDelay = 1f;

    private bool _isDead = false;

    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_isDead) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidBody2D.velocity = Vector2.zero;
            _rigidBody2D.AddForce(new Vector2(0, ForceAmount));
            _audioSource.PlayOneShot(Jump);
        }

        if (transform.position.y < -5.5)
        {
            StartCoroutine(HandleDeath());
        }

        AdjustTilt();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isDead) 
        {
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        _audioSource.PlayOneShot(Death);
        _isDead = true;
        _rigidBody2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(FallDelay);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        Game.Instance.Restart();
        _rigidBody2D.velocity = Vector2.zero;
        _currentTiltAngle = 0f;
        _isDead = false;
    }

    private void AdjustTilt()
    {
        float targetTiltAngle = Mathf.Lerp(-90, 90, (_rigidBody2D.velocity.y + 5) / 10);
        _currentTiltAngle = Mathf.SmoothDamp(_currentTiltAngle, targetTiltAngle, ref _tiltVelocity, TiltSmoothTime);
        transform.rotation = Quaternion.Euler(0, 0, _currentTiltAngle);
    }
}
