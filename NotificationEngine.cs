using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Threading;

namespace passiveEnglishVocab
{
    internal class NotificationEngine
    {
        private Thread _thread;
        private int _interval;
        private readonly VocabularyParser _parser;
        private bool _running;
        private int _learning_mode;

        public int Interval
        {
            get => _interval;
            set
            {
                _interval = value;
                // Free for future updates
            }
        }

        public int LearningMode
        {
            get => _learning_mode;
            set
            {
                _learning_mode = value;
                // Free for future updates
            }
        }

        public NotificationEngine(int interval, int learning_mode)
        {
            Interval = interval;
            this._parser = new VocabularyParser();
            this._running = true;
            this._thread = new Thread(RunLoop);
            this._learning_mode = learning_mode;
        }

        public void Start()
        {
            if (!_thread.IsAlive)
            {
                _thread.Start();
            }
        }

        public void Stop()
        {
            _running = false;
            _thread.Join();
            _running = true;
            _thread = new Thread(RunLoop);
        }

        private void RunLoop()
        {
            Random random = new Random();

            while (_running)
            {
                Thread.Sleep(_interval * 1000);
                if (!_running)
                {
                    break;
                }
                int randomId = random.Next(1, 501);
                VocabularyEntry entry = _parser.GetVocabularyById(randomId);

                if (entry != null)
                {
                    switch (_learning_mode)
                    {
                        case 1 :
                            new ToastContentBuilder()
                               .AddArgument("word", entry.id)
                               .AddText($"Word: {entry.word}")
                               .AddText($"Translation: {entry.translation}")
                               //.AddText($"Description: {entry.description}")
                               .AddText($"Synonyms: {string.Join(", ", entry.synonyms ?? new List<string>())}")
                               .SetToastDuration(ToastDuration.Long)
                               .Show();
                            break;

                        case 2:
                            new ToastContentBuilder()
                               .AddArgument("wordId", entry.id) // Argument to track which word it is
                               .AddText("Guess the Word!")
                               .AddText($"Word: {entry.word}")
                               .AddText($"Synonyms: {string.Join(", ", entry.synonyms ?? new List<string>())}")
                               .AddButton(new ToastButton()
                                   .SetContent("Check Answer")
                                   .AddArgument("action", "checkAnswer")
                                   .SetBackgroundActivation())   // Brings up the app to check answer
                               .SetToastDuration(ToastDuration.Long)
                               .Show();
                            break;
                    }

                }
            }
        }

        private void CheckAnswer(string userInput, VocabularyEntry entry)
        {
            if (string.Equals(userInput, entry.word, StringComparison.OrdinalIgnoreCase))
            {
                ShowFollowUpNotification("Correct!");
            }
            else
            {
                ShowFollowUpNotification($"Incorrect! The correct word is: {entry.translation}");
            }
        }
        private void ShowFollowUpNotification(string message)
        {
            new ToastContentBuilder()
                .AddText(message)
                .SetToastDuration(ToastDuration.Short)
                .Show();
        }



    }
}
