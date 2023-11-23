using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPause
{
    /// <summary>一時停止時に実行</summary>
    public void Pause();
    /// <summary>一時停止から通常に切り替わる時に実行</summary>
    public void Resume();
}
