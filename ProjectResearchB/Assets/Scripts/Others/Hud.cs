using UnityEngine;
using UnityEngine.UI;

// 腕の UI を制御するコンポーネント
public class Hud : MonoBehaviour
{
    [SerializeField]
    private Slider hpBar, mpBar;

    // 毎フレーム呼び出される関数
    private void Update()
    {
        var pStatus = Player.instance.GetStatus();
        hpBar.value = (float)pStatus.Hp / pStatus.MaxHp;
        mpBar.value = (float)pStatus.Mp / pStatus.MaxMp;
    }
}