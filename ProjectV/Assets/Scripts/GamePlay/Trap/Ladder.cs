using UnityEngine;

namespace GamePlay.Trap
{
    public class Ladder : MonoBehaviour
    {
        private float _speed;
        private bool _isUp=true;
    
        private Animator _animator;
        private Collider _collider;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _animator = GetComponent<Animator>();
        }


        public void Moving()
        {
            if (_isUp)
            {
                _collider.isTrigger = false;
                _animator.SetBool($"MoveUp", _isUp);
                _isUp = false;
            }
            else
            {
                _collider.isTrigger = true;
                _animator.SetBool($"MoveUp", _isUp);
                _isUp = true;
            }
        }
    }
}