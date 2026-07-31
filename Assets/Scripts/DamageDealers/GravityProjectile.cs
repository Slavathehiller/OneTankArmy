using Assets.Scripts.Player;
using UnityEngine;
using Random = UnityEngine.Random;

public class GravityProjectile : DamageDealer
{
    [Header("Настройки")]
    [SerializeField] private float _flightSpeed = 100f;
    [SerializeField] private float _flightDuration = 2f;

    [Header("Параметры линзы")]
    [SerializeField] private float _startRadius = 0.5f;
    [SerializeField] private float _startStrength = 0.15f;

    [Header("Параметры взрыва")]
    [SerializeField] private float _explosionDuration = 0.4f;
    [SerializeField] private float _maxExplosionRadius = 2.5f;
    [SerializeField] private float _maxExplosionStrength = 0.4f;

    [SerializeField] private float _forcedMoveScale = 8000;
    [SerializeField] private float _forcedRotateScale = 12000;

    private Material _lensMaterial;
    private float _flightTimer;
    private bool _isExploding;
    private float _explosionTimer;
    private Rigidbody2D _rigidBody;


    public override void ReactToCollision(GameObject collision) { }
    public void Init()
    {
        // Получаем материал из SpriteRenderer
        _lensMaterial = GetComponent<SpriteRenderer>().material;

        // Задаем начальные значения
        _lensMaterial.SetFloat("_Radius", _startRadius);
        _lensMaterial.SetFloat("_Strength", _startStrength);

        _flightTimer = _flightDuration;

        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.AddForce(transform.up * _flightSpeed);
    }

    private void Update()
    {
        if (!_isExploding)
        {
            _flightTimer -= Time.deltaTime;
            if (_flightTimer <= 0f)
            {
                StartExplosion();
            }
        }
        else
        {
            UpdateExplosion();
        }
    }

    private void StartExplosion()
    {
        _isExploding = true;
        _explosionTimer = 0f;
        _rigidBody.linearVelocity = Vector2.zero; 
    }

    private void UpdateExplosion()
    {
        _explosionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(_explosionTimer / _explosionDuration);

        // Плавное увеличение радиуса и силы искажения
        float currentRadius = Mathf.Lerp(_startRadius, _maxExplosionRadius, t);
        float currentStrength = Mathf.Lerp(_startStrength, _maxExplosionStrength, t);

        _lensMaterial.SetFloat("_Radius", currentRadius);
        _lensMaterial.SetFloat("_Strength", currentStrength);

        // Визуально увеличиваем сам спрайт, чтобы искажение покрывало большую площадь
        float scale = Mathf.Lerp(1f, _maxExplosionRadius / _startRadius, t);
        transform.localScale = Vector3.one * scale;

        if (t >= 1)
        {
            Destroy(gameObject); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerSide>(out var target))
        {
            var moveStrenght = new Vector3(Random.Range(-_startStrength, _startStrength), Random.Range(-_startStrength, _startStrength), 0) * _forcedMoveScale;
            var rotateStrenght = Random.Range(-_startStrength, _startStrength) * _forcedRotateScale;
            target.ForcedMove(moveStrenght, rotateStrenght);
        }
    }
}