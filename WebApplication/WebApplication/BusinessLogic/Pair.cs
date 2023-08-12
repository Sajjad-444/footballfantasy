namespace WebApplication.BusinessLogic;

public class Pair<F, S>
{
    public F first { get; set; }
    public S second { get; set; }

    public Pair()
    {
    }

    public Pair(F first, S second)
    {
        this.first = first;
        this.second = second;
    }
}