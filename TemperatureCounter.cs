using System.Collections.Generic;
using System.Diagnostics;

namespace TempLog
{
    /// <summary>
    /// 温度取得.
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform("Windows")]
    internal class TemperatureCounter
    {
        private List<PerformanceCounter> counters = new List<PerformanceCounter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TemperatureCounter"/> class.
        /// </summary>
        public TemperatureCounter()
        {
            var categoryName = "Thermal Zone Information";
            var counterName = "Temperature";

            var cat = new PerformanceCounterCategory(categoryName);
            var names = cat.GetInstanceNames();
            foreach (var name in names)
            {
                this.counters.Add(new PerformanceCounter(categoryName, counterName, name));
            }
        }

        /// <summary>
        /// hoge.
        /// </summary>
        /// <returns>instanceごとのname/valueペア.</returns>
        public (string name, float value)[] GetValues()
        {
            var results = new List<(string name, float value)>();
            foreach (var counter in this.counters)
            {
                results.Add((name: counter.InstanceName, value: counter.NextValue()));
            }

            return results.ToArray();
        }
    }
}
