using UnityEngine;
using System.Linq;

public class InteractiveShadows : MonoBehaviour
{
    [SerializeField] private Transform lightHead; // ������ ��ġ�� ����
    [SerializeField] private PolygonGenerator polygonGenerator; // �׸��� ���� �� ������ Ŭ����
    [SerializeField] private LightType lightType; // ������ ����
    [SerializeField] private LayerMask targetLayerMask; // �׸��ڰ� ������ ���̾� ����ũ

    private Vector3[] objectVertices; // ������Ʈ�� ���� ����
    private Vector3[] points;         // �׸��ڸ� �׸��� ���� ������ ��� �迭
    private Vector3 previousPosition, previousLightPosition;
    private Quaternion previousRotation, previousLightRotation;
    private Vector3 previousScale, previousLightScale;
    private bool canUpdateCollider = true; // �ݶ��̴� ������Ʈ �÷���

    [SerializeField][Range(0.02f, 1f)] private float shadowColliderUpdateTime = 0.08f; // �ݶ��̴� ������Ʈ �ֱ�

    private void Awake()
    {
        // ������Ʈ�� ������ ��� �ߺ��� ������ �����Ͽ� �迭�� ����
        objectVertices = transform.GetComponent<MeshFilter>().mesh.vertices.Distinct().ToArray();
        // ������Ʈ�� ������ŭ �迭�Ҵ�
        points = new Vector3[objectVertices.Length];
    }

    private void FixedUpdate()
    {
        // ������Ʈ�� ������ ��ġ, ȸ�� �Ǵ� ũ�⿡ ��ȭ�� ���� ���� ������Ʈ
        if (TransformHasChanged() && canUpdateCollider)
        {
            UpdateShadowVertices();
            canUpdateCollider = false; // �ߺ� ������Ʈ ����
            Invoke(nameof(ResetColliderUpdateFlag), shadowColliderUpdateTime); // ���� �ð� �� ������Ʈ �����ϵ��� �÷��� ����
        }

        // ���� ���¸� ����Ͽ� ���� ������Ʈ���� ��
        previousPosition = transform.position;
        previousRotation = transform.rotation;
        previousScale = transform.localScale;
        previousLightPosition = lightHead.position;
        previousLightRotation = lightHead.rotation;
        previousLightScale = lightHead.localScale;
    }

    private void ResetColliderUpdateFlag()
    {
        canUpdateCollider = true; // ������Ʈ ���� ���·� �ǵ���
    }

    private void UpdateShadowVertices()
    {
        Vector3 raycastDirection = lightHead.forward; // �⺻ ���� ���� (Directional Light ��� ��)

        for (int i = 0; i < objectVertices.Length; i++)
        {
            // ������Ʈ ������ ���� ��ǥ�� ��ȯ
            Vector3 point = transform.TransformPoint(objectVertices[i]);
            // ������ Directional�� �ƴ� ��� �� �������� ������ ���ϴ� ���͸� ���
            if (lightType != LightType.Directional)
            {
                raycastDirection = point - lightHead.position;
            }

            // ����ĳ��Ʈ�� �׸��� ������ ����Ͽ� �迭�� ����
            points[i] = GetShadowVerticesPos(point, raycastDirection);
        }

        // ������ �����ϰ� �������� �׸�
        polygonGenerator.DrawPolygon(SortVertices(points));
    }

    private Vector3[] SortVertices(Vector3[] _vertices)
    {
        // ������ �߽��� ����Ͽ� �ݽð� �������� ����, Aggregate�Լ� : ��� ��ҵ��� �����ؼ� � ����� �����س��� �Լ�
        Vector3 center = _vertices.Aggregate(Vector3.zero, (sum, v) => sum + v) / _vertices.Length;

        // center�� �������� �ݽð�������� ����
        // Atan�� �� ���� x��� �̷�� ������ �� �� �ֱ� ������ �������� ��� ����
        return _vertices.OrderBy(v => Mathf.Atan2(v.z - center.z, v.x - center.x)).ToArray();
    }

    private Vector3 GetShadowVerticesPos(Vector3 _fromPosition, Vector3 _direction)
    {
        RaycastHit hit;
        // ����ĳ��Ʈ�� ������ ��ġ�� ��
        if (Physics.Raycast(_fromPosition, _direction, out hit, 100f, targetLayerMask))
        {
            return hit.point - transform.position + new Vector3(0.01f, 0.01f, 0.01f); // ��������� ��ħ�� �����ϱ� ���� ������ ������ �߰�
        }
        return _fromPosition + 50 * _direction - transform.position; // �浹 ���н�, �ִ� �Ÿ� �����Ͽ� ������ ���� ����
    }

    private bool TransformHasChanged()
    {
        // ���� ���¿� ���Ͽ� ��ȭ�� �ִ��� Ȯ��
        return previousPosition != transform.position || previousRotation != transform.rotation || previousScale != transform.localScale
            || previousLightPosition != lightHead.position || previousLightRotation != lightHead.rotation || previousLightScale != lightHead.localScale;
    }
}
