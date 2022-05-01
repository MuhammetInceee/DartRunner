using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace MuhammetInce.Helpers
{
    public static class HelperUtils
    {
        public static void RotateAround(GameObject obj, float durationSecond, int rotateSide)
        {
            var eulerAngles = obj.transform.eulerAngles;
            obj.transform.DORotate(new Vector3(eulerAngles.x, eulerAngles.y,  eulerAngles.z + 180 * rotateSide), durationSecond, RotateMode.FastBeyond360);
        }
    }
}