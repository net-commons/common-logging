using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Common.Logging;
using Common.Logging.Simple;
using System.Threading;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace CustomLogger
{
    public partial class MainPage : UserControl
    {
        private CapturingLoggerFactoryAdapter _capturingLogger = new CapturingLoggerFactoryAdapter();
        private ObservableCollection<string> _logMessages = new ObservableCollection<string>();
        
        
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // set our custom factory adapter
            TestLogging(new MyLoggerFactoryAdapter(LogLevel.All), 
                    "Some Log Output To my logger");

            // Log to console logger
            TestLogging(new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter(LogLevel.All, true, true ,true, "d"), 
                    "Some Output To Console logger");

            LogManager.Adapter = _capturingLogger;

            logListbox.ItemsSource = _logMessages;

            // Since CapturingLoggerFactoryAdapter does not have an "Added" event, poll it each second
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(_timer_Tick);
            timer.Start();
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            // Go through all logging events since the last time we had any and display them
            foreach (CapturingLoggerEvent logEvent in _capturingLogger.LoggerEvents)
            {
                _logMessages.Add(logEvent.RenderedMessage);   
            }
            _capturingLogger.Clear();
        }

        private static void TestLogging(ILoggerFactoryAdapter factory, string message)
        {
            // set our custom factory adapter
            LogManager.Adapter = factory;

            // obtain logger instance
            ILog log = LogManager.GetCurrentClassLogger();

            // log something
            log.Info(message);
            log.Debug(message);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ILog log = LogManager.GetCurrentClassLogger();
            log.Info(logTextBox.Text);
        }
    }
}
