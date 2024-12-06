using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class InteractiveShadows : MonoBehaviour
{
    [SerializeField] private Transform lightHead; // ������ ��ġ�� ����
    [SerializeField] private PolygonGenerator polygonGenerator; // �׸��� ���� �� ������ Ŭ����
    [SerializeField] private LightType lightType; // ������ ����
    [SerializeField] private LayerMask targetLayerMask; // �׸��ڰ� ������ ���̾� ����ũ

    private Vector3[] objectVertices; // ������Ʈ�� ���� ����
   
    private Vector3 previousPosition, previousLightPosition;
    private Quaternion previousRotation, previousLightRotation;
    private Vector3 previousScale, previousLightScale;
    private bool canUpdateCollider = true; // �ݶ��̴� ������Ʈ �÷���

    [SerializeField][Range(0.02f, 1f)] private float shadowColliderUpdateTime = 0.08f; // �ݶ��̴� ������Ʈ �ֱ�
    private const float validRayDistance = 100f; // ����ĳ��Ʈ �ִ� �Ÿ�
    private const float defaultOffset = 0.01f; // ��ħ ���� ������
    private const float missedRayDistance = 50f; // �浹 ���� �� Ȯ�� �Ÿ�
    List<Vector3> validPoints = new List<Vector3>(); // ��ȿ�� ������ ������ ����Ʈ
    private void Awake()
    {
        // ������Ʈ�� ������ ��� �ߺ��� ������ �����Ͽ� �迭�� ����
        objectVertices = transform.GetComponent<MeshFilter>().mesh.vertices.Distinct().ToArray();
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
        validPoints.Clear();  // ��ȿ�� ��������Ʈ �ʱ�ȭ

        for (int i = 0; i < objectVertices.Length; i++)
        {
            // ������Ʈ ������ ���� ��ǥ�� ��ȯ
            Vector3 point = transform.TransformPoint(objectVertices[i]);

            // ������ Directional�� �ƴ� ��� �� �������� �������� ���ϴ� ���͸� ���
            if (lightType != LightType.Directional)
            {
                raycastDirection = point - lightHead.position;
                raycastDirection.Normalize();
            }

            // ����ĳ��Ʈ�� �׸��� ������ ���, ��ȿ�� ������ ����
            if (GetShadowVerticesPos(point, raycastDirection, out Vector3 shadowVertex))
            {
                validPoints.Add(shadowVertex);
            }
        }

        // ��ȿ�� ������ ���� ��쿡�� �������� �׸�
        if (validPoints.Count > 0)
        {
            SortVertices(ref validPoints);
            polygonGenerator.DrawPolygon(validPoints);
        }
    }

    private void SortVertices(ref List<Vector3> _vertices)
    {
        // ������ �߽��� ���
        Vector3 center = _vertices.Aggregate(Vector3.zero, (sum, v) => sum + v) / _vertices.Count;

        // ������ �������� �ݽð�������� ���� (�߽ɿ��� �� �������� ���ϴ� ������ ���)
        _vertices.Sort((a, b) => Mathf.Atan2(a.z - center.z, a.x - center.x).CompareTo(Mathf.Atan2(b.z - center.z, b.x - center.x)));
    }

    private bool GetShadowVerticesPos(Vector3 _fromPosition, Vector3 _direction, out Vector3 shadowVertex)
    {
        // ����ĳ��Ʈ ����
        if (Physics.Raycast(_fromPosition, _direction, out RaycastHit hit, validRayDistance, targetLayerMask))
        {
            // ��ȿ�� �Ÿ��� �浹�� ���, ���� ���� �� true ��ȯ
            shadowVertex = hit.point - transform.position + new Vector3(defaultOffset, defaultOffset, defaultOffset);
            return true;
        }
        // ��ȿ�� �Ÿ��� �浹���� ���� ��� false ��ȯ
        shadowVertex = Vector3.zero;
        return false;
    }

    private bool TransformHasChanged()
    {
        // ���� ���¿� ���Ͽ� ��ȭ�� �ִ��� Ȯ��
        return previousPosition != transform.position || previousRotation != transform.rotation || previousScale != transform.localScale
            || previousLightPosition != lightHead.position || previousLightRotation != lightHead.rotation || previousLightScale != lightHead.localScale;
    }
}
