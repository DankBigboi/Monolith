// SPDX-FileCopyrightText: 2020 DTanxxx
// SPDX-FileCopyrightText: 2020 Git-Nivrak
// SPDX-FileCopyrightText: 2020 Swept
// SPDX-FileCopyrightText: 2020 Tyler Young
// SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto
// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 Metal Gear Sloth
// SPDX-FileCopyrightText: 2021 Paul Ritter
// SPDX-FileCopyrightText: 2021 ShadowCommander
// SPDX-FileCopyrightText: 2021 Tarlan2
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2021 ike709
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2022 metalgearsloth
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Leon Friedrich
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2024 Whatstone
// SPDX-FileCopyrightText: 2025 starch
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Stacks
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class StackComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("stackType", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<StackPrototype>))]
        public string StackTypeId { get; private set; } = default!;

        /// <summary>
        ///     Current stack count.
        ///     Do NOT set this directly, use the <see cref="SharedStackSystem.SetCount"/> method instead.
        /// </summary>
        [DataField("count")]
        public int Count { get; set; } = 50; // Mono

        /// <summary>
        ///     Max amount of things that can be in the stack.
        ///     Overrides the max defined on the stack prototype.
        /// </summary>
        [ViewVariables(VVAccess.ReadOnly)]
        [DataField("maxCountOverride")]
        public int? MaxCountOverride  { get; set; }

        /// <summary>
        ///     Set to true to not reduce the count when used.
        ///     Note that <see cref="Count"/> still limits the amount that can be used at any one time.
        /// </summary>
        [DataField("unlimited")]
        [ViewVariables(VVAccess.ReadOnly)]
        public bool Unlimited { get; set; }

        /// <summary>
        /// Lingering stacks will remain present even when there are no items.
        /// Instead, they will become transparent.
        /// </summary>
        [DataField("lingering"), ViewVariables(VVAccess.ReadWrite)]
        public bool Lingering;

        [DataField("throwIndividually"), ViewVariables(VVAccess.ReadWrite)]
        public bool ThrowIndividually { get; set; } = false;

        [ViewVariables]
        public bool UiUpdateNeeded { get; set; }

        /// <summary>
        /// Default IconLayer stack.
        /// </summary>
        [DataField("baseLayer")]
        [ViewVariables(VVAccess.ReadWrite)]
        public string BaseLayer = "";

        /// <summary>
        /// Determines if the visualizer uses composite or non-composite layers for icons. Defaults to false.
        ///
        /// <list type="bullet">
        /// <item>
        /// <description>false: they are opaque and mutually exclusive (e.g. sprites in a cable coil). <b>Default value</b></description>
        /// </item>
        /// <item>
        /// <description>true: they are transparent and thus layered one over another in ascending order first</description>
        /// </item>
        /// </list>
        ///
        /// </summary>
        [DataField("composite")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool IsComposite;

        /// <summary>
        /// Sprite layers used in stack visualizer. Sprites first in layer correspond to lower stack states
        /// e.g. <code>_spriteLayers[0]</code> is lower stack level than <code>_spriteLayers[1]</code>.
        /// </summary>
        [DataField("layerStates")]
        [ViewVariables(VVAccess.ReadWrite)]
        public List<string> LayerStates = new();

        // Frontier: transforming Amount, MaxCount in speso stacks
        /// <summary>
        /// An optional function to adjust the layers used for a stack's appearance.
        /// </summary>
        [DataField]
        public StackLayerFunction LayerFunction = StackLayerFunction.None;
        // End Frontier
    }

    [Serializable, NetSerializable]
    public sealed class StackComponentState : ComponentState
    {
        public int Count { get; }
        public int? MaxCount { get; }

        public bool Lingering;

        public StackComponentState(int count, int? maxCount, bool lingering)
        {
            Count = count;
            MaxCount = maxCount;
            Lingering = lingering;
        }
    }
}
