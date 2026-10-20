using Fusion;
using UnityEngine;

public interface ICharacterLogic
{
    void Initialize();
    void Update();
    void Move(NetworkCharacterController ncc, Vector3 inputVector);
    void Destroy();
}
