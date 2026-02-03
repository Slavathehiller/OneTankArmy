using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private GameObject _enemy;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _firePoint;
    private float _cooldownTime = 0.5f;
    private float _cooldown = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemy != null) 
            RotateToObject2D(_enemy);



    }

    private void FixedUpdate()
    {
        int layerMask = 1 << 6;

        RaycastHit2D hit = Physics2D.Raycast(_firePoint.transform.position, transform.up, 5, layerMask);
        Debug.DrawRay(_firePoint.transform.position, transform.up * 5, Color.yellow, 1);
        if (hit)
        {
            Debug.DrawRay(_firePoint.transform.position, transform.up * hit.distance, Color.red, 1);
            if (hit.collider.gameObject.TryGetComponent<TankController>(out _))
            {
                if (_cooldown <= 0)
                    Fire();
            }

        }
        if(_cooldown > 0)
            _cooldown -= Time.deltaTime;
    }

    private void Fire()
    {
        var bullet = Instantiate(_bulletPrefab);
        bullet.transform.position = _firePoint.transform.position;
        bullet.transform.rotation = _firePoint.transform.rotation;
        bullet.SetActive(true);
        //_audioSourceFire.PlayOneShot(_fireSound);
        Destroy(bullet, 5);
        _cooldown = _cooldownTime;
    }

    public void FoundEnemy(GameObject enemy)
    {
        _enemy = enemy;
    }

    public void LostEnemy()
    {
        _enemy = null;
    }


    private void RotateToObject2D(GameObject objectToRotate)
    {
        var targetPosition = _enemy.transform.position - transform.position;
        var angle = Vector3.Angle(transform.up, targetPosition);
        var cross = Vector3.Cross(transform.up, targetPosition);
        if (cross.z < 0) // Проверяем знак y координаты векторного произведения
        {
            angle = -angle; // Если знак отрицательный, инвертируем угол
        }

        transform.Rotate(0, 0, angle);
    }
}
