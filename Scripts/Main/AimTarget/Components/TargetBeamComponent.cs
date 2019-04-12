using Core.MessageBus;
using Main.AimTarget.Data;
using UnityEngine;

namespace Main.AimTarget.Components
{
    public class TargetBeamComponent : SubscriberBehaviour
    {
        private AimTargetData _aimTargetData;
        
        [SerializeField] private LineRenderer _lineRenderer;

        private RaycastHit _rayCastHit;
        private Vector3 _direction;
        
        protected override void Awake()
        {
            base.Awake();

            _aimTargetData = gameObject.GetComponent<AimTargetData>();
            
            _lineRenderer.positionCount = 2;
            _lineRenderer.startWidth = 0.02f;
            _lineRenderer.endWidth = 0.02f;
			
            _lineRenderer.enabled = true;
        }

        private void Update()
        {
            if (!_aimTargetData.ShootPosition) return;
            
            _lineRenderer.SetPosition(0, _aimTargetData.ShootPosition.position);
			
            _direction = (_aimTargetData.ResultTargetObject.position - _aimTargetData.ShootPosition.position).normalized;

            if (Physics.Raycast(_aimTargetData.ShootPosition.position, _direction, out _rayCastHit))
            {
                _lineRenderer.enabled = true;
                _lineRenderer.SetPosition(1, _rayCastHit.point);

                _aimTargetData.StickyAimObject.gameObject.SetActive(true);
                _aimTargetData.StickyAimObject.position = _rayCastHit.point;
            }
            else
            {
                _lineRenderer.SetPosition(1, _aimTargetData.ShootPosition.position + _direction * 100.0f);
                _aimTargetData.StickyAimObject.gameObject.SetActive(false);
            }

            void OnDisable()
            {
                Unsubscribe();
            }

            void OnEnable()
            {
                Subscribe();
            }
        }
    }
}