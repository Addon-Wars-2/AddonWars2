// ==================================================================================================
// <copyright file="NLogLogsAggregatorTarget.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Logging
{
    using AddonWars2.App.Services;
    using AddonWars2.App.Services.Interfaces;
    using NLog;
    using NLog.Targets;

    /// <summary>
    /// A custom NLog target that writes to <see cref="LogEntry"/> object and then
    /// adds it to the collection specified in <see cref="ILogsAggregator"/>.
    /// </summary>
    [Target("NLogLogsAggregatorTarget")]
    public sealed class NLogLogsAggregatorTarget : TargetWithLayout
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogLogsAggregatorTarget"/> class.
        /// </summary>
        /// <param name="logsAggregator">A rerefence to a <see cref="ILogsAggregator"/> object.</param>
        public NLogLogsAggregatorTarget(ILogsAggregator logsAggregator)
        {
            LogsAggregatorInstance = logsAggregator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogLogsAggregatorTarget"/> class.
        /// </summary>
        public NLogLogsAggregatorTarget()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the <see cref="ILogsAggregator"/> instance.
        /// </summary>
        public static ILogsAggregator? LogsAggregatorInstance { get; internal set; }

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        protected override void InitializeTarget()
        {
            base.InitializeTarget();
        }

        /// <inheritdoc/>
        protected override void CloseTarget()
        {
            base.CloseTarget();
        }

        /// <inheritdoc/>
        protected override void Write(LogEventInfo logEvent)
        {
            var logMessage = RenderLogEvent(Layout, logEvent);
            SendLogMessage(logMessage);
        }

        // Sends a log message.
        private void SendLogMessage(string message)
        {
            LogsAggregatorInstance?.LogEntries.Add(new LogEntry(message));
        }

        #endregion Methods
    }
}
