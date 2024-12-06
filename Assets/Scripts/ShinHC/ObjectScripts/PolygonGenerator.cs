using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PolygonGenerator : MonoBehaviour
{
    private Mesh mesh; // 다각형의 메쉬
    private int[] indices; // 정점을 잇는 삼각형 정보
    private MeshCollider meshCollider; // 다각형의 충돌체

    private List<int> indicesTemp = new List<int>();
    private void Awake()
    {
        mesh = new Mesh();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
    }

    private void Update()
    {
        transform.rotation = Quaternion.identity; // 회전값을 초기화하여 안정적인 그리기
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
            Debug.Log("그림자에 진입");
    }

    public void DrawPolygon(List<Vector3> _vertices)
    {
        // 삼각형 인덱스 배열을 설정하여 폴리곤 그리기 준비
        DrawFilledIndices(_vertices.Count);

        // 정점 정보를 바탕으로 폴리곤을 생성하고 이를 메쉬에 적용
        GeneratePolygon(_vertices.ToArray(), indices);

        // 생성된 메쉬를 콜라이더에 설정하여 물리적 충돌 처리를 가능하게 함
        meshCollider.sharedMesh = mesh;
    }

    private void DrawFilledIndices(int verticesCount)
    {
        indicesTemp.Clear();    // 기존 임시인덱스 정보 초기화
        int triangleCount = verticesCount - 2;
        // 삼각형을 그리기 위한 인덱스순서 저장
        for (int i = 0; i < triangleCount; ++i)
        {
            indicesTemp.Add(0);
            indicesTemp.Add(i + 2);
            indicesTemp.Add(i + 1);
        }
        indices = indicesTemp.ToArray(); // 배열 형태로 인덱스순서 정보 저장
    }

    private void GeneratePolygon(Vector3[] vertices, int[] indices)
    {
        mesh.Clear(); // 기존 메쉬 데이터 초기화
        mesh.vertices = vertices; // 정점 정보 적용
        mesh.triangles = indices; // 인덱스 순서 적용
    }
}
