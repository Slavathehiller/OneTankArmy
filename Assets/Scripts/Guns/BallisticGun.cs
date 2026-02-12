using UnityEngine;

public class BallisticGun : Gun
{
    [SerializeField] private GameObject _bulletPrefab;

    protected override void Fire()
    {
        foreach (var firePoint in _firePoints)
        {
            var bullet = Instantiate(_bulletPrefab);
            bullet.transform.position = firePoint.transform.position;
            bullet.transform.rotation = firePoint.transform.rotation;
            bullet.SetActive(true);
            _audioSourceFire.PlayOneShot(_fireSound);
        }
    }
}
