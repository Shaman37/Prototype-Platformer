using UnityEngine;

namespace PlayerController2D
{
	public class SpriteAfterImage : MonoBehaviour
	{

		private float _activeTime = 0.1f;
		private float _timeActivated;
		private float _alpha;
		private float _alphaSet = 0.8f;
		private float _alphaMultiplier = 0.85f;
		
		private Transform _player;
		
		private SpriteRenderer _afterImageSpriteRenderer;
		private SpriteRenderer _playerSpriteRenderer;
		private Color _color;
		
		private void OnEnable()
		{
            _afterImageSpriteRenderer = GetComponent<SpriteRenderer>();
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _playerSpriteRenderer = _player.GetComponent<SpriteRenderer>();

            transform.position = _player.position;
            transform.rotation = _player.rotation;
            
            _alpha = _alphaSet;
            _afterImageSpriteRenderer.sprite = _playerSpriteRenderer.sprite;
			_timeActivated = Time.time;
        }

		private void Update()
		{
            _alpha *= _alphaMultiplier;
            _color = new Color(1f, 1f, 1f, _alpha);
            _afterImageSpriteRenderer.color = _color;

			if (Time.time >= _timeActivated + _activeTime)
			{
                AfterImagePool.Instance.AddToPool(gameObject);
            }
        }
	}
}
