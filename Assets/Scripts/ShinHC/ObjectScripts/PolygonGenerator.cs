using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PolygonGenerator : MonoBehaviour
{
    private Mesh mesh; // 다각형의 메쉬
    private Vector3[] vertices; // 다각형의 정점들
    private int[] indices; // 정점을 잇는 삼각형 정보
    private MeshCollider meshCollider; // 다각형의 충돌체

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

    public void DrawPolygon(Vector3[] _vertices)
    {
        vertices = _vertices;
        indices = DrawFilledIndices(vertices);  // 삼각형 인덱스배열 세팅
        GeneratePolygon(vertices, indices);     // 모든 정보를 메쉬에 적용하여 폴리곤생성
        meshCollider.sharedMesh = mesh;         // 메쉬를 콜라이더로 설정
    }

    private int[] DrawFilledIndices(Vector3[] vertices)
    {
        // 정점을 잇는 삼각형을 생성하여 인덱스 배열 생성
        int triangleCount = vertices.Length - 2;
        List<int> indices = new List<int>();
        for (int i = 0; i < triangleCount; ++i)
        {
            indices.Add(0);
            indices.Add(i + 2);
            indices.Add(i + 1);
        }
        return indices.ToArray();
    }

    private void GeneratePolygon(Vector3[] vertices, int[] indices)
    {
        mesh.Clear(); // 기존 메쉬 데이터 초기화
        mesh.vertices = vertices; // 정점 설정
        mesh.triangles = indices; // 삼각형 설정
        mesh.RecalculateBounds(); // 메쉬 경계를 다시 계산하여 정확한 충돌체 생성
        mesh.RecalculateNormals(); // 법선을 다시 계산하여 렌더링 품질 향상
    }
}
