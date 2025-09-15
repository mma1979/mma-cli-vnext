namespace MmaSolution.EntityFramework.Infrastrcture.ValueConverters;

public class SecureDataConverter : ValueConverter<string, string>
{

    public SecureDataConverter() : base(
        value => value == null ? null : EncryptionHelper.Encrypt2(value),
        value => value == null ? null : EncryptionHelper.Decrypt2(value)
        )
    { }
}