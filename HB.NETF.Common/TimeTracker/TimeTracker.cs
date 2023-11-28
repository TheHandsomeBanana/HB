using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Common.TimeTracker {
    public class TimeTracker : ITimeTracker {
        private readonly Dictionary<string, TrackedTime> trackedTimes = new Dictionary<string, TrackedTime>();
        public Dictionary<string, TimeSpan[]> TrackedTimes => trackedTimes.ToDictionary(e => e.Key, e => e.Value.Trackings.ToArray());
        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, TimeSpan[]> t in TrackedTimes) {
                sb.AppendLine($"{t.Key}: {string.Join(", ", t.Value.Select(e => e.TotalMilliseconds))} [ms]");
            }

            return sb.ToString();
        }

        public void Track(string context) {
            TrackedTime t;
            if (trackedTimes.ContainsKey(context))
                t = trackedTimes[context];
            else
                t = new TrackedTime();

            t.Start();
            trackedTimes[context] = t;
        }

        public void StopTrack(string context) {
            if (trackedTimes.TryGetValue(context, out TrackedTime t))
                t.Stop();
        }

        public void PauseTrack(string context) {
            if (trackedTimes.TryGetValue(context, out TrackedTime t))
                t.Pause();
        }

        public void ResumeTrack(string context) {
            if (trackedTimes.TryGetValue(context, out TrackedTime t))
                t.Resume();
        }

        class TrackedTime {
            private readonly Stopwatch sw = new Stopwatch();
            public List<TimeSpan> Trackings { get; } = new List<TimeSpan>();

            public void Start() {
                sw.Restart();
            }

            public void Stop() {
                sw.Reset();
                Trackings.Add(sw.Elapsed);
            }

            public void Resume() {
                sw.Start();
            }

            public void Pause() {
                sw.Stop();
            }
        }
    }
}
