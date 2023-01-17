using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models.ServiceModels;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileParser
{
    /// <summary>
    /// Worker class to handle file processing
    /// </summary>
    public class Worker : BackgroundService
    {
        /// <summary>
        /// Holds the logger object
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Holds the per file logger object
        /// </summary>
        private ILogger _fileLogger;

        /// <summary>
        /// Holds the Configuration object
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// This object is responsible for adding the expenses.
        /// </summary>
        private readonly IExpenseExtractionFlow _expenseExtractionFlow;

        /// <summary>
        /// Holds the base path to read files from
        /// </summary>
        private readonly string _basePath;

        /// <summary>
        /// Constructor to initialize dependencies
        /// </summary>
        /// <param name="logger">Instance of logger object</param>
        /// <param name="config">Instance of Configuration object</param>
        /// <param name="expenseExtractionFlow">Instance of ExpenseExtractionFlow object</param>
        public Worker(ILogger logger,
            IConfiguration config,
            IExpenseExtractionFlow expenseExtractionFlow)
        {
            _logger = logger;
            _config = config;
            _expenseExtractionFlow = expenseExtractionFlow;

            _basePath = _config.GetValue<string>(Constants.BasePath);
        }

        /// <summary>
        /// Method to continously execute
        /// </summary>
        /// <param name="stoppingToken">Cancellation Token</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("File Parser started Execution at {time}", DateTime.Now);
                if (Directory.Exists(_basePath))
                {
                    _logger.LogInformation($"Looking for files");

                    string[] files = Directory.GetFiles(_basePath, Constants.FileNamePattern, SearchOption.TopDirectoryOnly);
                    foreach (string file in files)
                    {
                        string fileName = file.Replace(_basePath, "");
                        SetupFileLogger(fileName);
                        try
                        {
                            List<VendorExpenseDetails> details = new List<VendorExpenseDetails>();
                            if (await _expenseExtractionFlow.Extract(file, _fileLogger))
                            {
                                MoveToProcessed(file, fileName);
                            }
                            else
                            {
                                MoveToError(file, fileName);
                            }
                        }
                        catch (Exception ex)
                        {
                            _fileLogger.LogError("Unable to process the file, Exception: {ex}", ex);
                            MoveToError(file, fileName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method writes the file from souce folder
        /// to Processed folder
        /// </summary>
        /// <param name="filePath">Current file path</param>
        /// <param name="fileName">File Name</param>
        private void MoveToProcessed(string filePath, string fileName)
        {
            string path = _basePath + Constants.ProcessedFolder;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            _logger.LogInformation("File successfully processed");
            File.Move(filePath, path + fileName);
        }

        /// <summary>
        /// This method writes the file from souce folder
        /// to error folder
        /// </summary>
        /// <param name="filePath">Current file path</param>
        /// <param name="fileName">File Name</param>
        private void MoveToError(string filePath, string fileName)
        {
            string path = _basePath + Constants.ErrorFolder;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            _logger.LogInformation("Unable to process the file");
            File.Move(filePath, path + fileName);
        }

        /// <summary>
        /// Setup Logger for every file
        /// </summary>
        /// <param name="fileName">File name</param>
        private void SetupFileLogger(string fileName)
        {
            var loggerFactory = LoggerFactory.Create(logging =>
            {
                logging.AddConfiguration(_config.GetSection("Logging"));
                logging.AddConsole(configure =>
                {
                    configure.TimestampFormat = Constants.LogsDateTimeFormat;
                });
                logging.AddDebug();
                string logFileName = string.Format("Logs/Files/{0}-{1}.log", fileName, "{Date}");
                logging.AddFile(logFileName, isJson: true);
            });

            _fileLogger = loggerFactory.CreateLogger(fileName);
        }
    }
}