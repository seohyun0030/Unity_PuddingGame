using System.Collections;
using System.Drawing;
using UnityEngine;

public class Stage2Camera : MonoBehaviour
{
    public static Stage2Camera i;
    private Camera camera;
    [SerializeField] float zoomOut; // 줌 아웃 크기
    [SerializeField] float zoomSpeed; // 줌 아웃 속도
    [SerializeField] float targetY; // 목표 Y 위치
    [SerializeField] float moveSpeed; // 이동 속도
    [SerializeField] float fixedX; // 고정될 X 위치
    [SerializeField] float size = 1.5f;

    [SerializeField] GameObject background;

    public bool isEffectRunning = false;

    private void Awake()
    {
        i = this;
        camera = GetComponent<Camera>();
        TriggerCameraEffect();
    }

    public void TriggerCameraEffect()
    {
        if (!isEffectRunning)
        {
            StartCoroutine(CameraZoomAndMove());
        }
    }

    private IEnumerator CameraZoomAndMove()
    {
        Vector3 backgroundPosition = background.transform.position;
        Vector2 originalScale = background.transform.localScale;
        background.transform.position = new Vector3(fixedX, 46.9f, background.transform.position.z);
        Vector2 scale = background.transform.localScale;
        scale.x *= size;
        scale.y *= size;
        background.transform.localScale = scale;
        isEffectRunning = true;
        transform.position = new Vector3(fixedX, transform.position.y, transform.position.z); // X좌표 설정
        // 줌 아웃
        float elapsedTime = 0f;
        float currentSize = camera.orthographicSize;
        float zoomOutTime = Mathf.Abs(zoomOut - currentSize) / zoomSpeed;

        while (elapsedTime < zoomOutTime)
        {
            camera.orthographicSize = Mathf.Lerp(currentSize, zoomOut, elapsedTime / zoomOutTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        camera.orthographicSize = zoomOut; // 최종적으로 줌 아웃 크기로 설정

        // 카메라 이동
        Vector3 targetPosition = new Vector3(fixedX, targetY, transform.position.z);
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition; // 최종 위치 설정

        // 원래 상태로 돌아가기
        yield return new WaitForSeconds(.5f); // 잠시 대기 후


        camera.orthographicSize = 5; // 최종적으로 원래 크기로 설정
        background.transform.localScale = originalScale;
        background.transform.position = backgroundPosition;
        isEffectRunning = false;
    }
}
