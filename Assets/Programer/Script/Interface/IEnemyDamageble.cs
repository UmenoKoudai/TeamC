using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�Ƀ_���[�W��^����C���^�[�t�F�C�X
/// </summary>
public interface IEnemyDamageble
{
    /// <summary>�U����������</summary>
    /// <param name="attackType"></param>
    /// <param name="attackHitTyp"></param>
    void Damage(AttackType attackType, MagickType attackHitTyp, float damage);
}


