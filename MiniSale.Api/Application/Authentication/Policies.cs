namespace MiniSale.Api.Application.Authentication
{
    public static class Policies
    {
        /// <summary>
        /// Admin
        /// </summary>
        public const string Admin = nameof(Admin);
        /// <summary>
        /// Management
        /// </summary>
        public const string Management = nameof(Management);
        /// <summary>
        /// User
        /// </summary>
        public const string Operator = nameof(Operator);

        internal static string[] PolicyNames { get; private set; } = new string[] {
                                                                                    Admin,
                                                                                    Management,
                                                                                    Operator
                                                                                  };
    }
}
