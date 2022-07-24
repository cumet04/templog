using System;
using System.Collections.Generic;

namespace TempLog
{
    /// <summary>
    /// タイマー発火するロジックとそのstateを保持する
    /// </summary>
    internal class TimerAction
    {
        private readonly TemperatureCounter counter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerAction"/> class.
        /// </summary>
        public TimerAction()
        {
            this.counter = new TemperatureCounter();
        }

        /// <summary>
        /// タイマーのcallbackで渡すaction endpoint
        /// </summary>
        /// <returns>呼び出し元に返す処理結果情報</returns>
        public TimerActionResult Action()
        {
            var values = this.counter.GetValues();
            this.AppendTemperatureRecord(DateTime.Now, values);

            var primaryValue = values[0];
            return new TimerActionResult() { Temperature = primaryValue.value };
        }

        private void AppendTemperatureRecord(DateTime time, (string name, float value)[] values)
        {
            var columns = new List<string>();

            columns.Add(time.
                ToUniversalTime().// UTCに揃えてフォーマット
                ToString("O")); // ISO8601

            foreach (var value in values)
            {
                columns.Add(value.name);
                columns.Add(value.value.ToString());
            }

            Console.WriteLine(string.Join(",", columns));
        }
    }

    internal record struct TimerActionResult
    {
        /// <summary>
        /// 取得された温度[K]
        /// </summary>
        public float Temperature { get; init; }
    }
}
