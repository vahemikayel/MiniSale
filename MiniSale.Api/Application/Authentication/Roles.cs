namespace MiniSale.Api.Application.Authentication
{
    internal class Roles
    {
        /// <summary>
        /// Administrator
        /// </summary>
        internal const string Admin = nameof(Admin);

        internal const string Operator = nameof(Operator);

        /// <summary>
        /// Manage Automatic jobs configuration
        /// </summary>
        internal const string Manage = nameof(Manage);

        internal static string[] RoleNames { get; private set; } = new string[] {
                                                                                    Admin,
                                                                                    Operator,
                                                                                    Manage
                                                                               };
    }
}
