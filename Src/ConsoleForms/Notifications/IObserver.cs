namespace ConsoleForms.Notifications;

public interface IObserver<TMessage>
{
    public void Update(TMessage message);
}
