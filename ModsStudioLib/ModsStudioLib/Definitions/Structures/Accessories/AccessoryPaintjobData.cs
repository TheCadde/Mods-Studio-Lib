using System.Diagnostics.CodeAnalysis;

using ModsStudioLib.Annotations;
using ModsStudioLib.Definitions.Attributes;
using ModsStudioLib.Definitions.DefinitionTypes;

namespace ModsStudioLib.Definitions.Structures.Accessories {
    [DefinitionStructure("accessory_paint_job_data", "A unit describing a paintjob that can be purchased for a certain truck.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
    public class AccessoryPaintjobData : DefinitionStructure {
        [DefinitionStructureValue(
            variableName: "name",
            variableGroup: "Shop details",
            defaultValue: "New Paintjob",
            defaultCommentValue: "Some name",
            comment: "The name of the paintjob.")]
        public string Name { get; set; }

        [DefinitionStructureValue(
            variableName: "price",
            variableGroup: "Shop details",
            defaultValue: 10000,
            defaultCommentValue: "~10000 to ~30000",
            comment: "The price of the paintjob.")]
        public long Price { get; set; }

        [DefinitionStructureValue(
            variableName: "unlock",
            variableGroup: "Shop details",
            defaultValue: 0,
            defaultCommentValue: "0 to ~25",
            comment: "The driver level required to purchase this paintjob. A level of 0 (zero) means anyone can purchase it.")]
        public uint Unlock { get; set; }

        [DefinitionStructureValue(
            variableName: "icon",
            variableGroup: "Shop details",
            defaultValue: "",
            defaultCommentValue: "Icon material",
            comment: "The icon to be shown in the paint shop. This is the name of the material (.mat) file present in the /material/ui/accessory folder.")]
        public string Icon { get; set; }

        [DefinitionStructureValue(
            variableName: "base_color",
            variableGroup: "Colors",
            defaultValue: "(0, 0, 0)",
            defaultCommentValue: "(0.0, 0.0, 0.0)",
            comment: "The base color in three floating point components. Red, Green and Blue respetively. Each component should have a value between 0 (zero) and 1 (one) where 0.5 (a half) represents roughly half color tone. Note that these are in SCS color space, not RGB (linear) color space.")]
        public DefinitionFloat3 BaseColor { get; set; }

        [DefinitionStructureValue(
            variableName: "stock",
            variableGroup: "Shop details",
            defaultValue: false,
            defaultCommentValue: "false",
            comment: "True if the paintjob is available in the truck dealership. False otherwise, which means it's only available in the repair shop / paint shop.")]
        public bool Stock { get; set; }

        [DefinitionStructureValue(
            variableName: "flipflake",
            variableGroup: "Skin type",
            defaultValue: false,
            defaultCommentValue: "false",
            comment: "True if the paintjob is metallic. False otherwise.")]
        public bool FlipFlake { get; set; }

        [DefinitionStructureValue(
            variableName: "flip_color",
            variableGroup: "Colors",
            defaultValue: "(0, 0, 0)",
            defaultCommentValue: "(0.0, 0.0, 0.0)",
            comment: "The flip color. As you turn the camera, the base color changes to this hue in certain areas. See base_color for more details.")]
        public DefinitionFloat3 FlipColor { get; set; }

        [DefinitionStructureValue(
            variableName: "flake_color",
            variableGroup: "Colors",
            defaultValue: "(0, 0, 0)",
            defaultCommentValue: "(0.0, 0.0, 0.0)",
            comment: "The flake color. As you turn the camera, the flake / detail color changes to this hue in certain areas. See base_color for more details.")]
        public DefinitionFloat3 FlakeColor { get; set; }

        [DefinitionStructureValue(
            variableName: "flip_strength",
            variableGroup: "Shading",
            defaultValue: 1.3f,
            defaultCommentValue: "1.3",
            comment: "The strength of the color changing effect for the flip color. see flip_color for more details.")]
        public float FlipStrength { get; set; }

        [DefinitionStructureValue(
            variableName: "flake_uvscale",
            variableGroup: "Shading",
            defaultValue: 10.0f,
            defaultCommentValue: "10.0",
            comment: "The size multiplier of the flake noise texture. A value of 1 (one) makes the noise texture map cover the whole truck once. A value of 2 (two) makes two tiled instances of the flake texture appear on the truck.")]
        public float FlakeUVScale { get; set; }

        [DefinitionStructureValue(
            variableName: "flake_density",
            variableGroup: "Shading",
            defaultValue: 1.0f,
            defaultCommentValue: "1.0",
            comment: null)]
        public float FlakeDensity { get; set; }

        [DefinitionStructureValue(
            variableName: "flake_shininess",
            variableGroup: "Shading",
            defaultValue: 20.0,
            defaultCommentValue: "20.0",
            comment: "How shiny (specular) the flake texture is. When light (such as the sun) hits the brighter areas of the texture, they will appear to be glowing. Higher values means more glow.")]
        public float FlakeShininess { get; set; }

        [DefinitionStructureValue(
            variableName: "flake_clearcoat_rolloff",
            variableGroup: "Shading",
            defaultValue: 4.5,
            defaultCommentValue: "4.5",
            comment: null)]
        public float FlakeClearcoatRolloff { get; set; }

        [DefinitionStructureValue(
            variableName: "flake_noise",
            variableGroup: "Textures",
            defaultValue: null,
            defaultCommentValue: "A texture file",
            comment: "Path to where the flake noise texture can be found. This is a path to a Texture Object file. (.tobj)")]
        public string FlakeNoise { get; set; }

        [DefinitionStructureValue(
            variableName: "base_color_locked",
            variableGroup: "Colors",
            defaultValue: false,
            defaultCommentValue: "false",
            comment: "When false, allows the user (player) to change the color in the custom color picking dialog in game. When true, the color is locked to what you set it to in here.")]
        public bool BaseColorLocked { get; set; }

        [DefinitionStructureValue(
            variableName: "flip_color_locked",
            variableGroup: "Colors",
            defaultValue: false,
            defaultCommentValue: "false",
            comment: "When false, allows the user (player) to change the color in the custom color picking dialog in game. When true, the color is locked to what you set it to in here.")]
        public bool FlipColorLocked { get; set; }

        [DefinitionStructureValue(
            variableName: "flake_color_locked",
            variableGroup: "Colors",
            defaultValue: false,
            defaultCommentValue: "false",
            comment: "When false, allows the user (player) to change the color in the custom color picking dialog in game. When true, the color is locked to what you set it to in here.")]
        public bool FlakeColorLocked { get; set; }

        [DefinitionStructureValue(
            variableName: "alternate_uvset",
            variableGroup: "Mapping",
            defaultValue: false,
            defaultCommentValue: "false",
            comment: "When true, the alternate UV map is used. When false, the standard UV map is used.")]
        public bool AlternateUVSet { get; set; }
    }
}
