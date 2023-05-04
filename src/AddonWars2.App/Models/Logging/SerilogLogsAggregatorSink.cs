// ==================================================================================================
// <copyright file="SerilogLogsAggregatorSink.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Logging
{
    using System.IO;
    using AddonWars2.App.Services.Interfaces;
    using Serilog.Core;
    using Serilog.Events;
    using Serilog.Formatting;
    using Serilog.Formatting.Display;

    /// <summary>
    /// A custom Serilog sink that writes to <see cref="LogEntry"/> object and then
    /// adds it to the collection specified in <see cref="ILogsAggregator"/>.
    /// </summary>
    public sealed class SerilogLogsAggregatorSink : ILogEventSink
    {
        #region Fields

        private readonly ITextFormatter _textFormatter = new MessageTemplateTextFormatter("[{Timestamp:yyyy-MM-dd HH:mm:ss.ffff zzz}] [{Level:u3}] [{Namespace}.{Method}] {Message}{Exception}");  // no newline

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogLogsAggregatorSink"/> class.
        /// </summary>
        /// <param name="logsAggregator">A rerefence to a <see cref="ILogsAggregator"/> object.</param>
        public SerilogLogsAggregatorSink(ILogsAggregator logsAggregator)
        {
            LogsAggregatorInstance = logsAggregator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogLogsAggregatorSink"/> class.
        /// </summary>
        public SerilogLogsAggregatorSink()
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
        public void Emit(LogEvent logEvent)
        {
            var strWriter = new StringWriter();
            _textFormatter.Format(logEvent, strWriter);
            SendLogMessage(strWriter.ToString());
        }

        // Sends a log message.
        private void SendLogMessage(string message)
        {
            LogsAggregatorInstance?.LogEntries.Add(new LogEntry(message));
        }

        #endregion Methods
    }
}
