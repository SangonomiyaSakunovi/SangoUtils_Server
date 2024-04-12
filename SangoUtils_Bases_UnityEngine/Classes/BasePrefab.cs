﻿using SangoUtils.Bases_Unity.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace SangoUtils.Bases_Unity
{
    public abstract class BasePrefab : MonoBehaviour
    {
        public List<Component> GetRelevantComponent()
        {
            return ComponentReflectionUtils.GetRelevantComponent(this);
        }
    }
}
