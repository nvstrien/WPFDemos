using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnMicroWithSimpleInjectorDemo.ViewModels
{
    public class ShellViewModel
    {
        public async Task Button1()
        {
            Debug.Print("Hello world");

            var data = await GetSampleDataAsync();

            foreach (var item in data)
            {
                Debug.Print(item);
            }
        }

        public async Task<IEnumerable<string>> GetSampleDataAsync()
        {
            // method simulating getting async data
            var data = new List<string>() { "hello", "world" };

            return await Task.FromResult(data);
        }

    }
}
