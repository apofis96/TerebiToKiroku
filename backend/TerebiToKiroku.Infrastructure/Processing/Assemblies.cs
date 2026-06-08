using System.Reflection;
using TerebiToKiroku.Application.Orders.PlaceCustomerOrder;

namespace TerebiToKiroku.Infrastructure.Processing
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(PlaceCustomerOrderCommand).Assembly;
    }
}