# PhonieCore
A simple RFID music box implemented in .NET Core

Inspired by the [Phoniebox project](http://phoniebox.de)

## What it does
- Plays mp3 file(s) when a corresponding RFID tag is recognized (uid)
- When a new tag is recognized a folder is created in a samba share
- When an existing tag is recognized the media files in the folder are played. 
- The folder that corresponds with the current tag is marked with an @ in the end (so that one knows where to add the media files for that tag)

## Hardware
- Raspberry pi 3b running raspbian buster lite
- rc522 RFID reader connected via SPI
- Powerbank with pass through changing (to enable use and charge at the same time)
- Nice box that fits all components

## Installation
Update your raspbian
```
sudo apt update
sudo apt upgrade
```

Samba shares
```
sudo apt-get install samba
sudo nano /etc/samba/smb.conf

add these lines at the bottom to get two open shares for the executable and the media):
path = /media/
public = yes
writable = yes
guest ok = yes

[core]
path = /opt/phoniecore
public = yes
writable = yes
guests ok = yes
```

Enable Zeroconf (use hostname for connections) 
```
sudo apt-get install avahi-daemon
sudo nano /etc/hostname
```

Get mpg123
```
sudo apt-get install mpg123
```

Install .Net Core
https://dotnet.microsoft.com/download/dotnet-core

Set up systemd
https://devblogs.microsoft.com/dotnet/net-core-and-systemd/

Binaries
```
Copy the binaries to the core share
```

## ToDo
- [ ] Fix playing files with whitespace
- [ ] Bring readme up to date

