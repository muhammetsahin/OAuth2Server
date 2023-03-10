namespace Base64
{
    internal enum Mask
    {
        NoPadding = 0x02,
        UrlSafe = 0x04,
    }


    public enum Variant
    {
        Original = 1,
        OriginalNoPadding = Original | Mask.NoPadding,
        UrlSafe = 1 | Mask.UrlSafe,
        UrlSafeNoPadding = UrlSafe | Mask.NoPadding,
    }
}