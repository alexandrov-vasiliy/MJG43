﻿using System;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MoreMountains.Feedbacks
{
	/// <summary>
	/// A helper class to copy and paste feedback properties
	/// </summary>
	static class MMF_PlayerCopy
	{
		static public System.Type Type { get; private set; }
		public static readonly List<MMF_Feedback> CopiedFeedbacks = new List<MMF_Feedback>();
		public static readonly Dictionary<MMF_Player, List<MMF_Feedback>> RuntimeChanges = new Dictionary<MMF_Player, List<MMF_Feedback>>();

		static string[] IgnoreList = new string[]
		{
			"m_ObjectHideFlags",
			"m_CorrespondingSourceObject",
			"m_PrefabInstance",
			"m_PrefabAsset",
			"m_GameObject",
			"m_Enabled",
			"m_EditorHideFlags",
			"m_Script",
			"m_Name",
			"m_EditorClassIdentifier"
		};
		
		static MMF_PlayerCopy()
		{
			EditorApplication.playModeStateChanged += ModeChanged;
		}

		private static void ModeChanged(PlayModeStateChange playModeState)
		{
			switch (playModeState)
			{
				case PlayModeStateChange.ExitingPlayMode:
					StoreRuntimeChanges();
					break;
        
				case PlayModeStateChange.EnteredEditMode:
					ApplyRuntimeChanges();
					break;
			}
		}

		private static void StoreRuntimeChanges()
		{
			foreach (MMF_Player player in Object.FindObjectsByType<MMF_Player>(FindObjectsInactive.Include, FindObjectsSortMode.None).Where(p => p.KeepPlayModeChanges))
			{
				MMF_PlayerCopy.StoreRuntimeChanges(player);
			}
		}

		private static void ApplyRuntimeChanges()
		{
			foreach (MMF_Player player in Object.FindObjectsByType<MMF_Player>(FindObjectsInactive.Include, FindObjectsSortMode.None).Where(MMF_PlayerCopy.RuntimeChanges.ContainsKey))
			{
				MMF_PlayerCopy.ApplyRuntimeChanges(player);
			}
		}

		static public bool HasCopy()
		{
			return CopiedFeedbacks != null && CopiedFeedbacks.Count == 1;
		}

		static public bool HasMultipleCopies()
		{
			return CopiedFeedbacks != null && CopiedFeedbacks.Count > 1;
		}

		static public void Copy(MMF_Feedback feedback)
		{
			Type feedbackType = feedback.GetType();
			MMF_Feedback newFeedback = (MMF_Feedback)Activator.CreateInstance(feedbackType);
			EditorUtility.CopySerializedManagedFieldsOnly(feedback, newFeedback);
			CopiedFeedbacks.Clear();
			CopiedFeedbacks.Add(newFeedback);
		}
        
		static public void CopyAll(MMF_Player sourceFeedbacks)
		{
			CopiedFeedbacks.Clear();
			foreach (MMF_Feedback feedback in sourceFeedbacks.FeedbacksList)
			{
				Type feedbackType = feedback.GetType();
				MMF_Feedback newFeedback = (MMF_Feedback)Activator.CreateInstance(feedbackType);
				EditorUtility.CopySerializedManagedFieldsOnly(feedback, newFeedback);
				CopiedFeedbacks.Add(newFeedback);    
			}
		}

		// Multiple Copy ----------------------------------------------------------

		static public void PasteAll(MMF_PlayerEditorUITK targetEditor)
		{
			foreach (MMF_Feedback feedback in MMF_PlayerCopy.CopiedFeedbacks)
			{
				targetEditor.TargetMmfPlayer.AddFeedback(feedback);
			}
			CopiedFeedbacks.Clear();
		}
		
		// Runtime Changes

		static public void StoreRuntimeChanges(MMF_Player player)
		{
			RuntimeChanges[player] = new List<MMF_Feedback>();
			foreach (MMF_Feedback feedback in player.FeedbacksList)
			{
				Type feedbackType = feedback.GetType();
				MMF_Feedback newFeedback = (MMF_Feedback)Activator.CreateInstance(feedbackType);
				EditorUtility.CopySerializedManagedFieldsOnly(feedback, newFeedback);
				RuntimeChanges[player].Add(newFeedback);    
			}
		}

		static public void ApplyRuntimeChanges(MMF_Player player)
		{
			SerializedObject playerSerialized = new SerializedObject(player);
			playerSerialized.Update();
			Undo.RecordObject(player, "Replace all feedbacks");
			player.FeedbacksList.Clear();
			foreach (MMF_Feedback feedback in MMF_PlayerCopy.RuntimeChanges[player])
			{
				player.AddFeedback(feedback, true);
			}
			playerSerialized.ApplyModifiedProperties();
			PrefabUtility.RecordPrefabInstancePropertyModifications(player);
			if (MMF_PlayerConfiguration.Instance.AutoDisableKeepPlaymodeChanges)
			{
				playerSerialized.Update();
				player.KeepPlayModeChanges = false;    
				playerSerialized.ApplyModifiedProperties();
			}
			player.RefreshCache();
		}
	}
}