﻿using RoR2.UI;
using UnityEngine;

namespace Moonstorm.Components
{
    /// <summary>
    /// The controller for an Event's Announcement text
    /// </summary>
    public class EventTextController : MonoBehaviour
    {
        /// <summary>
        /// The state of the event
        /// </summary>
        public enum EventFadeState
        {
            FadeIn,
            Wait,
            FadeOut,
        }

        /// <summary>
        /// UIJuice used for fading
        /// </summary>
        public UIJuice uiJuice;
        /// <summary>
        /// Wether the fade begins as soon as the event gets entered
        /// </summary>
        public bool fadeOnStart;
        /// <summary>
        /// How long the warning lasts
        /// </summary>
        public float warningDuration;

        private bool fading = false;
        private EventFadeState fadeState;
        private float internalStopwatch;
        private float actualWarningDuration;
        private void Start()
        {
            actualWarningDuration = warningDuration / 3;
            uiJuice.transitionDuration = actualWarningDuration;
            fadeState = EventFadeState.FadeIn;
            if (fadeOnStart)
                BeginFade();
        }

        /// <summary>
        /// Begins the EventTextController, only call this is <see cref="fadeOnStart"/> is false
        /// </summary>
        public void BeginFade()
        {
            switch (fadeState)
            {
                case EventFadeState.FadeIn:
                    uiJuice.destroyOnEndOfTransition = false;
                    fading = true;
                    uiJuice.originalAlpha = MSUConfig.maxOpacityForEventMessage;
                    uiJuice.TransitionAlphaFadeIn();
                    break;
                case EventFadeState.Wait:
                    fading = true;
                    break;
                case EventFadeState.FadeOut:
                    uiJuice.destroyOnEndOfTransition = true;
                    fading = true;
                    uiJuice.TransitionAlphaFadeOut();
                    break;
            }
        }

        private void Update()
        {
            if (fading)
            {
                internalStopwatch += Time.unscaledDeltaTime;
                if (internalStopwatch > warningDuration)
                {
                    FadeEnd();
                }
            }
        }

        private void FadeEnd()
        {
            fading = false;
            internalStopwatch = 0;

            if (fadeState == EventFadeState.FadeIn)
            {
                fadeState = EventFadeState.Wait;
                BeginFade();
                return;
            }
            else if (fadeState == EventFadeState.Wait)
            {
                fadeState = EventFadeState.FadeOut;
                BeginFade();
                return;
            }
            Destroy(gameObject);
        }
    }
}