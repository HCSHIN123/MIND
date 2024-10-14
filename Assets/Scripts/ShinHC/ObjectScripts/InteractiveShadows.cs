using UnityEngine;
using System.Linq;

public class InteractiveShadows : MonoBehaviour
{
    [SerializeField] private Transform lightHead; // 광원의 위치와 방향
    [SerializeField] private PolygonGenerator polygonGenerator; // 그림자 생성 및 렌더링 클래스
    [SerializeField] private LightType lightType; // 광원의 종류
    [SerializeField] private LayerMask targetLayerMask; // 그림자가 생성될 레이어 마스크

    private Vector3[] objectVertices; // 오브젝트의 정점 정보
    private Vector3[] points;         // 그림자를 그릴때 사용될 정점을 담는 배열
    private Vector3 previousPosition, previousLightPosition;
    private Quaternion previousRotation, previousLightRotation;
    private Vector3 previousScale, previousLightScale;
    private bool canUpdateCollider = true; // 콜라이더 업데이트 플래그

    [SerializeField][Range(0.02f, 1f)] private float shadowColliderUpdateTime = 0.08f; // 콜라이더 업데이트 주기

    private void Awake()
    {
        // 오브젝트의 정점을 얻고 중복된 정점을 제거하여 배열로 저장
        objectVertices = transform.GetComponent<MeshFilter>().mesh.vertices.Distinct().ToArray();
        // 오브젝트의 정점만큼 배열할당
        points = new Vector3[objectVertices.Length];
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

        for (int i = 0; i < objectVertices.Length; i++)
        {
            // 오브젝트 정점을 월드 좌표로 변환
            Vector3 point = transform.TransformPoint(objectVertices[i]);
            // 광원이 Directional이 아닌 경우 각 정점에서 광원을 향하는 벡터를 사용
            if (lightType != LightType.Directional)
            {
                raycastDirection = point - lightHead.position;
            }

            // 레이캐스트로 그림자 정점을 계산하여 배열에 저장
            points[i] = GetShadowVerticesPos(point, raycastDirection);
        }

        // 정점을 정렬하고 폴리곤을 그림
        polygonGenerator.DrawPolygon(SortVertices(points));
    }

    private Vector3[] SortVertices(Vector3[] _vertices)
    {
        // 정점의 중심을 계산하여 반시계 방향으로 정렬, Aggregate함수 : 모든 요소들을 결합해서 어떤 결과를 도출해내는 함수
        Vector3 center = _vertices.Aggregate(Vector3.zero, (sum, v) => sum + v) / _vertices.Length;

        // center을 기준으로 반시계방향으로 정렬
        // Atan은 한 점이 x축과 이루는 각도를 알 수 있기 때문에 기준으로 삼아 정렬
        return _vertices.OrderBy(v => Mathf.Atan2(v.z - center.z, v.x - center.x)).ToArray();
    }

    private Vector3 GetShadowVerticesPos(Vector3 _fromPosition, Vector3 _direction)
    {
        RaycastHit hit;
        // 레이캐스트로 정점의 위치를 얻어냄
        if (Physics.Raycast(_fromPosition, _direction, out hit, 100f, targetLayerMask))
        {
            return hit.point - transform.position + new Vector3(0.01f, 0.01f, 0.01f); // 접촉지면과 겹침을 방지하기 위해 적절한 오프셋 추가
        }
        return _fromPosition + 50 * _direction - transform.position; // 충돌 실패시, 최대 거리 조정하여 과도한 연산 방지
    }

    private bool TransformHasChanged()
    {
        // 이전 상태와 비교하여 변화가 있는지 확인
        return previousPosition != transform.position || previousRotation != transform.rotation || previousScale != transform.localScale
            || previousLightPosition != lightHead.position || previousLightRotation != lightHead.rotation || previousLightScale != lightHead.localScale;
    }
}
