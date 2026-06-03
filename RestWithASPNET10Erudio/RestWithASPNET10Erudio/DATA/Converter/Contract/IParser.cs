namespace RestWithASPNET10Erudio.DATA.Converter.Contract
{
    public interface Interface<O, D>
    {
        D Parse(0 origin);
        List<D> ParseList(List<O> origin);
    }
}
