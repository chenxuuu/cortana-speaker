using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Speaker
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = Color.FromArgb(255, 255, 255, 255);
                statusBar.ForegroundColor = Color.FromArgb(255, 0, 0, 0);
                statusBar.BackgroundOpacity = 1;
            }//手机状态栏颜色
        }

        string[] ReadList;

        bool isplay = false;
        int playNow = 0;

        private void ListText()
        {
            ReadList = null;
            ReadList = ReadTextTextBox.Text.Split('\r');
            playNow = 0;
        }

        private void ShowText()
        {
            try
            {
                SpeakTextList1.Text = ReadList[playNow - 1];
            }
            catch
            {
                SpeakTextList1.Text = "";
            }

            try
            {
                SpeakTextList2.Text = ReadList[playNow];
            }
            catch
            {
                SpeakTextList2.Text = "";
            }

            try
            {
                SpeakTextList3.Text = ReadList[playNow + 1];
            }
            catch
            {
                SpeakTextList3.Text = "";
            }

            try
            {
                SpeakTextList4.Text = ReadList[playNow + 2];
            }
            catch
            {
                SpeakTextList4.Text = "";
            }
        }

        private void PlayNext()
        {
            playNow++;
            try
            {
                ShowText();
                Speak(ReadList[playNow]);
            }
            catch
            {
                
            }
            isplay = false;
        }

        private void PlayNow()
        {
            try
            {
                ShowText();
                Speak(ReadList[playNow]);
            }
            catch
            {
                isplay = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ListText();
            PlayNow();
            isplay = true;
        }


        private SpeechSynthesizer _synth = new SpeechSynthesizer();

        private async void SpeakWay(string value, int hz,string rate,int volume)
        {
            string Ssml = $"<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='zh-CN'><prosody pitch='{hz}Hz' rate='{rate}' volume= '{volume}'>{value}</prosody></speak>";
            var stream = await _synth.SynthesizeSsmlToStreamAsync(Ssml);

            
            // 将音频播放出来。
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }

        private void Speak(string value)
        {
            int hz = 100, volume;
            string rate;
            try
            {
                hz = int.Parse(SpeakHzTextBox.Text);
            }
            catch { }
            
            rate = ((ComboBoxItem)SpeakSpeedComboBox.SelectedItem).Content.ToString();
            volume = (int)SpeakvolumeSlider.Value;
            SpeakWay(value, hz, rate, volume);
        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (isplay)
            {
                playNow++;
                PlayNow();
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PlayNow();
            isplay = true;
        }

        private void PlayOneButton_Click(object sender, RoutedEventArgs e)
        {
            PlayNext();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            isplay = false;
            mediaElement.Stop();
        }
    }
}
