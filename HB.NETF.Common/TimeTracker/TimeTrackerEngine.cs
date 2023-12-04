using System;
using System.Collections.Generic;

namespace HB.NETF.Common.TimeTracker {
    public static class TimeTrackerEngine {
        private static readonly Dictionary<string, ITimeTracker> trackers = new Dictionary<string, ITimeTracker>();
        public static void AddTracker<T>(ITimeTracker tracker) {
            trackers[typeof(T).FullName] = tracker ?? throw new ArgumentNullException(nameof(tracker));
        }

        public static ITimeTracker GetTracker<T>() {
            if (trackers.TryGetValue(typeof(T).FullName, out ITimeTracker tracker))
                return tracker;

            return defaultTracker;
        }

        public static void Track<T>(string context) where T : class, new() => GetTracker<T>().Track(context);
        public static void StopTrack<T>(string context) where T : class, new() => GetTracker<T>().StopTrack(context);
        public static void ResumeTrack<T>(string context) where T : class, new() => GetTracker<T>().ResumeTrack(context);
        public static void PauseTrack<T>(string context) where T : class, new() => GetTracker<T>().PauseTrack(context);

        private readonly static ITimeTracker defaultTracker = new TimeTracker();
        public static ITimeTracker DefaultTracker => defaultTracker;
    }
}
