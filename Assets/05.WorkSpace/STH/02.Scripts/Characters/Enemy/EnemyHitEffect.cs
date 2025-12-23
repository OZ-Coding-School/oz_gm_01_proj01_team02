using System.Collections;
using UnityEngine;

namespace STH.Characters.Enemy
{
    public class EnemyHitEffect : MonoBehaviour
    {
        [Header("설정")]
        [SerializeField] private float flashDuration = 0.1f;
        [ColorUsage(true, true)]
        [SerializeField] private Color hitColor = Color.white;

        [Range(0.1f, 10f)]
        [SerializeField] private float hitFresnelPower = 5.0f;

        [Range(0.1f, 1f)]
        [SerializeField] private float flashAmount = 0.2f;

        [SerializeField] private PoolableParticle hitParticle;

        private static readonly int FlashAmountId = Shader.PropertyToID("_FlashAmount");
        private static readonly int FlashColorId = Shader.PropertyToID("_FlashColor");
        private static readonly int FresnelPowerId = Shader.PropertyToID("_FresnelPower");


        private Renderer[] _renderers;
        private MaterialPropertyBlock _propBlock;
        private Coroutine _hitCoroutine;

        void Awake()
        {
            _renderers = GetComponentsInChildren<Renderer>();
            _propBlock = new MaterialPropertyBlock();
        }

        void Start()
        {
            GameManager.Pool.CreatePool(hitParticle, 20);
        }

        [ContextMenu("Test Hit")]
        public void PlayHitEffect()
        {
            if (_hitCoroutine != null) StopCoroutine(_hitCoroutine);
            _hitCoroutine = StartCoroutine(HitFlashRoutine());

            PoolableParticle ga = GameManager.Pool.GetFromPool(hitParticle);
            ga.transform.position = transform.position;
        }

        private IEnumerator HitFlashRoutine()
        {
            // FlashAmount를 1에 가깝게 주어야 하얗게 변합니다
            ApplyFlash(flashAmount);

            yield return new WaitForSeconds(flashDuration);

            ApplyFlash(0f);
            _hitCoroutine = null;
        }

        private void ApplyFlash(float amount)
        {
            foreach (var renderer in _renderers)
            {
                renderer.GetPropertyBlock(_propBlock);

                _propBlock.SetFloat(FlashAmountId, amount);
                _propBlock.SetColor(FlashColorId, hitColor);

                _propBlock.SetFloat(FresnelPowerId, hitFresnelPower);

                renderer.SetPropertyBlock(_propBlock);
            }
        }

        void OnDisable()
        {
            ApplyFlash(0f);
        }
    }
}