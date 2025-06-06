﻿using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	/// <summary>
	/// This feedback can act as a pause but also as a start point for your loops. Add a FeedbackLooper below this (and after a few feedbacks) and your MMF_Player will loop between both
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback can act as a pause but also as a start point for your loops. Add a FeedbackLooper below this (and after a few feedbacks) and your MMF_Player will loop between both.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks")]
	[FeedbackPath("Loop/Looper Start")]
	public class MMF_LooperStart : MMF_Pause
	{
		/// sets the color of this feedback in the inspector
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return MMFeedbacksInspectorColors.LooperStartColor; } }
		public override Color DisplayColor { get { return MMFeedbacksInspectorColors.LooperStartColor.MMDarken(0.35f); } }
		#endif
		public override bool LooperStart { get { return true; } }

		/// the duration of this feedback is the duration of the pause
		public override float FeedbackDuration { get { return ApplyTimeMultiplier(PauseDuration); } set { PauseDuration = value; } }

		/// <summary>
		/// Overrides the default value
		/// </summary>
		protected virtual void Reset()
		{
			PauseDuration = 0;
		}

		/// <summary>
		/// On play we run our pause
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (Active)
			{
				ProcessNewPauseDuration();
				Owner.StartCoroutine(PlayPause());
			}
		}

	}
}