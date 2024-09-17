using System.Reflection;

namespace Bot.Application
{
    public static class ApplicationAssembly
    {
        public static readonly Assembly Instance = typeof(ApplicationAssembly).Assembly;
    }
}
