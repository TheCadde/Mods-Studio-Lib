﻿using ModsStudioLib.Annotations;

namespace ModsStudioLib.Types {
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Float3 {
        public float Value1 { get; set; }

        public float Value2 { get; set; }

        public float Value3 { get; set; }

        public float Red {
            get { return Value1; }
            set { Value1 = value; }
        }

        public float Green {
            get { return Value2; }
            set { Value1 = value; }
        }

        public float Blue {
            get { return Value3; }
            set { Value1 = value; }
        }

        public float X {
            get { return Value1; }
            set { Value1 = value; }
        }

        public float Y {
            get { return Value2; }
            set { Value1 = value; }
        }

        public float Z {
            get { return Value3; }
            set { Value1 = value; }
        }

        public Float3(float x, float y, float z) {
            Value1 = x;
            Value2 = y;
            Value3 = z;
        }

        public override string ToString() {
            return $"X: {Value1}, Y: {Value2}, Z: {Value3}";
        }
    }
}