using TMPro;
using UnityEngine;

public class TankController : BaseEntity
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _firePoint;

    [SerializeField] private Transform _healthBar;
    [SerializeField] private SpriteRenderer _healthBarRenderer;
    [SerializeField] private Sprite _destroyedSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioSource _audioSourceDrive;
    [SerializeField] private AudioSource _audioSourceFire;
    [SerializeField] private AudioClip _fireSound;

    [SerializeField] private GameObject[] _cabins;
    [SerializeField] private float _cabinsRotationSpeed = 1f;

    private Vector3 _healthBarOffset;
    private float _healthBarMaxSize;

    private Tank _tank;

    // Start is called before the first frame update
    void Start()
    {
        _healthBarOffset = _healthBar.localPosition;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _healthBarMaxSize = _healthBarRenderer.size.x;
        _tank = new Tank();
        _tank.HealthChanges += RefreshHealth;
        _tank.HealthChanges += CheckIfDead;
        _tank.LoadPrefs();
    }

    private void RefreshHealth()
    {
        _healthBarRenderer.size = new Vector3(_healthBarMaxSize * _tank.Health / _tank.MaxHealth, _healthBarRenderer.size.y, 1);
    }

    void Update()
    {
        _healthBar.position = transform.position + _healthBarOffset;
        _healthBar.localRotation = Quaternion.Inverse(transform.rotation);
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // важно для 2D!
        foreach (var cabin in _cabins)
        {
            // Направление от объекта к курсору
            Vector2 direction = mousePosition - cabin.transform.position;

            // Угол в градусах
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            targetAngle -= 90;

            // Текущий угол объекта
            float currentAngle = cabin.transform.eulerAngles.z;

            // Плавно поворачиваем к цели
            float angle = Mathf.LerpAngle(currentAngle, targetAngle, _cabinsRotationSpeed * Time.deltaTime);            

            cabin.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }

    private void Fire()
    {
        var bullet = Instantiate(_bulletPrefab);
        bullet.transform.position = _firePoint.transform.position;
        bullet.transform.rotation = _firePoint.transform.rotation;
        bullet.SetActive(true);
        _audioSourceFire.PlayOneShot(_fireSound);
       // Destroy(bullet, 5);
    }
    protected override void CheckIfDead()
    {
        if (_tank.Health <= 0)
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
           // dd.gameObject.SetActive(false);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<FirePit>(out var firepit))
        {
            TakeDamage(firepit.DOT);
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

    public void TakeDamage(float damage)
    {
        _tank.TakeDamage(damage);
    }

    private void OnDestroy()
    {
        _tank.HealthChanges -= RefreshHealth;
        _tank.HealthChanges -= CheckIfDead;
    }

}
