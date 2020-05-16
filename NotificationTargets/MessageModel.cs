namespace NotificationTargets
{
    public class MessageModel
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public override string ToString()
            => $"{Title}: {Message}";
    }
}
