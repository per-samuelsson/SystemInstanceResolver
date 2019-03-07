
using System;
using System.IO;
using System.Linq;
using System.Text;
using Starcounter.BluestarFFI.Env;

namespace SystemInstanceResolver
{
    class Program
    {

        static int Main(string[] args)
        {
            string ArgOrNull(int i) => args.Length > i ? args[i] : null;

            var instanceId = ArgOrNull(0);
            var pathOverride = ArgOrNull(1);
            var currentDirectory = ArgOrNull(2);

            Console.WriteLine(
                $"Resolving {instanceId ?? "<default>"} using path override {pathOverride ?? "<null>"} and current directory {currentDirectory ?? "<null>"}");

            int ResolveConfig(StringBuilder builder) => ScEnv.scenv_resolve_cfgfile(
                builder,
                builder.Capacity,
                instanceId,
                pathOverride,
                currentDirectory);

            var buffer = new StringBuilder(512);

            var ret = ResolveConfig(buffer);
            if (ret < 0)
            {
                var abs = Math.Abs(ret);
                if (abs == 1)
                {
                    Console.Error.WriteLine(nameof(ScEnv.scenv_resolve_cfgfile) + " returned -1");
                    return 1;
                }

                buffer.EnsureCapacity(abs);
                ret = ResolveConfig(buffer);
                if (ret < 0)
                {
                    Console.Error.WriteLine(nameof(ScEnv.scenv_resolve_cfgfile) + " returned " + ret);
                    return 1;
                }
            }

            var path = buffer.ToString();
            if (!File.Exists(path))
            {
                Console.WriteLine($"Resolved to path {path}. File don't exist.");
                return 0;
            }

            ret = ScEnv.scenv_makeuuid(buffer, buffer.Capacity, path);
            if (ret < 0)
            {
                Console.Error.WriteLine(nameof(ScEnv.scenv_makeuuid) + " returned " + ret);
                return 1;
            }

            Console.WriteLine($"Resolved to path {path} and UUID {buffer.ToString()}");
            return 0;
        }
    }
}
