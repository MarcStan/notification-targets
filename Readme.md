# Notification targets

A (non-exhaustive) collection of services that can receive notifications.

Implemented with a strategy pattern that allows broadcasting notifications to multiple services at once.

## Matrix

Post messages to a matrix room via an account. Requires you to setup a "bot account" (or use your own account).

Note that this notification sends messages without end-to-end encryption.

When setting up bots, it is common for the bot to require an "access token" in order to work. Access tokens authenticate bots to the server so that they can function. Access tokens should be kept secret and never shared.

1. In a private/incognito browser window, open Element.
2. Log in to the account you want to get the access token for, such as the bot's account.
3. Click on the bot's name in the top left corner then "Settings".
4. (Optional) Set your bot's display name and avatar.
5. Click the "Help & About" tab (left side of the dialog).
6. Scroll to the bottom and click the <click to reveal> part of Access Token: <click to reveal>.
7. Copy your access token to a safe place, like the bot's configuration file.
8. Do not log out. Instead, just close the window. If you used a private browsing session, you should be able to still use Element for your own account. Logging out deletes the access token from the server, making the bot unable to use it.


See also [Getting your access token from Element](https://t2bot.io/docs/access_tokens/).

``` json
{
  "Notification": {
    "Matrix": {
      "RoomId": "go to room settings -> advanced to find the internal room id",
      "AccessToken": "log into the account, goto settings -> Help & about -> click to reveal (do not log out of the account or the token is revoked)"
    }
  }
}
```

## Slack

Post a message to a slack channel.

Requires you to set up a [slack app](https://api.slack.com/messaging/webhooks) in your workspace.

``` json
{
    "Slack": {
      "Webhook": "Create a slack app to get your incoming webhook url: https://api.slack.com/messaging/webhooks"
    }
  }
}
```
## Sendgrid

Send an email via [Sendgrid](https://sendgrid.com/).

Requires you to have a sendgrid account (free tier allows ~100 emails per day).

In your sendgrid account you must generate an api key that needs to be configured (along with the sender address and target address).

``` json
{
  "Notification": {
    "SendGrid": {
      "Key": "Generate an api key with Mail.send permissions in the portal",
      "Sender": "The email of the originating domain (token must be valid for it)",
      "To": "to whom it may concern.."
    }
  }
}
```