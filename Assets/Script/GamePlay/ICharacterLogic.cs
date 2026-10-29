using Fusion;
using UnityEngine;

public interface ICharacterLogic
{
    void Initialize(CharacterAnimation anim);
    void NormalAttack(NetworkCharacterController ncc);
    void UniqueSkillA(NetworkCharacterController ncc);
    void UniqueSkillB(NetworkCharacterController ncc);
    void Move(NetworkCharacterController ncc, Vector3 inputVector);
    void Destroy();
}
