// ==================================================================================================
// <copyright file="NLogLoggingManagerTarget.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Logging
{
    using AddonWars2.App.Services;
    using NLog;
    using NLog.Targets;

    /// <summary>
    /// A custom NLog target that writes to <see cref="LogEntry"/> object and then
    /// adds it to the collection specified in <see cref="LoggingService"/>.
    /// </summary>
    [Target("NLogLoggingManagerTarget")]
    public sealed class NLogLoggingManagerTarget : TargetWithLayout
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogLoggingManagerTarget"/> class.
        /// </summary>
        /// <param name="loggingManager">A rerefence to a <see cref="LoggingService"/> object.</param>
        public NLogLoggingManagerTarget(LoggingService loggingManager)
        {
            LoggingManagerInstance = loggingManager;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogLoggingManagerTarget"/> class.
        /// </summary>
        public NLogLoggingManagerTarget()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the <see cref="LoggingService"/> service.
        /// </summary>
        public static LoggingService? LoggingManagerInstance { get; internal set; }

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
            LoggingManagerInstance?.LogEntries.Add(new LogEntry(message));
        }

        #endregion Methods
    }
}
