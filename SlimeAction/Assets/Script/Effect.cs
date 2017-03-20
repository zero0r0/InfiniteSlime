using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {

    /// <summary>
    /// アニメーションイベントからの呼び出し
    /// </summary>
    public void DestroyEffect() {
        Destroy(this.gameObject);
    }

}
