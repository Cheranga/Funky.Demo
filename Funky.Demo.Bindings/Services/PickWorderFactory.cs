using System;
using Funky.Demo.Functions;
using Funky.Demo.Messages;

namespace Funky.Demo.Services
{
    public class PickWorderFactory : IPickWorkerFactory
    {
        private readonly Random randomIdGenerator;

        public PickWorderFactory()
        {
            this.randomIdGenerator = new Random();
        }

        public PickerData GetPicker(string productCode)
        {
            var pickerData = new PickerData
            {
                Category = nameof(PickerFunction),
                Id = randomIdGenerator.Next(1, 6).ToString()
            };

            return pickerData;
        }
    }
}