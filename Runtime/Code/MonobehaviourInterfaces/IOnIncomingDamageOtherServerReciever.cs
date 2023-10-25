﻿using RoR2;

namespace Moonstorm
{
    /// <summary>
    /// An IOnIncomingDamageOtherServerReciever can be used to modify the DamageInfo of a victim that the player is attacking.
    /// <para>Effectively allows modifications that  run during the TakeDamage method of HealthComponent</para>
    /// </summary>
    public interface IOnIncomingDamageOtherServerReciever
    {
        /// <summary>
        /// Modify the DamageInfo of the victim
        /// </summary>
        /// <param name="victimHealthComponent">The soon to be victim's HealthComponent</param>
        /// <param name="damageInfo">The DamageInfo to modify</param>
        void OnIncomingDamageOther(HealthComponent victimHealthComponent, DamageInfo damageInfo);
    }
}
