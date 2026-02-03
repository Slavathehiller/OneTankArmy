using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MISC
{
    public static class RotateCalculator
    {
        public static float? AngleTolookAt(Transform observer, Transform target)
        {
            float? angleToTurn = null;

            var targetPosition = target.position - observer.position;

            var angle = Vector3.Angle(observer.up, targetPosition);
            var cross = Vector3.Cross(observer.up, targetPosition);
            if (cross.z < 0) // Проверяем знак y координаты векторного произведения
            {
                angle = -angle; // Если знак отрицательный, инвертируем угол
            }
            angleToTurn = angle;

            return angleToTurn;

            //if (_angleToTurn != null)
            //{
            //    var nowTurnAngle = _rotateSpeed * Time.deltaTime;
            //    if (_angleToTurn< 0)
            //        nowTurnAngle = -nowTurnAngle;
            //    transform.Rotate(0, 0, nowTurnAngle);
            //    _angleToTurn -= nowTurnAngle;
            //    if (Math.Abs(_angleToTurn.Value) <= Math.Abs(nowTurnAngle))
            //        _angleToTurn = null;
            //}
        }
    } 
}
