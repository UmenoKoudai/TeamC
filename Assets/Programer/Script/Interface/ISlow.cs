using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlow
{
    /// <summary>�ʏ킩��X���[�ɐ؂�ւ�鎞�Ɏ��s����</summary>
    /// <param name="slowSpeedRate">�X���[���̑��x�̊���</param>
    public void OnSlow(float slowSpeedRate);
    /// <summary>�X���[����ʏ�ɐ؂�ւ�鎞�Ɏ��s����</summary>
    public void OffSlow();
}
