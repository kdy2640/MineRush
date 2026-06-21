
public interface IXPProvider
{
    event System.Action<int, int> OnXPChanged;

    int GetCurrentXP();

    int GetRequiredXP();

}