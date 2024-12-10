using System.Collections;
using System.Drawing;
using UnityEngine;

public class Stage2Camera : MonoBehaviour
{
    public static Stage2Camera i;
    private Camera camera;
    [SerializeField] float zoomOut; // �� �ƿ� ũ��
    [SerializeField] float zoomSpeed; // �� �ƿ� �ӵ�
    [SerializeField] float targetY; // ��ǥ Y ��ġ
    [SerializeField] float moveSpeed; // �̵� �ӵ�
    [SerializeField] float fixedX; // ������ X ��ġ
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
        transform.position = new Vector3(fixedX, 15.6f, transform.position.z); // X��ǥ ����
        // �� �ƿ�
        float elapsedTime = 0f;
        float currentSize = camera.orthographicSize;
        float zoomOutTime = Mathf.Abs(zoomOut - currentSize) / zoomSpeed;

        while (elapsedTime < zoomOutTime)
        {
            camera.orthographicSize = Mathf.Lerp(currentSize, zoomOut, elapsedTime / zoomOutTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        camera.orthographicSize = zoomOut; // ���������� �� �ƿ� ũ��� ����

        // ī�޶� �̵�
        Vector3 targetPosition = new Vector3(fixedX, targetY, transform.position.z);
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition; // ���� ��ġ ����

        // ���� ���·� ���ư���
        yield return new WaitForSeconds(.5f); // ��� ��� ��


        camera.orthographicSize = 5; // ���������� ���� ũ��� ����
        background.transform.localScale = originalScale;
        background.transform.position = backgroundPosition;
        isEffectRunning = false;
    }
}
