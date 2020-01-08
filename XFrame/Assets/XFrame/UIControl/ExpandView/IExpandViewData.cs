using UniRx;

public interface IExpandViewData
{
    ReactiveProperty<string> Id { get; set; }
    ReactiveProperty<string> Name { get; set; }
    ReactiveProperty<bool> IsExpand { get; set; }
    ReactiveProperty<bool> Enabled { get; set; }
}

public class ExpandViewData : IExpandViewData
{
    public ReactiveProperty<string> Id { get; set; } = new ReactiveProperty<string>("");
    public ReactiveProperty<string> Name { get; set; } = new ReactiveProperty<string>("");
    public ReactiveProperty<bool> IsExpand { get; set; } = new ReactiveProperty<bool>(true);
    public ReactiveProperty<bool> Enabled { get; set; } = new ReactiveProperty<bool>(true);
}