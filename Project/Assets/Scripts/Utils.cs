using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class Utils
    {
        public static bool CompareStringList(string comparison, string[] list)
        {
            foreach (var item in list)
            {
                if (comparison.Contains(item))
                    return true;
            }

            return false;
        }

        public static bool CompareStringList(string comparison, List<string> list)
        {
            foreach (var item in list)
            {
                if (comparison.Contains(item))
                    return true;
            }

            return false;
        }

        public static bool CompareParentTransform(Transform comparison, List<Transform> list)
        {
            foreach (var item in list)
            {
                if (comparison.parent == item)
                {
                    return true;
                }
            }

            return false;
        }
        
        public static bool CompareParentTransform(Transform comparison, Transform[] list)
        {
            foreach (var item in list)
            {
                if (comparison.parent == item)
                {
                    return true;
                }
            }

            return false;
        }
    }
}