using System.Collections;
using UnityEngine;

namespace STH.Characters.Enemy
{
    public class EnemyHitEffect : MonoBehaviour
    {
        [Header("Flash Settings")]
        [SerializeField] private float flashDuration = 0.1f;
        [ColorUsage(true, true)]
        [SerializeField] private Color hitColor = Color.white;

        [Range(0.1f, 10f)]
        [SerializeField] private float hitFresnelPower = 5.0f;
        [Range(0.1f, 1f)]
        [SerializeField] private float flashAmount = 0.2f;

        [Header("Camera Shake Settings")]
        [SerializeField] private float shakeDuration = 0.15f;
        [SerializeField] private float shakeMagnitude = 0.1f;

        [Header("References")]
        [SerializeField] private PoolableParticle hitParticle;
        [SerializeField] private Transform cameraTransform;

        private Vector3 cameraOriginalPosition;

        private static readonly int FlashAmountId = Shader.PropertyToID("_FlashAmount");
        private static readonly int FlashColorId = Shader.PropertyToID("_FlashColor");
        private static readonly int FresnelPowerId = Shader.PropertyToID("_FresnelPower");


        private Renderer[] renderers;
        private MaterialPropertyBlock propBlock;
        private Coroutine hitCoroutine;
        private Coroutine shakeCoroutine;

        void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>();
            propBlock = new MaterialPropertyBlock();

            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
            cameraOriginalPosition = cameraTransform.localPosition;
        }

        void Start()
        {
            GameManager.Pool.CreatePool(hitParticle, 50);
        }

        [ContextMenu("Test Hit")]
        public void PlayHitEffect(bool shakeCamera = false)
        {
            if (hitCoroutine != null) StopCoroutine(hitCoroutine);
            hitCoroutine = StartCoroutine(HitFlashRoutine());

            PoolableParticle ga = GameManager.Pool.GetFromPool(hitParticle);
            ga.transform.position = transform.position;

            if (!shakeCamera) return;
            // 카메라 쉐이크 실행
            if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
            shakeCoroutine = StartCoroutine(CameraShakeCo(shakeDuration, shakeMagnitude));


        }

        private IEnumerator HitFlashRoutine()
        {
            // FlashAmount를 1에 가깝게 주어야 하얗게 변합니다
            ApplyFlash(flashAmount);

            yield return new WaitForSeconds(flashDuration);

            ApplyFlash(0f);
            hitCoroutine = null;
        }

        private void ApplyFlash(float amount)
        {
            foreach (var renderer in renderers)
            {
                renderer.GetPropertyBlock(propBlock);

                propBlock.SetFloat(FlashAmountId, amount);
                propBlock.SetColor(FlashColorId, hitColor);

                propBlock.SetFloat(FresnelPowerId, hitFresnelPower);

                renderer.SetPropertyBlock(propBlock);
            }
        }

        private IEnumerator CameraShakeCo(float duration, float magnitude)
        {
            Debug.Log("Camera Shake Start");
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float offsetX = Random.Range(-1f, 1f) * magnitude;
                float offsetY = Random.Range(-1f, 1f) * magnitude;

                cameraTransform.localPosition = cameraOriginalPosition + new Vector3(offsetX, offsetY, 0);

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            cameraTransform.localPosition = cameraOriginalPosition;
        }

        void OnDisable()
        {
            ApplyFlash(0f);
        }
    }
}