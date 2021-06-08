using System.Windows.Input;
using System.Windows.Media;

namespace Intma.ModbusServerService.Configurator
{
    public class ContextAction
    {
        public string Name { get; set; }
        public ICommand Action { get; set; }
        public Brush Icon { get; set; }
    }
}
