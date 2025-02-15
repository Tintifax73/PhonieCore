using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetCoreAudio;

namespace PhonieCore
{
    public class Mp3Player
    {
        private readonly NetCoreAudio.Player _player;
        private readonly Library _library;

        private const string StopFile = "STOP";
        private const string PauseFile = "PAUSE";
        private const string PlayFile = "PLAY";
        private const string SpotifyFile = "SPOTIFY";

        private int volume = 50;

        public Mp3Player()
        {
            _library = new Library();

            _player = new NetCoreAudio.Player();

            SetVolume(volume);
        }

        public void ProcessFolder(string uid)
        {
            string folder = _library.GetFolderForId(uid);
            var files = Directory.EnumerateFiles(folder).ToArray();

            foreach (string file in files)
            {
                Console.WriteLine(file);
            }

            if (files.Any(f => f.Contains(StopFile)))
            {
                Stop();
            }
            else if (files.Any(f => f.Contains(PlayFile)))
            {
                Play();
            }
            else if (files.Any(f => f.Contains(PauseFile)))
            {
                Pause();
            }
            else if(files.Any(f => f.EndsWith("mp3")))
            {
                Play(files);
            }
        }

        public void Play()
        {
            //_mopidyClient.Play();
        }

        public void Next()
        {
            //_mopidyClient.Next();
        }

        public void Previous()
        {
            //_mopidyClient.Previous();
        }

        public void Seek(int sec)
        {
            //_mopidyClient.Seek(sec);
        }

        private void SetVolume(int volume)
        {
            //_mopidyClient.SetVolume(volume);
        }

        public void IncreaseVolume()
        {
            if (volume <= 95)
            {
                volume += 5;
            }
            //_mopidyClient.SetVolume(volume);
        }

        public void DecreaseVolume()
        {
            if (volume >= 5)
            {
                volume -= 5;
            }
            //_mopidyClient.SetVolume(volume);
        }

        private void Play(IEnumerable<string> files)
        {
            string arguments = string.Join(" ", files);
            Console.WriteLine("Play files: " + arguments);

            Stop();

         
            _player.Play(files.FirstOrDefault()).Wait();
        }

        private void PlaySpotify(string uri)
        {
            Console.WriteLine("Play Spotify: " + uri);

            Stop();

            //_mopidyClient.ClearTracks();            
            // _mopidyClient.AddTrack(uri);            

            //_mopidyClient.Play();
        }

        public void Stop()
        {
            Console.WriteLine("Stop");
            _player.Stop();      
        }

        public void Pause()
        {
            Console.WriteLine("Pause");
            if (!_player.Paused) 
            {
                _player.Pause();
            }
            else
            {
                _player.Resume();
            }
        }       
    }
}
