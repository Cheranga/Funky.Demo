using Funky.Demo.Functions;
using Funky.Demo.Messages;

namespace Funky.Demo.Services
{
    public interface IPickWorkerFactory
    {
        PickerData GetPicker(string productCode);
    }
}