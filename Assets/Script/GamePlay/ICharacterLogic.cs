using Fusion;
using UnityEngine;

public interface ICharacterLogic
{
    void Initialize(CharacterAnimation anim);
    void UniqueSkillA();
    void UniqueSkillB();
    void Move(NetworkCharacterController ncc, Vector3 inputVector);
    void Destroy();
}
