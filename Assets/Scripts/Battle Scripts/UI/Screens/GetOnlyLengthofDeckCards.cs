using System.Collections.Generic;
using UI.Meta.Laboratory;

public class GetOnlyLengthofDeckCards : GetDeckLengthHelper
{
    private int _deckDefaultLength = 8;
    public override int GetDeckLength(IReadOnlyList<MetaCardUIHandler> get)
    {
        return _deckDefaultLength;
    }
}
