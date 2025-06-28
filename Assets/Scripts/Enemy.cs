using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    /// <summary>  
    /// プレイヤー  
    /// </summary>  
    [SerializeField] private Player player_ = null;

    /// <summary>  
    /// ワールド行列   
    /// </summary>  
    private Matrix4x4 worldMatrix_ = Matrix4x4.identity;

	/// <summary>
	/// 敵の視野角（±20° = 合計40°）
	/// </summary>
	[SerializeField] private float viewAngle_ = 20f;

	/// <summary>
	/// 移動速度（1フレーム0.2）
	/// </summary>
	[SerializeField] private float moveSpeed_ = 0.2f;

	/// <summary>
	/// 旋回速度（1フレーム最大10°）
	/// </summary>
	[SerializeField] private float rotationSpeed_ = 10f;

	/// <summary>  
	/// ターゲットとして設定する  
	/// </summary>  
	/// <param name="enable">true:設定する / false:解除する</param>  
	public void SetTarget(bool enable)
    {
        // マテリアルの色を変更する  
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.materials[0].color = enable ? Color.red : Color.white;
    }

	/// <summary>
	/// 開始処理
	/// </summary>
	public void Start()
    {
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    public void Update()
    {
		if (player_ == null) return;

		// プレイヤーのワールド座標を取得
		Vector3 playerPos = player_.worldMatrix.GetColumn(3);
		Vector3 myPos = transform.position;

		// プレイヤーまでの方向（XZ平面）
		Vector3 toPlayer = playerPos - myPos;
		toPlayer.y = 0;

		Vector3 forward = transform.forward;
		forward.y = 0;

		// 視野角チェック（±viewAngle_）
		float angle = Vector3.Angle(forward, toPlayer);
		if (angle <= viewAngle_)
		{
			// プレイヤーの方向にY軸だけで最大10度まで回転
			Quaternion targetRot = Quaternion.LookRotation(toPlayer);
			transform.rotation = Quaternion.RotateTowards(
				transform.rotation,
				targetRot,
				rotationSpeed_
			);

			// 現在の forward 方向に 0.2 移動
			transform.position += transform.forward * moveSpeed_;
		}
	}
}
