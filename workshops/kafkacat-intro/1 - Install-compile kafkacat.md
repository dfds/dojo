# 1. Install/Compile kafkacat

- [1. Install/Compile kafkacat](#1-installcompile-kafkacat)
  - [Linux](#linux)
    - [Debian](#debian)
    - [Compiling](#compiling)
  - [macOS](#macos)
  - [Windows](#windows)
    - [Precompiled binaries](#precompiled-binaries)
    - [Compiling](#compiling-1)
  - [Docker](#docker)

For more up-to-date instructions keep an eye on [github.com/edenhill/kafkacat](https://github.com/edenhill/kafkacat)

## Linux

### Debian

Running `apt-get install kafkacat` on most Debian-based systems should to the trick

### Compiling

Ensure the necessary libraries are installed/available to the compiler.

- librdkafka - https://github.com/edenhill/librdkafka

Instructions:

- `git clone https://github.com/edenhill/kafkacat.git`
- `./configure`
- `make`
- `sudo make install`

Should be available in your PATH now, assuming nothing failed.

## macOS

Install Homebrew if you haven't already (see [brew.sh](https://brew.sh/)).

Run `brew install kafkacat`

kafkacat should now be available for use.

## Windows

### Precompiled binaries

If you'd rather not compile it yourself, ask for a precompiled binary during the workshop.

### Compiling

This assumes that Visual Studio alongside development tools(libraries, msbuild) are installed

- `git clone https://github.com/edenhill/kafkacat.git`
- `cd win32`
- `nuget restore`
- `msbuild`

## Docker

With Docker installed:

Run `docker pull edenhill/kafkacat:1.6.0`

With the image pulled, you can use the following

`docker run --rm -it edenhill/kafkacat:1.6.0 kafkacat` as a base, e.g.

`docker run --rm -it edenhill/kafkacat:1.6.0 kafkacat -b localhost:9092 -G this.is.a.group.id this.is.a.topic`
