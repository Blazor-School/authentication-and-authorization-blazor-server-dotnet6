namespace Mistake2.Data;

public class MessagingTransferService
{
    public int Counter { get; set; }
    public event EventHandler MessageReceived = (sender, args) => { };
}