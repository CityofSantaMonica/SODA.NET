namespace SODA.Tests.Mocks
{
    class SodaClientMocks
    {
        public static SodaClient New()
        {
            return new SodaClient(StringMocks.Host, StringMocks.NonEmptyInput);
        }
    }
}
