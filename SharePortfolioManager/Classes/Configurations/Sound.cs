using System.IO;
using System.Media;

namespace SharePortfolioManager.Classes
{
    internal static class Sound
    {
        #region Variables

        /// <summary>
        /// Flag if the file for the update finished sound exists
        /// </summary>
        private static bool _updateFinishedSoundFileExist;

        /// <summary>
        /// File name for the update finished sound
        /// </summary>
        private static string _updateFinishedSoundFileName = @"";

        /// <summary>
        /// Sound player for the update finished sound
        /// </summary>
        private static readonly SoundPlayer PlayerUpdateFinished = new SoundPlayer();

        /// <summary>
        /// Flag if the file for the error sound exists
        /// </summary>
        private static bool _errorSoundFileExist;

        /// <summary>
        /// File name for the error sound
        /// </summary>
        private static string _errorSoundFileName = @"-";

        /// <summary>
        /// Sound player for the error sound
        /// </summary>
        private static readonly SoundPlayer PlayerError = new SoundPlayer();

        #endregion Variables

        #region Properties

        /// <summary>
        /// Flag if the update finish sound file should be played
        /// </summary>
        public static bool UpdateFinishedEnable { get; set; }

        /// <summary>
        /// Flag if the error sound file should be played
        /// </summary>
        public static bool ErrorEnable { get; set; }

        /// <summary>
        /// File name for the update finished sound
        /// </summary>
        public static string UpdateFinishedFileName
        {
            get => _updateFinishedSoundFileName;
            set
            {
                if (File.Exists(value))
                {
                    _updateFinishedSoundFileExist = true;
                    _updateFinishedSoundFileName = value;

                    LoadUpdateFinishedSound();

                    return;
                }

                _updateFinishedSoundFileExist = false;
                _updateFinishedSoundFileName = @"";
            }
        }

        /// <summary>
        /// File name for the error sound
        /// </summary>
        public static string ErrorFileName
        {
            get => _errorSoundFileName;
            set
            {
                if (File.Exists(value))
                {
                    _errorSoundFileExist = true;
                    _errorSoundFileName = value;

                    return;
                }

                LoadErrorSound();

                _errorSoundFileExist = false;
                _errorSoundFileName = @"";
            }
        }

        #endregion Properties

        #region Methodes

        /// <summary>
        /// Function which loads update finished sound file
        /// if the file exists
        /// </summary>
        private static void LoadUpdateFinishedSound()
        {
            if (!File.Exists(_updateFinishedSoundFileName)) return;

            PlayerUpdateFinished.SoundLocation = _updateFinishedSoundFileName;
            PlayerUpdateFinished.LoadAsync();
            _updateFinishedSoundFileExist = true;
        }

        /// <summary>
        /// Function which loads error sound file
        /// if the file exists
        /// </summary>
        private static void LoadErrorSound()
        {
            if (!File.Exists(_errorSoundFileName)) return;

            PlayerError.SoundLocation = _errorSoundFileName;
            PlayerError.LoadAsync();
            _errorSoundFileExist = true;
        }

        /// <summary>
        /// Functions which plays the update finished sound file.
        /// It checks if the file exists and if the enable flag is set
        /// </summary>
        public static void PlayUpdateFinishedSound()
        {
            if(_updateFinishedSoundFileExist && UpdateFinishedEnable)
                PlayerUpdateFinished.Play();
        }

        /// <summary>
        /// Functions which plays the error sound file.
        /// It checks if the file exists and if the enable flag is set
        /// </summary>
        public static void PlayErrorSound()
        {
            if (_errorSoundFileExist && ErrorEnable)
                PlayerError.Play();
        }

        #endregion Methodes
    }
}
