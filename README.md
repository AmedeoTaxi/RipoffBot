# DiscordBot

A bot for Discord

## Description

It is a bot able to play an audio by searching on Youtube

## Usage

Pretty simple:
1. go to the [Discord developer portal](https://discord.com/developers/applications) and create a new bot (all the informations are in the Discord documentation or easily retrievable online)
2. get the authentication token of the newly created bot (**warning:** the token will be visible and copyable only once, if you lose, you will need to create a new one)
3. paste the token into the `App.config` file in the value of "DiscordToken"
4. add a valid path to the `App.config` file in the value of "filePath"
5. from the Discord Developer Portal, add the bot to a Discord server (you must be the administrator of a server to add the bot)
6. run the application
7. connect to a voice channel
8. type "!p "anything you want to search""
9. enjoy

## Disclaimers

At the point I'm writing this, this software has MANY problems. It's not optimized, it's not really as I intended, it's not even really complete.
Use at own risk.