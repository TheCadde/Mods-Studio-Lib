using ModsStudioLib.Annotations;
using ModsStudioLib.Definitions.Attributes;
using ModsStudioLib.Types;

namespace ModsStudioLib.Definitions.Structures.Accessories {
    [DefinitionStructure("accessory_paint_job_data", "A unit describing a paintjob that can be purchased for a certain truck.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AccessoryPaintjobData : DefinitionStructure {
        [DefinitionValue("name", "Some name", "The name of the paintjob.")]
        public string Name { get; set; }

        [DefinitionValue("price", "~10000 to ~30000", "The price of the paintjob.")]
        public long Price { get; set; }

        [DefinitionValue("unlock", "0 to ~25", "The driver level required to purchase this paintjob. A level of 0 (zero) means anyone can purchase it.")]
        public uint Unlock { get; set; }

        [DefinitionValue("icon", "An icon from the /material/ui/accessory folder", "The icon to be shown in the paint shop. This is the name of the material (.mat) file present in the /material/ui/accessory folder.")]
        public string Icon { get; set; }

        [DefinitionValue("base_color",  "(0.0, 0.0, 0.0)", "The base color in three floating point components. Red, Green and Blue respetively. Each component should have a value between 0 (zero) and 1 (one) where 0.5 (a half) represents roughly half color tone. Note that these are in SCS color space, not RGB (linear) color space.")]
        public Float3 BaseColor { get; set; }

        [DefinitionValue("stock", "false", "True if the paintjob is available in the truck dealership. False otherwise, which means it's only available in the repair shop / paint shop.")]
        public bool Stock { get; set; }

        [DefinitionValue("flipflake", "false", "True if the paintjob is metallic. False otherwise.")]
        public bool FlipFlake { get; set; }

        [DefinitionValue("flip_color", "(0.0, 0.0, 0.0)", "The flip color. As you turn the camera, the base color changes to this hue in certain areas. See base_color for more details.")]
        public Float3 FlipColor { get; set; }

        [DefinitionValue("flake_color", "(0.0, 0.0, 0.0)", "The flake color. As you turn the camera, the flake / detail color changes to this hue in certain areas. See base_color for more details.")]
        public Float3 FlakeColor { get; set; }

        [DefinitionValue("flip_strength", "1.3", "The strength of the color changing effect for the flip color. see flip_color for more details.")]
        public float FlipStrength { get; set; }

        [DefinitionValue("flake_uvscale", "10.0", "The size multiplier of the flake noise texture. A value of 1 (one) makes the noise texture map cover the whole truck once. A value of 2 (two) makes two tiled instances of the flake texture appear on the truck.")]
        public float FlakeUVScale { get; set; }

        [DefinitionValue("flake_density", "1.0", "??? UNKNOWN ???")]
        public float FlakeDensity { get; set; }

        [DefinitionValue("flake_shininess", "20.0", "How shiny (specular) the flake texture is. When light (such as the sun) hits the brighter areas of the texture, they will appear to be glowing. Higher values means more glow.")]
        public float FlakeShininess { get; set; }

        [DefinitionValue("flake_clearcoat_rolloff", "4.5", "??? UNKNOWN ???")]
        public float FlakeClearcoatRolloff { get; set; }

        [DefinitionValue("flake_noise", "A texture file", "Path to where the flake noise texture can be found. This is a path to a Texture Object file. (.tobj)")]
        public string FlakeNoise { get; set; }

        [DefinitionValue("base_color_locked", "false", "When false, allows the user (player) to change the color in the custom color picking dialog in game. When true, the color is locked to what you set it to in here.")]
        public bool BaseColorLocked { get; set; }

        [DefinitionValue("flip_color_locked", "false", "When false, allows the user (player) to change the color in the custom color picking dialog in game. When true, the color is locked to what you set it to in here.")]
        public bool FlipColorLocked { get; set; }

        [DefinitionValue("flake_color_locked", "false", "When false, allows the user (player) to change the color in the custom color picking dialog in game. When true, the color is locked to what you set it to in here.")]
        public bool FlakeColorLocked { get; set; }

        [DefinitionValue("alternate_uvset", "false", "When true, the alternate UV map is used. When false, the standard UV map is used.")]
        public bool AlternateUVSet { get; set; }
    }
}
