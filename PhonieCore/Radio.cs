﻿using System;
using Unosquare.WiringPi;

namespace PhonieCore
{
    public class Radio
    {
        private readonly Mp3Player _player;

        public Radio()
        {
            Unosquare.RaspberryIO.Pi.Init<BootstrapWiringPi>();

            _player = new Mp3Player();

            var keyListener = new KeyListener();
            keyListener.OnKeyPressed += HandleKeyPressed;
            keyListener.OnKeyReleased += HandleKeyReleased;

            var rfid = new Rfid();
            rfid.CardDetected += HandleCardDetected;
            rfid.NewCardDetected += HandleNewCardDetected;
            rfid.CardReleased += HandleCardReleased;
        }

        private void HandleCardReleased(string uid)
        {
            Console.WriteLine($"Card Released card: " + uid);
            _player.Stop();
        }

        private void HandleCardDetected(string uid)
        {
        }

        private void HandleNewCardDetected(string uid)
        {
            Console.WriteLine($"New card: " + uid);
            _player.ProcessFolder(uid);
        }

        private void HandleKeyPressed(int key)
        {
            Console.WriteLine("Pressed: " + key);

            switch (key)
            {
                case 6:
                    _player.Pause();
                    break;
                case 16:
                    _player.Play();
                    break;
                case 5:
                    _player.IncreaseVolume();
                    break;
                case 26:
                    _player.DecreaseVolume();
                    break;
            }
        }

        private void HandleKeyReleased(int key)
        {
            Console.WriteLine("Released: " + key);
        }
    }
}
