using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intma.ModbusServerService.Configurator
{
    public static class DataTypes
    {
        public static IReadOnlyList<string> MbtcpDataTypes { get; } = new List<string>() { "Float", "Int", "Short" };
    }
}
