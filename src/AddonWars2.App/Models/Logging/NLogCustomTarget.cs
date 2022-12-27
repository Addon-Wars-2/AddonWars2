// ==================================================================================================
// <copyright file="NLogCustomTarget.cs" company="Addon-Wars-2">
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
    /// adds it to a collection specified in <see cref="LoggingManager"/>.
    /// </summary>
    [Target("NLogCustomTarget")]
    public sealed class NLogCustomTarget : TargetWithLayout
    {
        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="NLogCustomTarget"/> class.
        /// </summary>
        /// <param name="loggingManager">A rerefence to a <see cref="LoggingManager"/> object.</param>
        public NLogCustomTarget(LoggingManager loggingManager)
        {
            LoggingManagerInstance = loggingManager;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogCustomTarget"/> class.
        /// </summary>
        public NLogCustomTarget()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to <see cref="LoggingManager"/> service.
        /// </summary>
        public static LoggingManager LoggingManagerInstance { get; internal set; }

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

        // Sends the log message.
        private void SendLogMessage(string message)
        {
            LoggingManagerInstance.LogEntries.Add(new LogEntry(message));
        }

        #endregion Methods
    }
}
