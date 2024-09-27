using UnityEngine;

namespace Game.Components.GridSystem.Visual
{
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField]private Material _gridMaterial;
        [SerializeField]private MeshRenderer _meshRenderer;
        [SerializeField]private MeshFilter _meshFilter;
        private Mesh _mesh;
        private int _gridSizeX; 
        private int _gridSizeY; 
        private float _cellSize;       
        private float _lineWidth;
        
        public void Initialize(int gridSizeX, int gridSizeY, float cellSize, float lineWidth = .1f)
        {
            _gridSizeX = gridSizeX;
            _gridSizeY = gridSizeY;
            _cellSize = cellSize;
            _lineWidth = lineWidth;
            _mesh = new Mesh();
            _meshFilter.mesh = _mesh;
            _meshRenderer.material = _gridMaterial;
            DrawGridMesh();
        }
        
        void DrawGridMesh()
        {
            int totalLines = (_gridSizeX + 1) + (_gridSizeY + 1);
            Vector3[] vertices = new Vector3[totalLines * 4]; 
            int[] triangles = new int[totalLines * 6];

            int vertIndex = 0;
            int triIndex = 0;
            
            for (int x = 0; x <= _gridSizeX; x++)
            {
                AddQuad(vertices, triangles, ref vertIndex, ref triIndex,
                    new Vector3(x * _cellSize - _lineWidth / 2, 0, 0),
                    new Vector3(x * _cellSize + _lineWidth / 2, _gridSizeY * _cellSize, 0));
            }
            
            for (int y = 0; y <= _gridSizeY; y++)
            {
                AddQuad(vertices, triangles, ref vertIndex, ref triIndex,
                    new Vector3(0, y * _cellSize - _lineWidth / 2, 0),
                    new Vector3(_gridSizeX * _cellSize, y * _cellSize + _lineWidth / 2, 0));
            }
            
            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
        }
        
        void AddQuad(Vector3[] vertices, int[] triangles, ref int vertIndex, ref int triIndex, Vector3 start,
            Vector3 end)
        {
            vertices[vertIndex] = new Vector3(start.x, start.y, 0);
            vertices[vertIndex + 1] = new Vector3(start.x, end.y, 0);
            vertices[vertIndex + 2] = new Vector3(end.x, start.y, 0);
            vertices[vertIndex + 3] = new Vector3(end.x, end.y, 0);
            
            triangles[triIndex] = vertIndex;
            triangles[triIndex + 1] = vertIndex + 1;
            triangles[triIndex + 2] = vertIndex + 2;

            triangles[triIndex + 3] = vertIndex + 1;
            triangles[triIndex + 4] = vertIndex + 3;
            triangles[triIndex + 5] = vertIndex + 2;

            vertIndex += 4;
            triIndex += 6;
        }
    }
}