using System;

/// <summary>
/// 敵の状態を管理するインターフェース
/// </summary>
public interface IEnemyState
{
    /// <summary>
    /// 状態に入った時に一度だけ呼ばれる
    /// </summary>
    /// <param name="enemyController">敵の処理</param>
    void OnEnter(EnemyController enemyController);

    /// <summary>
    /// この状態である間呼ばれる
    /// </summary>
    void OnUpdate();

    /// <summary>
    /// 状態から出るときに一度だけ呼ばれる
    /// </summary>
    void OnExit();
}
