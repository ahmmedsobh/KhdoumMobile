using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace KhdoumMobile.Helpers
{
    public class SoundPlayer
    {
        public static void Play(string sound)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            Stream audioStream = assembly.GetManifestResourceStream("KhdoumMobile.Resources.Sounds." + sound + ".mp3");
            var player = CrossSimpleAudioPlayer.Current;
            player.Load(audioStream);
            player.Play();
        }
    }
}
