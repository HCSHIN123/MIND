using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class InteractiveShadows : MonoBehaviour
{
    [SerializeField] private Transform lightHead; // 광원의 위치와 방향
    [SerializeField] private PolygonGenerator polygonGenerator; // 그림자 생성 및 렌더링 클래스
    [SerializeField] private LightType lightType; // 광원의 종류
    [SerializeField] private LayerMask targetLayerMask; // 그림자가 생성될 레이어 마스크

    private Vector3[] objectVertices; // 오브젝트의 정점 정보
   
    private Vector3 previousPosition, previousLightPosition;
    private Quaternion previousRotation, previousLightRotation;
    private Vector3 previousScale, previousLightScale;
    private bool canUpdateCollider = true; // 콜라이더 업데이트 플래그

    [SerializeField][Range(0.02f, 1f)] private float shadowColliderUpdateTime = 0.08f; // 콜라이더 업데이트 주기
    private const float validRayDistance = 100f; // 레이캐스트 최대 거리
    private const float defaultOffset = 0.01f; // 겹침 방지 오프셋
    private const float missedRayDistance = 50f; // 충돌 실패 시 확장 거리
    List<Vector3> validPoints = new List<Vector3>(); // 유효한 정점을 저장할 리스트
    private void Awake()
    {
        // 오브젝트의 정점을 얻고 중복된 정점을 제거하여 배열로 저장
        objectVertices = transform.GetComponent<MeshFilter>().mesh.vertices.Distinct().ToArray();
    }

    private void FixedUpdate()
    {
        // 오브젝트나 광원의 위치, 회전 또는 크기에 변화가 있을 때만 업데이트
        if (TransformHasChanged() && canUpdateCollider)
        {
            UpdateShadowVertices();
            canUpdateCollider = false; // 중복 업데이트 방지
            Invoke(nameof(ResetColliderUpdateFlag), shadowColliderUpdateTime); // 일정 시간 후 업데이트 가능하도록 플래그 리셋
        }

        // 현재 상태를 기록하여 다음 업데이트에서 비교
        previousPosition = transform.position;
        previousRotation = transform.rotation;
        previousScale = transform.localScale;
        previousLightPosition = lightHead.position;
        previousLightRotation = lightHead.rotation;
        previousLightScale = lightHead.localScale;
    }

    private void ResetColliderUpdateFlag()
    {
        canUpdateCollider = true; // 업데이트 가능 상태로 되돌림
    }

    private void UpdateShadowVertices()
    {
        Vector3 raycastDirection = lightHead.forward; // 기본 광원 방향 (Directional Light 사용 시)
        validPoints.Clear();  // 유효한 정점리스트 초기화

        for (int i = 0; i < objectVertices.Length; i++)
        {
            // 오브젝트 정점을 월드 좌표로 변환
            Vector3 point = transform.TransformPoint(objectVertices[i]);

            // 광원이 Directional이 아닌 경우 각 광원에서 정점으로 향하는 벡터를 사용
            if (lightType != LightType.Directional)
            {
                raycastDirection = point - lightHead.position;
                raycastDirection.Normalize();
            }

            // 레이캐스트로 그림자 정점을 계산, 유효한 정점만 저장
            if (GetShadowVerticesPos(point, raycastDirection, out Vector3 shadowVertex))
            {
                validPoints.Add(shadowVertex);
            }
        }

        // 유효한 정점이 있을 경우에만 폴리곤을 그림
        if (validPoints.Count > 0)
        {
            SortVertices(ref validPoints);
            polygonGenerator.DrawPolygon(validPoints);
        }
    }

    private void SortVertices(ref List<Vector3> _vertices)
    {
        // 정점의 중심을 계산
        Vector3 center = _vertices.Aggregate(Vector3.zero, (sum, v) => sum + v) / _vertices.Count;

        // 각도를 기준으로 반시계방향으로 정렬 (중심에서 각 정점으로 향하는 각도를 사용)
        _vertices.Sort((a, b) => Mathf.Atan2(a.z - center.z, a.x - center.x).CompareTo(Mathf.Atan2(b.z - center.z, b.x - center.x)));
    }

    private bool GetShadowVerticesPos(Vector3 _fromPosition, Vector3 _direction, out Vector3 shadowVertex)
    {
        // 레이캐스트 수행
        if (Physics.Raycast(_fromPosition, _direction, out RaycastHit hit, validRayDistance, targetLayerMask))
        {
            // 유효한 거리에 충돌한 경우, 정점 저장 후 true 반환
            shadowVertex = hit.point - transform.position + new Vector3(defaultOffset, defaultOffset, defaultOffset);
            return true;
        }
        // 유효한 거리에 충돌하지 않은 경우 false 반환
        shadowVertex = Vector3.zero;
        return false;
    }

    private bool TransformHasChanged()
    {
        // 이전 상태와 비교하여 변화가 있는지 확인
        return previousPosition != transform.position || previousRotation != transform.rotation || previousScale != transform.localScale
            || previousLightPosition != lightHead.position || previousLightRotation != lightHead.rotation || previousLightScale != lightHead.localScale;
    }
}
