using System;
using UnityEngine;

namespace STH.Core
{
    /// <summary>
    /// 발사 로직 인터페이스 - 발사 패턴의 규약
    /// </summary>
    public interface IFireStrategy
    {
        /// <summary>
        /// 발사 로직을 실행합니다.
        /// </summary>
        /// <param name="origin">발사 시작점</param>
        /// <param name="spawnCallback">총알 생성 콜백 (위치, 회전)</param>
        void Fire(Transform origin, Action<Vector3, Quaternion> spawnCallback);
    }
}
