<p align="center">
  <img src="/source/img/Ikas-256.png">
</p>

# Ikas

[![Build status](https://ci.appveyor.com/api/projects/status/hlj5mrc6vh4rl8bu?svg=true)](https://ci.appveyor.com/project/XieZhihao/ikas) [![GitHub release](https://img.shields.io/github/release/zhxie/Ikas.svg)](https://github.com/zhxie/Ikas/releases/latest)

**Ikas** is a schedule and battle statistic client of Splatoon 2 for Windows.

## Features

- Automatic cookie generation of SplatNet
- Get current and coming schedules and shifts
- Get the latest battle including details of players' gears and weapons
- Get the latest salmon run including details of players' weapons and defeated boss salmoniods
- ... and display all of them in a *fancy* way
- Receive notifications of the latest battle and salmon run
- Japanese and English (North America)

## Configuration

Ikas reads and parses configuration from both user.ini, for user information, and config.ini, for system configuration, in the working directory.

### Setup

When Ikas starts at the first time, it will ask for configuration. You can fill your SplatNet cookie in the cookie textbox directly, or obtain one by Automatic Cookie Generation.

1. Obtain a session token. Press "Log In", log in into the Nintendo website opened by Ikas, right click on "Select this Person", copy the link address, and then Ikas will try to get session token.
2. Obtain the SplatNet cookie. When Ikas successfully obtained the session token, it will try to get SplatNet cookie, and Automatic Cookie Generation will be used. **Please read "Security and Privacy" section below carefully before you start.**

## Security and Privacy

### Automatic Cookie Generation

Ikas uses cookies to access SplatNet, get schedule and battle data. This cookie may be obtained automatically using Automatic Cookie Generation which is also instructed in [splatnet2statink/Cookie Generation](https://github.com/frozenpandaman/splatnet2statink#cookie-generation). Please read the following paragraph CAREFULLY before you use Automatic Cookie Generation.

Automatic Cookie Generation involves making a secure request to **two non-Nintendo servers** with minimal, non-identifying information. For details, please refer to [splatnet2statink/api docs](https://github.com/frozenpandaman/splatnet2statink/wiki/api-docs). The developers aim to be 100% transparent about this and provide in-depth information in [splatnet2statink/Cookie Generation/Automatic](https://github.com/frozenpandaman/splatnet2statink#automatic)'s privacy statement.

If you do not want to use Automatic Cookie Generation for obtaining cookie, you may also retrieve one by intercepting into the device's traffic with SplatNet, which is also called MitM. You may follow the [splatnet2statink/mitmproxy instructions](https://github.com/frozenpandaman/splatnet2statink/wiki/mitmproxy-instructions) to get one, and fill in the app's Cookie textbox.

## License

Ikas is licensed under [the MIT License](/LICENSE).

Ikas uses [Json.NET](https://github.com/JamesNK/Newtonsoft.Json) by Newtonsoft licensed under [the MIT License](/LICENSE.Newtonsoft.Json.md), [ini-parser](https://github.com/rickyah/ini-parser) by rickyah licensed under [the MIT License](/LICENSE.ini-parser.md), and [Microsoft.Toolkit.Uwp.Notifications](https://github.com/windows-toolkit/WindowsCommunityToolkit) by Microsoft.Toolkit licensed under [the MIT License](/LICENSE.Microsoft.Toolkit.Uwp.Notifications.md).

Ikas also uses API of [splatnet2statink](https://github.com/frozenpandaman/splatnet2statink) by [eli fessler](https://github.com/frozenpandaman) and flapg API by [NexusMine](https://twitter.com/NexusMine).
