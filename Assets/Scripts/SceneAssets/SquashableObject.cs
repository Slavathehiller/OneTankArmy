using System;
using UnityEngine;

public class SquashableObject : MonoBehaviour
{    
    [SerializeField]
    private Sprite _squashedSprite;
    [SerializeField]
    private GameObject _activeStaff;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BaseEntity>(out var entity) && entity.TagCloud.Contains(Tag.Heavy)
            || collision.TryGetComponent<GravityProjectile>(out _))
        {
            Squash();
        }
    }

    private void Squash()
    {
        _spriteRenderer.sprite = _squashedSprite;
        _collider.enabled = false;
        if (_activeStaff != null)
            _activeStaff.SetActive(false);
    }
}
