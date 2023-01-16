using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SeasonalTimeManager : MonoBehaviour {

	public static SeasonalTimeManager Instance;

	public delegate void OnSeasonalYearly();
	public event OnSeasonalYearly SeasonalYearly;

	public delegate void OnSeasonalMonthly();
	public event OnSeasonalMonthly SeasonalMonthly;

	public delegate void OnSeasonalWeekly();
	public event OnSeasonalWeekly SeasonalWeekly;

	public delegate void OnSeasonalDaily();
	public event OnSeasonalDaily SeasonalDaily;

	public delegate void OnSeasonalHourly();
	public event OnSeasonalHourly SeasonalHourly;

	public delegate void OnSeasonalMinutely();
	public event OnSeasonalMinutely SeasonalMinutely;

	private DateTime previousTime = DateTime.UtcNow;

	// Start is called before the first frame update
	void Start() {
		Instance = this;
		previousTime = DateTime.UtcNow;
	}

	// Update is called once per frame
	void Update() {
		UpdateSeasonalTimers();
		previousTime = DateTime.UtcNow;
	}


	private void UpdateSeasonalTimers() {
		if (DidSeasonResetFromLastDateTime(previousTime, kSeasonTimerType.Yearly)) {
			SeasonalYearly?.Invoke();
		}

		if (DidSeasonResetFromLastDateTime(previousTime, kSeasonTimerType.Monthly)) {
			SeasonalMonthly?.Invoke();
		}

		if (DidSeasonResetFromLastDateTime(previousTime, kSeasonTimerType.Weekly)) {
			SeasonalWeekly?.Invoke();
		}

		if (DidSeasonResetFromLastDateTime(previousTime, kSeasonTimerType.Daily)) {
			SeasonalDaily?.Invoke();
		}

		if (DidSeasonResetFromLastDateTime(previousTime, kSeasonTimerType.Hourly)) {
			SeasonalHourly?.Invoke();
		}

		if (DidSeasonResetFromLastDateTime(previousTime, kSeasonTimerType.Minutely)) {
			// Debug.LogWarning("Invoke Seasonal: " + kSeasonTimerType.Minutely);
			SeasonalMinutely?.Invoke();
		}
	}

	public bool DidSeasonResetFromLastDateTime(DateTime dateTime, kSeasonTimerType seasonTimerType) {
		if (seasonTimerType == kSeasonTimerType.Permanent) return false;

		DateTime curTime = DateTime.UtcNow;

		if (curTime.Year != dateTime.Year) { return true; }
		if (seasonTimerType == kSeasonTimerType.Yearly) return false;

		if (curTime.Month != dateTime.Month) { return true; }
		if (seasonTimerType == kSeasonTimerType.Monthly) return false;

		if (curTime.DayOfWeek < dateTime.DayOfWeek || curTime.Day - dateTime.Day >= 7) { return true; }
		if (seasonTimerType == kSeasonTimerType.Weekly) return false;

		if (curTime.Day != dateTime.Day) { return true; }
		if (seasonTimerType == kSeasonTimerType.Daily) return false;

		if (curTime.Hour != dateTime.Hour) { return true; }
		if (seasonTimerType == kSeasonTimerType.Hourly) return false;

		if (curTime.Minute != dateTime.Minute) { return true; }
		if (seasonTimerType == kSeasonTimerType.Minutely) return false;

		return false;
	}
}
