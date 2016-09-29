using ModsStudioLib.Annotations;
using ModsStudioLib.Definitions.Attributes;
using ModsStudioLib.Definitions.Parsing;
using ModsStudioLib.Types;

namespace ModsStudioLib.Definitions.Structures.Vehicle.Truck {
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    class AccessoryPaintjobData : DefinitionStructure {
        [DefinitionValue("name", DefinitionValueTypes.String, "Some name", "The name of the paintjob.")]
        public string Name { get; set; }

        [DefinitionValue("price", DefinitionValueTypes.S64, "~10000 to ~30000", "The price of the paintjob.")]
        public long Price { get; set; }

        [DefinitionValue("unlock", DefinitionValueTypes.UInt, "0 to ~25", "The driver level required to purchase this paintjob. A level of 0 (zero) means anyone can purchase it.")]
        public uint Unlock { get; set; }

        [DefinitionValue("icon", DefinitionValueTypes.String, "An icon from the /material/ui/accessory folder", "The icon to be shown in the paint shop. This is the name of the material (.mat) file present in the /material/ui/accessory folder.")]
        public string Icon { get; set; }

        [DefinitionValue("base_color", DefinitionValueTypes.Float3, "(0.0, 0.0, 0.0)", "The base color in three floating point components. Red, Green and Blue respetively. Each component should have a value between 0 (zero) and 1 (one) where 0.5 (a half) represents roughly half color tone. Note that these are in SCS color space, not RGB (linear) color space.")]
        public Float3 BaseColor { get; set; }

        [DefinitionValue("stock", DefinitionValueTypes.Bool, "false", "True if the paintjob is available in the truck dealership. False otherwise, which means it's only available in the repair shop / paint shop.")]
        public bool Stock { get; set; }

        [DefinitionValue("flipflake", DefinitionValueTypes.Bool, "false", "True if the paintjob is metallic. False otherwise.")]
        public bool FlipFlake { get; set; }

        [DefinitionValue("flip_color", DefinitionValueTypes.Float3, "(0.0, 0.0, 0.0)", "The flip color. As you turn the camera, the base color changes to this hue in certain areas. See base_color for more details.")]
        public Float3 FlipColor { get; set; }

        [DefinitionValue("flake_color", DefinitionValueTypes.Float3, "(0.0, 0.0, 0.0)", "The flake color. As you turn the camera, the flake / detail color changes to this hue in certain areas. See base_color for more details.")]
        public Float3 FlakeColor { get; set; }

        [DefinitionValue("flip_strength", DefinitionValueTypes.Float, "1.3", "The strength of the color changing effect for the flip color. see flip_color for more details.")]
        public float FlipStrength { get; set; }

        [DefinitionValue("flake_uvscale", DefinitionValueTypes.Float, "10.0", "The size multiplier of the flake noise texture. A value of 1 (one) makes the noise texture map cover the whole truck once. A value of 2 (two) makes two tiled instances of the flake texture appear on the truck.")]
        public float FlakeUVScale { get; set; }

        [DefinitionValue("flake_density", DefinitionValueTypes.Float, "1.0", "??? UNKNOWN ???")]
        public float FlakeDensity { get; set; }

        [DefinitionValue("flake_shininess", DefinitionValueTypes.Float, "20.0", "How shiny (specular) the flake texture is. When light (such as the sun) hits the brighter areas of the texture, they will appear to be glowing. Higher values means more glow.")]
        public float FlakeShininess { get; set; }

        [DefinitionValue("flake_clearcoat_rolloff", DefinitionValueTypes.Float, "4.5", "??? UNKNOWN ???")]
        public float FlakeClearcoatRolloff { get; set; }

        [DefinitionValue("flake_noise", DefinitionValueTypes.String, "A texture file", "Path to where the flake noise texture can be found. This is a path to a Texture Object file. (.tobj)")]
        public string FlakeNoise { get; set; }

        [DefinitionValue("base_color_locked", DefinitionValueTypes.Bool, "false", "When false, allows the user (player) to change the color in the custom color picking dialog in game. When true, the color is locked to what you set it to in here.")]
        public bool BaseColorLocked { get; set; }

        [DefinitionValue("flip_color_locked", DefinitionValueTypes.Bool, "false", "When false, allows the user (player) to change the color in the custom color picking dialog in game. When true, the color is locked to what you set it to in here.")]
        public bool FlipColorLocked { get; set; }

        [DefinitionValue("flake_color_locked", DefinitionValueTypes.Bool, "false", "When false, allows the user (player) to change the color in the custom color picking dialog in game. When true, the color is locked to what you set it to in here.")]
        public bool FlakeColorLocked { get; set; }

        [DefinitionValue("alternate_uvset", DefinitionValueTypes.Bool, "false", "When true, the alternate UV map is used. When false, the standard UV map is used.")]
        public bool AlternateUVSet { get; set; }
    }
}
