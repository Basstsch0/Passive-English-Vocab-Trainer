using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Text.Json;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;


namespace passiveEnglishVocab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConfigWrapper configWrapper = new ConfigWrapper();
        NotificationEngine engine;
        private List<VocabularyEntry> vocabEntries;
        public MainWindow()
        {
            InitializeComponent();
            engine = new NotificationEngine(Convert.ToInt32(intervalTextBox.Text), Convert.ToInt32(((ComboBoxItem)learningModeComboBox.SelectedItem).Content));
            Configuration config = new ConfigWrapper().getConfiguration();
            vocabPathLabel.Content = "...\\"+System.IO.Path.GetFileName(config.file_path);
            intervalTextBox.Text = config.interval.ToString();
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            if (engine != null)
            { 
                engine.Start();
                startButton.Opacity = 0.6;
                startButton.IsEnabled = false;
                stopButton.Opacity = 1;
                stopButton.IsEnabled = true;
            }
        }
        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            if (engine != null)
            {
                // Color the stop button first 
                stopButton.Opacity = 0.6;
                stopButton.IsEnabled = false;
                // Stopping may take some time (max. Interval length)
                engine.Stop();
                startButton.Opacity = 1;
                startButton.IsEnabled = true;

            }
        }


        private void intervalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(engine != null && intervalTextBox.Text != "")
            {
                engine.Interval = Convert.ToInt32(intervalTextBox.Text);
                Configuration currentConfig = configWrapper.getConfiguration();
                currentConfig.interval = engine.Interval;

                // Save the updated configuration
                configWrapper.setConfiguration(currentConfig);
            }
        }

        // Only allow numbers in the TextBox by validating input as it's typed
        private void intervalTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Regex to match non-numeric input
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        // Optionally allow Backspace for corrections
        private void intervalTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                e.Handled = false;  // Allow Backspace
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            // Initialize the OpenFileDialog and set the initial directory
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.GetDirectoryName(configWrapper.getConfiguration().file_path); // Sets the initial directory to the current config path's directory

            // Show the dialog and check if the user selected a file
            bool? result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                // Update the file path in the configuration
                Configuration currentConfig = configWrapper.getConfiguration();
                currentConfig.file_path = dlg.FileName;

                // Save the updated configuration
                configWrapper.setConfiguration(currentConfig);
            }
        }

        +
        private void learningModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(engine != null)
            {
                Configuration config = configWrapper.getConfiguration();
                config.learning_mode = Convert.ToInt32(((ComboBoxItem)learningModeComboBox.SelectedItem).Content);
                configWrapper.setConfiguration(config);
                engine.LearningMode = Convert.ToInt32(((ComboBoxItem)learningModeComboBox.SelectedItem).Content);

            }
        }
    }
}