using Google.Cloud.Speech.V1;
using NAudio.Wave;

using System;
using System.Linq;
using System.Windows.Forms;

namespace AI_LAB_4
{
    public partial class MainForm : Form
    {
        private static string TEMP_AUDIO_PATH = Environment.CurrentDirectory + "/temp.wav";
        private static string GOOGLE_API_CREDS_PATH = Environment.CurrentDirectory + "\\iad-lab-4-24b45bc93c76.json";

        private static WaveFormat Format { get; set; } = new WaveFormat(16000, 16, 1);

        private static WaveInEvent Capturing { get; set; }
        private static WaveFileWriter Writer { get; set; } 

        public MainForm()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (Capturing != null)
                return;

            Capturing = new WaveInEvent();
            Capturing.WaveFormat = Format;

            Writer = new WaveFileWriter(TEMP_AUDIO_PATH, Format);

            Capturing.DataAvailable += Capture_DataAvailable;
            Capturing.RecordingStopped += Capture_RecordingStopped;

            Capturing.StartRecording();
        }

        private void Capture_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (Capturing != null)
            {
                Capturing.DataAvailable -= Capture_DataAvailable;
                Capturing.RecordingStopped -= Capture_RecordingStopped;

                Capturing.Dispose();
                Capturing = null;
            }

            if(Writer != null)
            {
                Writer.Dispose();
                Writer = null;
            }

            Recognize();
        }

        private void Capture_DataAvailable(object sender, WaveInEventArgs e)
        {
            Writer.Write(e.Buffer, 0, e.BytesRecorded);
            Writer.Flush();
        }

        private void FinishButton_Click(object sender, EventArgs e)
        {
            if (Capturing != null)
                Capturing.StopRecording();
            else
                Recognize();
        }

        private void Recognize()
        { 
            SpeechClientBuilder builder = new SpeechClientBuilder();
            builder.CredentialsPath = GOOGLE_API_CREDS_PATH;

            SpeechClient client = builder.Build();
            RecognizeRequest request = new RecognizeRequest()
            {
                Audio = RecognitionAudio.FromFile(TEMP_AUDIO_PATH),
                Config = new RecognitionConfig()
                {
                    Encoding = RecognitionConfig.Types.AudioEncoding.EncodingUnspecified,
                    LanguageCode = "ru-RU",
                    EnableWordTimeOffsets = false
                }
            };
            RecognizeResponse response = client.Recognize(request);

            Result.Text = string.Join("\n", response.Results.Select(
                result => result.Alternatives[0].Transcript
            ));
        }
    }
}
