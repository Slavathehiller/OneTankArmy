using UnityEngine;

public class TankController : BaseEntity
{
    [SerializeField] private Transform _healthBar;
    [SerializeField] private SpriteRenderer _healthBarRenderer;
    [SerializeField] private Sprite _destroyedSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioSource _audioSourceDrive;

    [SerializeField]
    private float _cabinsRotationSpeed = 1f;
    [SerializeField] 
    private GameObject[] _cabins;
    [SerializeField]
    private Vehicle _vehicle;

    private Vector3 _healthBarOffset;
    private float _healthBarMaxSize;


    void Start()
    {
        _healthBarOffset = _healthBar.localPosition;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _healthBarMaxSize = _healthBarRenderer.size.x;
        _vehicle.HealthChanges += RefreshHealth;
        _vehicle.HealthChanges += CheckIfDead;
        _vehicle.LoadPrefs();
    }

    private void RefreshHealth()
    {
        _healthBarRenderer.size = new Vector3(_healthBarMaxSize * _vehicle.Health / _vehicle.MaxHealth, _healthBarRenderer.size.y, 1);
    }

    void Update()
    {
        _healthBar.position = transform.position + _healthBarOffset;
        _healthBar.localRotation = Quaternion.Inverse(transform.rotation);
        CabinsFollowCursor();
    }

    private void CabinsFollowCursor()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        foreach (var cabin in _cabins)
        {
            Vector2 direction = mousePosition - cabin.transform.position;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            targetAngle -= 90;

            float currentAngle = cabin.transform.eulerAngles.z;

            float angle = Mathf.LerpAngle(currentAngle, targetAngle, _cabinsRotationSpeed * Time.deltaTime);

            cabin.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }

    protected override void CheckIfDead()
    {
        if (_vehicle.Health <= 0)
        {
            _audioSourceDrive.Stop();
            _spriteRenderer.sprite = _destroyedSprite;
            _healthBar.gameObject.SetActive(false);
            foreach(var cabin in _cabins)
                cabin.SetActive(false);
            this.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            RigidBody.AddForce(transform.up * _moveSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            RigidBody.AddForce(-transform.up * _moveSpeed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            RigidBody.AddTorque(_rotateSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            RigidBody.AddTorque(-_rotateSpeed);
        }

        if (RigidBody.linearVelocity.magnitude > 0.6f)
        {
            if (!_audioSourceDrive.isPlaying)
            {
                _audioSourceDrive.Play();
            }
        }
        else
        {
            _audioSourceDrive.Stop();
        }

        //int layerMask = 63;
        //int layerMask = 1 << 6;
        //layerMask = ~layerMask;

        //RaycastHit2D hit = Physics2D.Raycast(_firePoint.transform.position, transform.up, 5, layerMask);
        //Debug.DrawRay(_firePoint.transform.position, transform.up * 5, Color.yellow, 1);
        //if (hit)
        //{
        //    Debug.DrawRay(_firePoint.transform.position, transform.up * hit.distance, Color.red, 1);
        //    if (hit.collider.gameObject.TryGetComponent<Mine>(out _))
        //    {
        //        _mineWarning?.gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        _mineWarning?.gameObject.SetActive(false);
        //    }
        //}
        //else
        //    _mineWarning?.gameObject.SetActive(false);
    }      

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<DamageDealer>(out var dd))
        {
            TakeDamage(dd.Damage);
        }

        if (collision.gameObject.TryGetComponent<Mine>(out var mine))
        {
            TakeDamage(mine.Damage);
            mine.Explode();
        }

        if (collision.gameObject.TryGetComponent<Portal>(out var portal))
        {
            portal.LoadNextScene();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Electra>(out var electra))
        {
            TakeDamage(electra.DOT);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<DamageDealer>(out var dd))
        {
            TakeDamage(dd.Damage);
        }
    }
    public override void TakeDamage(float damage)
    {
        _vehicle.TakeDamage(damage);
    }

    private void OnDestroy()
    {
        _vehicle.HealthChanges -= RefreshHealth;
        _vehicle.HealthChanges -= CheckIfDead;
    }

}
