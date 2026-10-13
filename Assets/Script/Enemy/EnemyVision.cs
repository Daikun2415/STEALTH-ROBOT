using UnityEngine;

/// <summary>
/// 敵の視野を表示する処理
/// </summary>
public class EnemyVision : MonoBehaviour
{
    /// <summary>
    /// 視界距離
    /// </summary>
    private float sightDistance;

    /// <summary>
    /// 視野角
    /// </summary>
    private float sightAngle;

    /// <summary>
    /// 扇形の三角形分割数
    /// </summary>
    [SerializeField]
    private int segmentCount = 20;

    /// <summary>
    /// メッシュを保持
    /// </summary>
    [SerializeField]
    private MeshFilter meshFilter;

    /// <summary>
    /// 敵のステータスを設定する
    /// </summary>
    /// <param name="enemyData">敵のデータ</param>
    public void SetEnemyData(EnemyData enemyData)
    {
        this.sightDistance = enemyData.SightDistance;
        this.sightAngle = enemyData.SightAngle;

        this.CreateVisionMesh();
    }

    /// <summary>
    /// 視野角の扇形を生成
    /// </summary>
    private void CreateVisionMesh()
    {
        // 頂点
        Vector3[] vertices = new Vector3[this.segmentCount + 2];

        // 頂点番号
        int[] triangles = new int[this.segmentCount * 3];

        // 中心点
        vertices[0] = Vector3.zero;

        // 開始角度
        float startAngle = -this.sightAngle * 0.5f;

        // 1個当たりの角度
        float angleStep = this.sightAngle / this.segmentCount;

        // 外周の頂点を生成
        for (int i = 0; i <= this.segmentCount; i++)
        {
            // 角度を計算してラジアンに変換
            float angle = startAngle + (angleStep * i);
            float radian = angle * Mathf.Deg2Rad;

            // 円周上の座標を計算
            vertices[i + 1] = new Vector3(Mathf.Cos(radian) * this.sightDistance, Mathf.Sin(radian) * this.sightDistance, 0.0f);
        }

        // 三角形を作る
        for (int i = 0; i < this.segmentCount; i++)
        {
            int index = i * 3;

            triangles[index] = 0;
            triangles[index + 1] = i + 2;
            triangles[index + 2] = i + 1;
        }

        // メッシュ生成
        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // 法線、大きさ、範囲を計算
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // フィルターに設定
        this.meshFilter.mesh = mesh;
    }

    /// <summary>
    /// 視野角の表示状態を設定する
    /// </summary>
    /// <param name="isvisible">表示する場合はtrue、違えばfalse</param>
    public void SetVisionVisible(bool isvisible)
    {
        this.meshFilter.gameObject.SetActive(isvisible);
    }
}