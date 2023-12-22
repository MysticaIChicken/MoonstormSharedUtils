﻿using R2API.AddressReferencedAssets;
using RoR2;
using System;
using System.Linq;
using UnityEngine;
using static RoR2.CombatDirector;

namespace Moonstorm
{
    /// <summary>
    /// Represents a Serialized version of a <see cref="CombatDirector.EliteTierDef"/>
    /// <para>Utilized by the <see cref="EliteTierDefBase"/> and the <see cref="EliteTierDefModuleBase"/></para>
    /// </summary>
    public class SerializableEliteTierDef : ScriptableObject
    {
        [Tooltip("This multiplier is applied to the cost of the CharacterSpawnCard when the CombatDirector tries to spawn an elite from this tier")]
        public float costMultiplier;
        public AddressReferencedEliteDef[] elites = Array.Empty<AddressReferencedEliteDef>();
        [Tooltip("Wether the combat director can select this tier if no eliteTypes are supplied")]
        public bool canSelectWithoutAvailableEliteDef;

        /// <summary>
        /// The created EliteTierDef from the serialized data of a <see cref="SerializableEliteTierDef"/> and it's tied <see cref="EliteTierDefBase"/>
        /// </summary>
        public EliteTierDef EliteTierDef { get; private set; }
        
        internal void Create()
        {
            EliteTierDef = new EliteTierDef
            {
                eliteTypes = elites.Select(x => x.Asset).ToArray(),
                costMultiplier = costMultiplier,
                canSelectWithoutAvailableEliteDef = canSelectWithoutAvailableEliteDef,
                isAvailable = (rules) => true,
            };
        }
    }
}
